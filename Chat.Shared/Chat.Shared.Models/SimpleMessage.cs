using System;

namespace Chat.Shared.Models
{
    public class SimpleMessage
    {
        public SimpleUser Sender { get; }
        public SimpleUser Receiver { get; }
        public string Message { get; }
        public DateTime MessageSentDateTime { get; set; }

        public bool IsLocalMessage { get; set; }

        public SimpleMessage(SimpleUser sender, SimpleUser receiver, string message)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender), "Cannot create SimpleMessage. Given sender is null.");

            if (receiver == null)
                throw new ArgumentNullException(nameof(receiver), "Cannot create SimpleMessage. Given receiver is null.");

            if (message == null)
                throw new ArgumentNullException(nameof(message), "Cannot create SimpleMessage. Given message is null.");

            Sender = sender;
            Receiver = receiver;
            Message = message;
        }
    }
}
