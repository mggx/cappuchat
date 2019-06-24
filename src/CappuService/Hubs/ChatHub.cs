using CappuChat;
using CappuChat.DTOs;
using System;

namespace Chat.Server.Hubs
{
    public class ChatHub : BaseHub
    {
        public void SendMessage(SimpleMessage message)
        {
            Console.WriteLine($"SendMessage from {message.Sender?.Username} to {message.Receiver?.Username} received. {Environment.NewLine} Message: {message.Message}");
            Clients.Others.OnMessageReceived(message);
        }

        public void SendPrivateMessage(SimpleMessage message)
        {
            if (!UsernameConnectionIdCache.ContainsKey(message.Receiver.Username.ToLower()))
            {
                //if user exists in database, add to pending messages
                return;
            }

            string targetConnectionId = UsernameConnectionIdCache[message.Receiver.Username.ToLower()];
            Clients.Client(targetConnectionId).OnPrivateMessageReceived(message);
        }

        public GetOnlineUsersResponse GetOnlineUsers()
        {
            Console.WriteLine("GetOnlineUsers received.");
            GetOnlineUsersResponse response = new GetOnlineUsersResponse { OnlineUserList = base.GetOnlineUsers() };
            return response;
        }
    }
}
