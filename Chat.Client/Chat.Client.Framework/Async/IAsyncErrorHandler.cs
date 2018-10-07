using System;

namespace Chat.Client.Framework.Async
{
    public interface IAsyncErrorHandler
    {
        event AsyncExceptionHandler ExceptionOcurred;
        void RaiseExceptionOcurred(Exception exception);
        void RaiseExceptionOcurred(string message);
        void RaiseExceptionOcurred(string message, Exception innerException);
    }
}
