using System;

namespace Chat.Server.DataAccess.Exceptions
{
    public class GroupException : Exception
    {
        public GroupException()
        {
        }

        public GroupException(string message) : base(message)
        {
        }

        public GroupException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
