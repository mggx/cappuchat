using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.Viewmodels.Events;
using Chat.Client.ViewModels.Delegates;
using System;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Chat.Client.ViewModels.Events;
using System.Windows;
using Chat.Client.ViewModels.Providers;
using CappuChat;

namespace Chat.Client.ViewModels
{
    public class CappuLoginViewModel : ViewModelBase, IDialog
    {
        private readonly ISignalHelperFacade _signalHelperFacade;

        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); RaiseCanExecuteChanged(); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); RaiseCanExecuteChanged(); }
        }

        private bool _loggedIn;

        public bool LoggedIn
        {
            get { return _loggedIn; }
            set { _loggedIn = value; OnPropertyChanged(); }
        }

        public ProgressProvider ProgressProvider { get; } = new ProgressProvider();

        public RelayCommand LoginCommand { get; }
        public RelayCommand LogoutCommand { get; }
        public RelayCommand OpenRegisterCommand { get; }

        public event PreviewServerCallHandler PreviewServerCall;
        public event LoginSucceededHandler LoginSucceeded;
        public event LoginFailedHandler LoginFailed;
        public event Action<string> LoggedOut;
        public event Action RegisterOpen;

        public CappuLoginViewModel(ISignalHelperFacade signalHelperFacade)
        {
            _signalHelperFacade = signalHelperFacade ?? throw new ArgumentNullException(nameof(signalHelperFacade));

            LoginCommand = new RelayCommand(Login, CanLogin);
            LogoutCommand = new RelayCommand(Logout, CanLogout);
            OpenRegisterCommand = new RelayCommand(OpenRegister, CanOpenRegister);

            Initialize();
        }

        private void Initialize()
        {
            InitializeSignalHelperFacadeEvents();
        }

        private void InitializeSignalHelperFacadeEvents()
        {
            _signalHelperFacade.LoginSignalHelper.LoggedOutByServer += LoginSignalHelperOnLoggedOutByServer;
        }

        private void LoginSignalHelperOnLoggedOutByServer(object sender, string e)
        {
            LoggedOut?.Invoke(e);
            LoggedIn = false;
            RaiseCanExecuteChanged();
        }

        private bool CanLogin()
        {
            return PreviewServerCall?.Invoke() == true && !ProgressProvider.ProgressScope.InProgress && !LoggedIn;
        }

        private async void Login()
        {
            RaiseCanExecuteChanged();

            SimpleUser user;
            try
            {
                using (ProgressProvider.StartProgress())
                {
                    user = await _signalHelperFacade.LoginSignalHelper.Login(Username, Password).ConfigureAwait(false);
                }
            }
            catch (LoginFailedException e)
            {
                LoginFailed?.Invoke(new LoginFailedEventArgs(e.Message));
                return;
            }
            finally
            {
                RaiseCanExecuteChanged();
            }

            LoggedIn = true;
            LoginSucceeded?.Invoke(new LoginSucceededEventArgs(user));

            Application.Current.Dispatcher.Invoke(RaiseCanExecuteChanged);
        }

        private bool CanLogout()
        {
            return LoggedIn;
        }

        private void Logout()
        {
            _signalHelperFacade.LoginSignalHelper.Logout();
            LoggedIn = false;
            LoggedOut?.Invoke(string.Empty);
            RaiseCanExecuteChanged();
        }

        private bool CanOpenRegister()
        {
            return PreviewServerCall?.Invoke() == true && !ProgressProvider.ProgressScope.InProgress && !LoggedIn;
        }

        private void OpenRegister()
        {
            RegisterOpen?.Invoke();
        }

        public void Load(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public void RaiseCanExecuteChanged()
        {
            LoginCommand.RaiseCanExecuteChanged();
            LogoutCommand.RaiseCanExecuteChanged();
            OpenRegisterCommand.RaiseCanExecuteChanged();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _signalHelperFacade.LoginSignalHelper.LoggedOutByServer -= LoginSignalHelperOnLoggedOutByServer;
            }

            base.Dispose(disposing);
        }
    }
}
