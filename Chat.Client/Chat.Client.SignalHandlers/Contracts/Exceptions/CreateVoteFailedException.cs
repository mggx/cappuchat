using System;

namespace Chat.Client.SignalHelpers.Contracts.Exceptions
{
    public class CreateVoteFailedException : Exception
    {
        public CreateVoteFailedException()
        {
        }

        public CreateVoteFailedException(string message) : base(message)
        {
        }

        public CreateVoteFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
