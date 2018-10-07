using System;
using System.Windows.Input;

namespace Chat.Client.Framework
{
    public class RelayCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("Cannot create RelayCommand. Given action is null.");
            _action = action;
        }

        public RelayCommand(Action action, Func<bool> canExecute)
        {
            if (action == null)
                throw new ArgumentNullException("Cannot create RelayCommand. Given action is null.");

            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;
            return _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _action?.Invoke();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _action;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException("Cannot create RelayCommand. Given action is null.");
            _action = action;
        }

        public RelayCommand(Action<T> action, Func<T, bool> canExecute)
        {
            if (action == null)
                throw new ArgumentNullException("Cannot create RelayCommand. Given action is null.");

            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;
            return _canExecute.Invoke((T) parameter);
        }

        public void Execute(object parameter)
        {
            _action?.Invoke((T) parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;
    }
}
