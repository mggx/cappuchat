using System;

namespace Chat.Client.SignalHelpers.Contracts.Exceptions
{
    public class VoteFailedException : Exception
    {
        public VoteFailedException()
        {
        }

        public VoteFailedException(string message) : base(message)
        {
        }

        public VoteFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
