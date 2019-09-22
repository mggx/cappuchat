using System;

namespace CappuChat
{
    [Serializable]
    public class SimpleMessage
    {
        public SimpleUser Sender { get; set; }
        public SimpleUser Receiver { get; set; }
        public string Message { get; set; }

        public string ImageName { get; set; }
        public string Base64ImageString { get; set; }

        public DateTime MessageSentDateTime { get; set; }

        public bool IsLocalMessage { get; set; }

        public SimpleMessage()
        {
        }

        public SimpleMessage(SimpleUser sender, string message)
        {
            Sender = sender ?? throw new ArgumentNullException(nameof(sender));

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException(CappuChat.Properties.Strings.Error_MessageCannotBeEmpty, nameof(sender));
            Message = message;
        }

        public SimpleMessage(SimpleUser sender, SimpleUser receiver, string message) : this(sender, message)
        {
            Receiver = receiver;
        }
    }
}
