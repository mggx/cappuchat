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
        public CappuVoteResultViewModel CappuVoteResultViewModel { get; private set; }

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
            InitializeVoteSignalHelperEvents();
        }

        private void InitializeCappuVoteViewModel()
        {
            CappuVoteViewModel = new CappuVoteViewModel(_signalHelperFacade, _viewProvider);
            CurrentViewModel = CappuVoteViewModel;
        }

        private void InitializeCappuVoteViewModelEvents()
        {
            CappuVoteViewModel.Voted += CappuVoteViewModelOnVoted;
        }

        private void InitializeVoteSignalHelperEvents()
        {
            _signalHelperFacade.VoteSignalHelper.FinalCappuCalled += VoteSignalHelperOnFinalCappuCalled;
        }

        private void VoteSignalHelperOnFinalCappuCalled()
        {
            _viewProvider.ShowToastNotification(Texts.Texts.GoGoCall, NotificationType.Information);
            CappuVoteViewModel.Reset();
            CurrentViewModel = CappuVoteViewModel;
        }

        private async void CappuVoteViewModelOnVoted(object sender, SimpleCappuVote simpleCappuVote)
        {
            CappuVoteResultViewModel = new CappuVoteResultViewModel(_signalHelperFacade, _viewProvider);
            await CappuVoteResultViewModel.Load(simpleCappuVote);
            CurrentViewModel = CappuVoteResultViewModel;
        }

        public async Task Load(SimpleUser user)
        {
            _user = user;
            await CappuVoteViewModel.Load(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CappuVoteViewModel.Voted -= CappuVoteViewModelOnVoted;
                _signalHelperFacade.VoteSignalHelper.FinalCappuCalled -= VoteSignalHelperOnFinalCappuCalled;

                CappuVoteViewModel.Dispose();
                CappuVoteResultViewModel?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
