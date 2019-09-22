using CappuChat;
using CappuChat.DTOs;
using Chat.Server.Controller;
using System;
using System.Collections.Generic;

namespace Chat.Server.Hubs
{
    public class ChatHub : BaseHub
    {
        private static Dictionary<string, IList<SimpleMessage>> PendingMessages { get; } = new Dictionary<string, IList<SimpleMessage>>();

        public void SendMessage(SimpleMessage message)
        {
            Console.WriteLine($"SendMessage from {message.Sender?.Username} to {message.Receiver?.Username} received. {Environment.NewLine} Message: {message.Message}");
            Clients.Others.OnMessageReceived(message);
        }

        public void SendPrivateMessage(SimpleMessage message)
        {
            var targetUsername = message.Receiver.Username;

            if (TryGetUserIDFromCache(targetUsername, out var targetConnectionId))
                Clients.Client(targetConnectionId).OnPrivateMessageReceived(message);
            else if (UserController.GetUser(targetUsername) != null)
            {
                var normalizedUsername = NormalizeUsernameForCache(targetUsername);
                if (PendingMessages.ContainsKey(normalizedUsername))
                {
                    PendingMessages[normalizedUsername].Add(message);
                }
                else
                {
                    PendingMessages.Add(normalizedUsername, new List<SimpleMessage> { message });
                }
            }
        }

        public GetOnlineUsersResponse GetOnlineUsers()
        {
            Console.WriteLine("GetOnlineUsers received.");
            return new GetOnlineUsersResponse { OnlineUserList = GetOnlineUserList() };
        }

        public GetPendingMessagesResponse GetPendingMessages()
        {
            var username = GetUsernameByConnectionId(Context.ConnectionId);
            if (TryGetPendingMessagesFromUser(username, out var pendingMessages))
                return new GetPendingMessagesResponse(pendingMessages, true);
            return new GetPendingMessagesResponse() { ErrorMessage = $"Could not find pending messages for user {username}", Success = true };
        }

        private static bool TryGetPendingMessagesFromUser(string username, out IList<SimpleMessage> pendingMessages)
        {
            var normalizedUsername = NormalizeUsernameForCache(username);
            if (PendingMessages.TryGetValue(normalizedUsername, out pendingMessages))
            {
                PendingMessages.Remove(normalizedUsername);
                return true;
            }

            return false;
        }
    }
}
