using Chat.Client.Framework;
using Chat.Client.Presenters.Delegates;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.ViewModels;
using Chat.Client.ViewModels.Events;
using System;
using System.Globalization;

namespace Chat.Client.Presenters
{
    public class CappuLoginPresenter : ViewModelBase, IDialog
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;

        private bool _connectedToServer;
        public bool ConnectedToServer {
            get { return _connectedToServer; }
            set { _connectedToServer = value; OnPropertyChanged(); StartServerConnectionCommand.RaiseCanExecuteChanged(); }
        }

        public CappuLoginViewModel CappuLoginViewModel { get; private set; }

        public RelayCommand StartServerConnectionCommand { get; }

        public event StartServerConnectionHandler StartConnection;

        public event Action<string> LoggedOut {
            add => CappuLoginViewModel.LoggedOut += value;
            remove => CappuLoginViewModel.LoggedOut -= value;
        }

        public CappuLoginPresenter(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            _signalHelperFacade = signalHelperFacade ?? throw new ArgumentNullException(nameof(signalHelperFacade));
            _viewProvider = viewProvider ?? throw new ArgumentNullException(nameof(viewProvider));

            StartServerConnectionCommand = new RelayCommand(StartServerConnection, CanStartServerConnection);

            Initialize();
        }

        private bool CanStartServerConnection()
        {
            return !ConnectedToServer;
        }

        public async void StartServerConnection()
        {
            if (StartConnection == null)
                throw new InvalidOperationException(CappuChat.Properties.Errors.NoOneRegisteredOnStartConnection);

            using (CappuLoginViewModel.ProgressProvider.StartProgress())
            {
                ConnectedToServer = await (StartConnection?.Invoke()).ConfigureAwait(true);
            }

            CappuLoginViewModel.RaiseCanExecuteChanged();
        }

        private void Initialize()
        {
            InitializeLoginViewModel();
            InitializeLoginViewModelEvents();
        }

        private void InitializeLoginViewModel()
        {
            CappuLoginViewModel = new CappuLoginViewModel(_signalHelperFacade);
        }

        private void InitializeLoginViewModelEvents()
        {
            CappuLoginViewModel.PreviewServerCall += CappuServerCallViewModelPreviewServerCall;
            CappuLoginViewModel.LoginFailed += CappuLoginViewModelOnLoginFailed;
            CappuLoginViewModel.LoggedOut += CappuLoginViewModelOnLoggedOut;
        }

        private bool CappuServerCallViewModelPreviewServerCall()
        {
            return ConnectedToServer;
        }

        private void CappuLoginViewModelOnLoginFailed(LoginFailedEventArgs eventArgs)
        {
            _viewProvider.ShowMessage(
                CappuChat.Properties.Strings.LoginFailed,
                string.Format(CultureInfo.CurrentCulture, CappuChat.Properties.Strings.LoginFailed_Reason, eventArgs.Reason)
            );
        }

        private void CappuLoginViewModelOnLoggedOut(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                return;
            _viewProvider.ShowMessage(
                CappuChat.Properties.Strings.LoggedOut,
                string.Format(CultureInfo.CurrentCulture, CappuChat.Properties.Strings.LoggedOut_Reason, reason)
            );
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CappuLoginViewModel.PreviewServerCall -= CappuServerCallViewModelPreviewServerCall;
                CappuLoginViewModel.LoginFailed -= CappuLoginViewModelOnLoginFailed;
            }

            base.Dispose(disposing);
        }
    }
}
