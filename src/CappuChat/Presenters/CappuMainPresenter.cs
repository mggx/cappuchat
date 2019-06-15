using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.Viewmodels.Events;
using Chat.Client.ViewModels.Dialogs;
using System;
using System.Windows.Input;
using Chat.Models;
using CappuChat.Configuration;
using CappuChat;

namespace Chat.Client.Presenters
{
    public class CappuMainPresenter : ViewModelBase, IDialog
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;

        private readonly ConfigurationController<NotificationConfiguration> _notificationConfigurationController =
            new ConfigurationController<NotificationConfiguration>();

        private readonly ConfigurationController<ClientConfiguration> _clientConfigurationController =
            new ConfigurationController<ClientConfiguration>();

        private bool _showNotifications;
        public bool ShowNotifications
        {
            get => _showNotifications;
            set { _showNotifications = value; OnPropertyChanged(); }
        }

        private bool _safeMode;
        public bool SafeMode
        {
            get { return _safeMode; }
            set { _safeMode = value; OnPropertyChanged(); }
        }

        public CappuLoginPresenter CappuLoginPresenter { get; private set; }
        public CappuChatPresenter CappuChatPresenter { get; private set; }
        public CappuVotePresenter CappuVotePresenter { get; private set; }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();

                if (value == 1)
                {
                    CappuChatPresenter?.CurrentChatViewModel?.ConversationHelper.ResetNewMessages();
                }
            }
        }

        private ViewModelBase _currentPresenter;
        public ViewModelBase CurrentPresenter
        {
            get { return _currentPresenter; }
            set { _currentPresenter = value; OnPropertyChanged(); }
        }

        public ICommand ChangeShowNotificationsCommand { get; }
        public ICommand ChangeSaveModeCommand { get; }

        public CappuMainPresenter(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException("Cannot create CappuMainPresenter. Given controllerFacade is null.");

            if (viewProvider == null)
                throw new ArgumentNullException("Cannot create CappuMainPresenter. Given viewProvider is null.");

            _signalHelperFacade = signalHelperFacade;
            _viewProvider = viewProvider;

            ChangeShowNotificationsCommand = new RelayCommand(ChangeShowNotifications);
            ChangeSaveModeCommand = new RelayCommand(ChangeSaveMode);
            Initialize();
        }

        private void ChangeSaveMode()
        {
            SafeMode = !_safeMode;
            _clientConfigurationController.WriteConfiguration(new ClientConfiguration { SafeMode = _safeMode } );
        }

        private void ChangeShowNotifications()
        {
            ShowNotifications = !ShowNotifications;
            _notificationConfigurationController.WriteConfiguration(new NotificationConfiguration { ShowPushNotifications = ShowNotifications });
        }

        private void Initialize()
        {
            InitializeLoginPresenter();
            InitializeLoginPresenterEvents();
            InitializeClientConfigurations();

            ShowNotifications = _notificationConfigurationController.ReadConfiguration(new NotificationConfiguration()).ShowPushNotifications;
        }

        private void InitializeClientConfigurations()
        {
            if (_clientConfigurationController.TryReadConfiguration(out var retrievedInstance))
                SafeMode = retrievedInstance.SafeMode;
        }

        private void InitializeLoginPresenter()
        {
            CappuLoginPresenter = new CappuLoginPresenter(_signalHelperFacade, _viewProvider);
            CurrentPresenter = CappuLoginPresenter;
        }

        private void InitializeLoginPresenterEvents()
        {
            CappuLoginPresenter.CappuLoginViewModel.LoginSucceeded += LoginPresenterOnLoginSucceeded;
            CappuLoginPresenter.LoggedOut += LoginPresenterOnLoggedOut;
            CappuLoginPresenter.CappuLoginViewModel.RegisterOpen += CappuLoginPresenterOnRegisterOpen;
        }

        private void CappuLoginPresenterOnRegisterOpen()
        {
            var registerViewModel = new CappuRegisterViewModel(_signalHelperFacade.RegisterSignalHelper);
            registerViewModel.RegisterCompleted += RegisterViewModelOnRegisterCompleted;
            registerViewModel.RegisterFailed += RegisterViewModelOnRegisterFailed;
            registerViewModel.RegisterCanceled += RegisterViewModelOnRegisterCanceled;
            CurrentPresenter = registerViewModel;
        }

        private void RegisterViewModelOnRegisterCompleted(IDialog registerViewModel)
        {
            CappuLoginPresenter.CappuLoginViewModel.Username = ((CappuRegisterViewModel)registerViewModel).Username;
            HandleFinishedRegister(registerViewModel);
        }

        private void RegisterViewModelOnRegisterFailed(object sender, string e)
        {
            _viewProvider.ShowMessage(CappuChat.Properties.Strings.RegistrationFailed, e);
            HandleFinishedRegister(sender as IDialog);
        }

        private void RegisterViewModelOnRegisterCanceled(IDialog registerViewModel)
        {
            HandleFinishedRegister(registerViewModel);
        }

        private void HandleFinishedRegister(IDialog registerViewModel)
        {
            CappuRegisterViewModel viewModel = registerViewModel as CappuRegisterViewModel;
            if (viewModel == null)
                throw new ArgumentNullException(nameof(registerViewModel), "Given registerViewModel is null or not a CappuRegisterViewModel");

            viewModel.RegisterCompleted -= RegisterViewModelOnRegisterCompleted;
            viewModel.RegisterFailed -= RegisterViewModelOnRegisterFailed;

            CurrentPresenter = CappuLoginPresenter;
        }

        private async void LoginPresenterOnLoginSucceeded(LoginSucceededEventArgs eventArgs)
        {
            InitializeCappuVotePresenter();
            InitializeCappuVotePresenterEvents();
            InitializeCappuChatPresenter();
            InitializeCappuChatPresenterEvents();

            await CappuVotePresenter.Load(eventArgs.User);
            CappuChatPresenter.Load(eventArgs.User);

            CurrentPresenter = this;
        }

        private void InitializeCappuVotePresenter()
        {
            CappuVotePresenter = new CappuVotePresenter(_signalHelperFacade, _viewProvider);
        }

        private void InitializeCappuVotePresenterEvents()
        {
            CappuVotePresenter.CappuGroupChatViewModel.OpenChat += CappuVoteViewModelOnOpenChat;
        }

        private void CappuVoteViewModelOnOpenChat(SimpleUser target)
        {
            CappuChatPresenter.TryAddCappuChatViewModel(target.Username, true);
            SelectedTabIndex = 1;
        }

        private void InitializeCappuChatPresenter()
        {
            CappuChatPresenter = new CappuChatPresenter(_signalHelperFacade, _viewProvider);
        }

        private void InitializeCappuChatPresenterEvents()
        {
            CappuChatPresenter.AddNewMessage += CappuChatPresenterOnNewMessage;
        }

        private bool CappuChatPresenterOnNewMessage(object sender, Models.SimpleConversation conversation)
        {
            return SelectedTabIndex == 0;
        }

        private void LoginPresenterOnLoggedOut(string reason)
        {
            CappuVotePresenter.CappuGroupChatViewModel.OpenChat -= CappuVoteViewModelOnOpenChat;
            CappuVotePresenter.Dispose();
            CappuChatPresenter.Dispose();
            CurrentPresenter = CappuLoginPresenter;
        }

        public void Load()
        {
            CappuLoginPresenter.StartServerConnection();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CappuLoginPresenter.CappuLoginViewModel.LoginSucceeded -= LoginPresenterOnLoginSucceeded;
                CappuLoginPresenter.LoggedOut -= LoginPresenterOnLoggedOut;
                CappuLoginPresenter.CappuLoginViewModel.RegisterOpen -= CappuLoginPresenterOnRegisterOpen;

                CappuVotePresenter?.Dispose();
                CappuChatPresenter?.Dispose();
                CappuLoginPresenter?.Dispose();
            }

            base.Dispose(disposing);
        }

        public void ShowChangelog(string changelog)
        {
            var changelogViewModel = new ChangelogViewModel(changelog);
            changelogViewModel.ChangelogClose += ChangelogViewModelOnChangelogClose;
            CurrentPresenter = changelogViewModel;
        }

        private void ChangelogViewModelOnChangelogClose(object sender, EventArgs e)
        {
            if (sender is ChangelogViewModel changelogViewModel)
                changelogViewModel.ChangelogClose -= ChangelogViewModelOnChangelogClose;
            CurrentPresenter = CappuLoginPresenter;
        }
    }
}