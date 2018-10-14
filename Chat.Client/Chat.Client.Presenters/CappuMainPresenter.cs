using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using System;
using Chat.Client.Viewmodels.Events;
using Chat.Client.ViewModels.Dialogs;

namespace Chat.Client.Presenters
{
    public class CappuMainPresenter : ViewModelBase, IDialog
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;

        public LoginPresenter LoginPresenter { get; private set; }
        public CappuChatPresenter CappuChatPresenter { get; private set; }
        public CappuVotePresenter CappuVotePresenter { get; private set; }

        private ViewModelBase _currentContainer;
        public ViewModelBase CurrentContainer
        {
            get { return _currentContainer; }
            set { _currentContainer = value; OnPropertyChanged(); }
        }

        public CappuMainPresenter(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException("Cannot create CappuMainPresenter. Given controllerFacade is null.");

            if (viewProvider == null)
                throw new ArgumentNullException("Cannot create CappuMainPresenter. Given viewProvider is null.");

            _signalHelperFacade = signalHelperFacade;
            _viewProvider = viewProvider;

            Initialize();
        }

        private void Initialize()
        {
            InitializeLoginPresenter();
            InitializeLoginPresenterEvents();
        }

        private void InitializeLoginPresenter()
        {
            LoginPresenter = new LoginPresenter(_signalHelperFacade, _viewProvider);
            CurrentContainer = LoginPresenter;
        }

        private void InitializeLoginPresenterEvents()
        {
            LoginPresenter.CappuLoginViewModel.LoginSucceeded += LoginPresenterOnLoginSucceeded;
            LoginPresenter.LoggedOut += LoginPresenterOnLoggedOut;
            LoginPresenter.CappuLoginViewModel.RegisterOpen += CappuLoginPresenterOnRegisterOpen;
        }

        private void CappuLoginPresenterOnRegisterOpen()
        {
            var registerViewModel = new RegisterViewModel(_signalHelperFacade.RegisterSignalHelper);
            registerViewModel.RegisterCompleted += RegisterViewModelOnRegisterCompleted;
            registerViewModel.RegisterFailed += RegisterViewModelOnRegisterFailed;
            CurrentContainer = registerViewModel;
        }

        private void RegisterViewModelOnRegisterCompleted(IDialog registerViewModel)
        {
            RegisterViewModel viewModel = registerViewModel as RegisterViewModel;
            if (viewModel == null)
                throw new ArgumentNullException(nameof(registerViewModel), "Given registerViewModel is null or not a RegisterViewModel");

            LoginPresenter.CappuLoginViewModel.Username = viewModel.Username;

            viewModel.RegisterCompleted -= RegisterViewModelOnRegisterCompleted;
            viewModel.RegisterFailed -= RegisterViewModelOnRegisterFailed;

            CurrentContainer = LoginPresenter;
        }

        private void RegisterViewModelOnRegisterFailed(object sender, string e)
        {
            RegisterViewModel viewModel = sender as RegisterViewModel;
            if (viewModel == null)
                throw new ArgumentNullException(nameof(sender), "Given sender is null or not a RegisterViewModel");

            _viewProvider.ShowMessage(Texts.Texts.RegisterFailed, e);

            viewModel.RegisterFailed -= RegisterViewModelOnRegisterFailed;
            viewModel.RegisterCompleted -= RegisterViewModelOnRegisterCompleted;

            CurrentContainer = LoginPresenter;
        }

        private async void LoginPresenterOnLoginSucceeded(LoginSucceededEventArgs eventArgs)
        {
            InitializeCappuVotePresenter();
            InitializeCappuChatPresenter();

            await CappuVotePresenter.Load(eventArgs.User);
            CappuChatPresenter.Load(eventArgs.User);

            CurrentContainer = this;
        }

        private void InitializeCappuVotePresenter()
        {
            CappuVotePresenter = new CappuVotePresenter(_signalHelperFacade, _viewProvider);
        }

        private void InitializeCappuChatPresenter()
        {
            CappuChatPresenter = new CappuChatPresenter(_signalHelperFacade, _viewProvider);
        }

        private void LoginPresenterOnLoggedOut(string reason)
        {
            CurrentContainer = LoginPresenter;
        }

        public override void Dispose()
        {
            LoginPresenter.CappuLoginViewModel.LoginSucceeded -= LoginPresenterOnLoginSucceeded;
            LoginPresenter.LoggedOut -= LoginPresenterOnLoggedOut;

            base.Dispose();
        }
    }
}