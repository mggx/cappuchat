using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Chat.Shared.Models;
using System;
using Chat.Client.ViewModels.Providers;

namespace Chat.Client.ViewModels.Dialogs
{
    public class CappuRegisterViewModel : ViewModelBase, IModalDialog<SimpleUser>
    {
        private readonly IRegisterSignalHelper _registerSignalHelper;

        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); RaiseCanExecuteChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); RaiseCanExecuteChanged(); }
        }

        public ModalResult ModalResult { get; set; } = ModalResult.Closed;

        public event EventHandler<string> RegisterFailed;
        public event Action<IDialog> RegisterCompleted;
        public event Action<IDialog> RegisterCanceled;
        

        public RelayCommand RegisterCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ProgressProvider ProgressProvider { get; }

        public CappuRegisterViewModel(IRegisterSignalHelper registerSignalHelper)
        {
            if (registerSignalHelper == null)
                throw new ArgumentNullException(nameof(registerSignalHelper), "Cannot create CappuRegisterViewModel. Given registerSignalHelper is null.");
            _registerSignalHelper = registerSignalHelper;

            RegisterCommand = new RelayCommand(Register, CanRegister);
            CancelCommand = new RelayCommand(Cancel);

            ProgressProvider = new ProgressProvider();
        }

        private bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(_username) &&
                   !string.IsNullOrWhiteSpace(_password);
        }

        private async void Register()
        {
            using (ProgressProvider.StartProgress())
            {
                try
                {
                    await _registerSignalHelper.Register(_username, _password);
                }
                catch (RequestFailedException e)
                {
                    RegisterFailed?.Invoke(this, e.Message);
                    return;
                }
            }

            RegisterCompleted?.Invoke(this);

            ModalResult = ModalResult.Ok;
        }

        private void Cancel()
        {
            RegisterCanceled?.Invoke(this);
        }

        private void RaiseCanExecuteChanged()
        {
            RegisterCommand.RaiseCanExecuteChanged();
        }

        public SimpleUser GetResult()
        {
            return new SimpleUser(_username);
        }
    }
}
