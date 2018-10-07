using Chat.Client.Framework.Async;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chat.Client.Framework
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool> _canExecute;
        private readonly IAsyncErrorHandler _errorHandler;
        private bool _isExecuting;

        public AsyncRelayCommand(Action action, Func<bool> canExecute, IAsyncErrorHandler errorHandler) : this(action, errorHandler)
        {
            _canExecute = canExecute;
        }

        public AsyncRelayCommand(Action action, IAsyncErrorHandler errorHandler) : this(errorHandler)
        {
            if (action == null)
                throw new ArgumentNullException("Cannot create RelayCommand. Given action is null.");
            _action = action;
        }

        public AsyncRelayCommand(IAsyncErrorHandler errorHandler)
        {
            if (errorHandler == null)
                throw new ArgumentNullException(nameof(errorHandler), "Cannot create AsyncRelayCommand. Given errorHandler is null.");
            _errorHandler = errorHandler;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;
            return _canExecute.Invoke() && !_isExecuting;
        }

        public async void Execute(object parameter)
        {
            await AsyncExecute(parameter);
        }

        private async Task AsyncExecute(object parameter)
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                await Task.Factory.StartNew(() => { _action.Invoke(); });
            }
            catch (Exception e)
            {
                _errorHandler.RaiseExceptionOcurred(e);
            }

            _isExecuting = false;
            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;
    }

    public class AsyncRelayCommand<T> : ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _action;
        private readonly IAsyncErrorHandler _errorHandler;
        private bool _isRunning;

        public AsyncRelayCommand(Action<T> action, Func<T, bool> canExecute, IAsyncErrorHandler errorHandler) : this(action, errorHandler)
        {
            _canExecute = canExecute;
        }

        public AsyncRelayCommand(Action<T> action, IAsyncErrorHandler errorHandler) : this(errorHandler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action), "Cannot create AsyncRelayCommand. Given action is null.");
            _action = action;
        }

        private AsyncRelayCommand(IAsyncErrorHandler errorHandler)
        {
            if (errorHandler == null)
                throw new ArgumentNullException(nameof(errorHandler), "Cannot create AsyncRelayCommand. Given errorHandler is null.");
            _errorHandler = errorHandler;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return !_isRunning;
            return _canExecute?.Invoke((T) parameter) == true && !_isRunning;
        }

        public async void Execute(object parameter)
        {
            await AsyncExecute(parameter);
        }

        private async Task AsyncExecute(object parameter)
        {
            _isRunning = true;
            RaiseCanExecuteChanged();

            try
            {
                await Task.Factory.StartNew(() => { _action.Invoke((T) parameter); });
            }
            catch (Exception e)
            {
                _errorHandler.RaiseExceptionOcurred(e);
            }

            _isRunning = false;
            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;
    }
}
