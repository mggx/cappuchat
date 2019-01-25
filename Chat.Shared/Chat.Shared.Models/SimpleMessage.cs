using System;

namespace Chat.Shared.Models
{
    public class SimpleMessage
    {
        public SimpleUser Sender { get; set; }
        public SimpleUser Receiver { get; set; }
        public string Message { get; set; }

        public DateTime MessageSentDateTime { get; set; }

        public bool IsLocalMessage { get; set; }

        public SimpleMessage()
        {
        }

        public SimpleMessage(SimpleUser sender, string message)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender), "Cannot create SimpleMessage. Given sender is null.");

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Cannot create SimpleMessage. Given message is invalid.");

            Sender = sender;
            Message = message;
        }

        public SimpleMessage(SimpleUser sender, SimpleUser receiver, string message) : this(sender, message)
        {
            Receiver = receiver;
        }
    }
}
