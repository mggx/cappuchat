using System;
using Chat.Shared.Models;

namespace Chat.Client.SignalHelpers.Contracts.Events
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public SimpleMessage ReceivedMessage { get; }

        public MessageReceivedEventArgs(SimpleMessage receivedMessage)
        {
            if (receivedMessage == null)
                throw new ArgumentNullException(nameof(receivedMessage), "Cannot create MessageReceivedEventArgs. Given receivedMessage is null.");

            if (receivedMessage.Sender == null)
                throw new ArgumentNullException(nameof(receivedMessage.Sender), "Cannot create MessageReceivedEventArgs. Given receivedMessage.Sender is null.");

            ReceivedMessage = receivedMessage;
        }
    }
}
