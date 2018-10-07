using System;

namespace Chat.Client.Framework.Async
{
    public class AsyncExceptionHelper : IAsyncErrorHandler
    {
        public event AsyncExceptionHandler ExceptionOcurred;

        public void RaiseExceptionOcurred(string message)
        {
            AsyncException exception = new AsyncException(message);
            RaiseExceptionOcurred(exception);
        }

        public void RaiseExceptionOcurred(string message, Exception innerException)
        {
            AsyncException exception = new AsyncException(message, innerException);
            RaiseExceptionOcurred(exception);
        }

        public void RaiseExceptionOcurred(Exception exception)
        {
            ExceptionOcurred?.Invoke(exception);
        }
    }
}
