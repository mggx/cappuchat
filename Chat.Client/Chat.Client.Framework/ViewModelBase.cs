using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Chat.Client.Framework.Async;

namespace Chat.Client.Framework
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected readonly IAsyncErrorHandler ErrorHandler;

        public ViewModelBase()
        {
            ErrorHandler = new AsyncExceptionHelper();
            ErrorHandler.ExceptionOcurred += ErrorHandlerOnExceptionOcurred;
        }

        protected virtual void ErrorHandlerOnExceptionOcurred(Exception exception)
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
            ErrorHandler.ExceptionOcurred -= ErrorHandlerOnExceptionOcurred;
        }
    }
}
