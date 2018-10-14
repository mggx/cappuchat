using System;
using System.Threading.Tasks;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.ViewModels;
using Chat.Client.ViewModels.Dialogs;
using Chat.Shared.Models;

namespace Chat.Client.Presenters
{
    public class CappuVotePresenter : ViewModelBase
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;
        private SimpleUser _user;

        public CappuVoteViewModel CappuVoteViewModel { get; private set; }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        public CappuVotePresenter(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade), "Cannot create CappuVotePresenter. Given signalHelperFacade is null.");
            _signalHelperFacade = signalHelperFacade;

            if (viewProvider == null)
                throw new ArgumentNullException(nameof(viewProvider), "Cannot create CappuVotePresenter. Given viewProvider is null.");
            _viewProvider = viewProvider;

            Initialize();
        }

        private void Initialize()
        {
            InitializeCappuVoteViewModel();
            InitializeCappuVoteViewModelEvents();
        }

        private void InitializeCappuVoteViewModel()
        {
            CappuVoteViewModel = new CappuVoteViewModel(_signalHelperFacade, _viewProvider);
            CurrentViewModel = CappuVoteViewModel;
        }

        private void InitializeCappuVoteViewModelEvents()
        {
            CappuVoteViewModel.VoteCreate += CappuVoteViewModelOnVoteCreate;
        }

        private void CappuVoteViewModelOnVoteCreate()
        {
            var createVoteViewModel = new CreateVoteViewModel(_user.Username);
            createVoteViewModel.VoteCreated += CreateVoteViewModelOnVoteCreated;
            CurrentViewModel = createVoteViewModel;
        }

        private async void CreateVoteViewModelOnVoteCreated(object sender, SimpleVote e)
        {
            var createVoteViewModel = (CreateVoteViewModel) sender;
            createVoteViewModel.VoteCreated -= CreateVoteViewModelOnVoteCreated;
            await CappuVoteViewModel.CreateVote(e);
            CurrentViewModel = CappuVoteViewModel;
        }

        public async Task Load(SimpleUser user)
        {
            _user = user;
            await CappuVoteViewModel.Load(user);
        }
    }
}
