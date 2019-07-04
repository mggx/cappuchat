using CappuChat;
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
            if (TryGetUserIDFromCache(message.Receiver.Username, out var targetConnectionId))
                Clients.Client(targetConnectionId).OnPrivateMessageReceived(message);
        }
    }
}
