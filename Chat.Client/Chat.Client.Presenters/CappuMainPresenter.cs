using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.Viewmodels.Events;
using Chat.Client.ViewModels.Dialogs;
using Chat.Shared.Models;
using System;
using System.Windows.Input;
using Chat.Configurations;
using Chat.Configurations.Models;
using Chat.Models;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace Chat.Client.Presenters
{
    public class CappuMainPresenter : ViewModelBase, IDialog
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;

        private readonly ConfigurationController<NotificationConfiguration> _notificationConfigurationController =
            new ConfigurationController<NotificationConfiguration>();

        private bool _showNotifications;
        public bool ShowNotifications
        {
            get => _showNotifications;
            set { _showNotifications = value; OnPropertyChanged(); }
        }

        private bool _saveMode;
        public bool SaveMode
        {
            get { return _saveMode; }
            set { _saveMode = value; OnPropertyChanged(); }
        }

        private bool _saveModeInverse;
        public bool SaveModeInverse
        {
            get { return _saveModeInverse; }
            set { _saveModeInverse = value; OnPropertyChanged(); }
        }

        private string _chatIcon;
        public string ChatIcon
        {
            get { return _chatIcon; }
            set { _chatIcon = value; OnPropertyChanged(); }
        }

        private string _windowTitle;
        public string WindowTitle
        {
            get { return _windowTitle; }
            set { _windowTitle = value; OnPropertyChanged(); }
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

        private Uri _currentIcon;
        public Uri CurrentIcon
        {
            get { return _currentIcon; }
            set { _currentIcon = value; OnPropertyChanged(); }
        }

        public ICommand ChangeShowNotificationsCommand { get; }
        public ICommand ChangeSaveModeCommand { get; }

        private Dictionary<string, Uri> _trayIcons;

        public CappuMainPresenter(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException("Cannot create CappuMainPresenter. Given controllerFacade is null.");

            if (viewProvider == null)
                throw new ArgumentNullException("Cannot create CappuMainPresenter. Given viewProvider is null.");

            _signalHelperFacade = signalHelperFacade;
            _viewProvider = viewProvider;

            _trayIcons = new Dictionary<string, Uri>();

            _trayIcons.Add("safeoff", new Uri("/chaticon.ico", UriKind.RelativeOrAbsolute));
            _trayIcons.Add("safeon", new Uri("/saveoutlook.ico", UriKind.RelativeOrAbsolute));

            SaveMode = true;
            SaveModeInverse = false;
            WindowTitle = "CappuChat";
            ChangeSaveMode();

            ChangeShowNotificationsCommand = new RelayCommand(ChangeShowNotifications);
            ChangeSaveModeCommand = new RelayCommand(ChangeSaveMode);
            Initialize();
        }

        private void ChangeSaveMode()
        {
            if (SaveMode)
            {
                SaveMode = false;
                CurrentIcon = _trayIcons["safeoff"];
                WindowTitle = "Mail";
            }
            else
            {
                SaveMode = true;
                CurrentIcon = _trayIcons["safeon"];
                ShowNotifications = false;
                WindowTitle = "CappuChat";
            }
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

            ShowNotifications = _notificationConfigurationController.ReadConfiguration(new NotificationConfiguration()).ShowPushNotifications;
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
            _viewProvider.ShowMessage(Texts.Texts.RegisterFailed, e);
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
    }
}