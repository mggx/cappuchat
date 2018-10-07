using System;

namespace Chat.Server.DataAccess.Exceptions
{
    public class UserCreationFailedException : Exception
    {
        public UserCreationFailedException(string message) : base(message)
        {
        }

        public UserCreationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
