using System;
using System.Collections.Generic;
using Chat.Responses;
using Chat.Shared.Models;

namespace Chat.Server.Hubs
{
    public class ChatHub : BaseHub
    {
        public static IList<SimpleMessage> PublicChatMessages { get; set; } = new List<SimpleMessage>();

        public void SendMessage(SimpleMessage message)
        {
            Console.WriteLine($"SendMessage from {message.Sender.Username} to {message.Receiver.Username} received. {Environment.NewLine} Message: {message.Message}");
            PublicChatMessages.Add(message);
            Clients.Others.OnMessageReceived(message);
        }

        public void SendPrivateMessage(SimpleMessage message)
        {
            if (!UsernameConnectionIdCache.ContainsKey(message.Receiver.Username))
            {
                //if user exists in database, add to pending messages
                return;
            }

            string targetConnectionId = UsernameConnectionIdCache[message.Receiver.Username];
            Clients.Client(targetConnectionId).OnPrivateMessageReceived(message);
        }

        public GetOnlineUsersResponse GetOnlineUsers()
        {
            Console.WriteLine("GetOnlineUsers received.");
            GetOnlineUsersResponse response = new GetOnlineUsersResponse{ Success = true };

            IList<SimpleUser> onlineUsers = new List<SimpleUser>();
            foreach (var pair in UsernameConnectionIdCache)
            {
                onlineUsers.Add(new SimpleUser(pair.Key));
            }

            response.OnlineUserList = onlineUsers;
            return response;
        }
    }
}
