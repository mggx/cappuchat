using CappuChat;
using System;

namespace Chat.Client.SignalHelpers.Contracts.Events
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public SimpleMessage ReceivedMessage { get; }

        public MessageReceivedEventArgs(SimpleMessage receivedMessage)
        {
            if (receivedMessage == null)
                throw new ArgumentNullException(nameof(receivedMessage));

            if (receivedMessage.Sender == null)
                throw new InvalidOperationException(CappuChat.Properties.Errors.MessageReceivedEventArgsWithoutSender);

            ReceivedMessage = receivedMessage;
        }
    }
}
