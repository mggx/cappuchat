using System;

namespace Chat.Client.SignalHelpers.Contracts.Exceptions
{
    public class NullServerResponseException : Exception
    {
        public NullServerResponseException(string message) : base(message)
        {
        }

        public NullServerResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
