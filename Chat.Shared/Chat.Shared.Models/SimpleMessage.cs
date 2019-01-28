using System;

namespace Chat.Shared.Models
{
    [Serializable]
    public class SimpleMessage
    {
        public SimpleUser Sender { get; set; }
        public SimpleUser Receiver { get; set; }
        public string Message { get; set; }
        //public int Reactios { get; set; }

        public DateTime MessageSentDateTime { get; set; }

        public bool IsLocalMessage { get; set; }

        public SimpleMessage()
        {
        }

        public SimpleMessage(SimpleUser sender, string message)
        //public SimpleMessage(SimpleUser sender, string message, int reactions)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender), "Cannot create SimpleMessage. Given sender is null.");

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Cannot create SimpleMessage. Given message is invalid.");

            Sender = sender;
            Message = message;
            //Reactios = reactions;
        }

        public SimpleMessage(SimpleUser sender, SimpleUser receiver, string message) : this(sender, message)
        //public SimpleMessage(SimpleUser sender, SimpleUser receiver, string message, int reactions) : this(sender, message, reactions)
        {
            Receiver = receiver;
        }
    }
}
