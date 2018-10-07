using System;

namespace Chat.Client.Framework.Async
{
    public class AsyncException : Exception
    {
        public AsyncException(string message) : base(message)
        {
        }

        public AsyncException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
