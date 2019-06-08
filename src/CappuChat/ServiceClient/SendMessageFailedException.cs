using System;

namespace Chat.Client.SignalHelpers.Contracts.Exceptions
{
    public class SendMessageFailedException : Exception
    {
        public SendMessageFailedException(string message) : base(message)
        {
        }

        public SendMessageFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
