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
        public CappuGroupChatViewModel CappuGroupChatViewModel { get; private set; }

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
            InitializeCappuVoteResultViewModel();
            InitializeCappuGroupChatViewModel();
            InitializeVoteSignalHelperEvents();
        }

        private void InitializeCappuVoteResultViewModel()
        {
            CappuVoteResultViewModel = new CappuVoteResultViewModel(_signalHelperFacade, _viewProvider);
        }

        private void InitializeCappuVoteViewModel()
        {
            CappuVoteViewModel = new CappuVoteViewModel(_signalHelperFacade, _viewProvider);
        }

        private void InitializeCappuGroupChatViewModel()
        {
            CappuGroupChatViewModel = new CappuGroupChatViewModel(_signalHelperFacade, _viewProvider);
        }

        private void InitializeVoteSignalHelperEvents()
        {
            _signalHelperFacade.VoteSignalHelper.FinalCappuCalled += VoteSignalHelperOnFinalCappuCalled;
        }

        private void VoteSignalHelperOnFinalCappuCalled()
        {
            _viewProvider.ShowToastNotification(Texts.Texts.GoGoCall, NotificationType.Information);
            CappuVoteViewModel.Reset();
        }

        public async Task Load(SimpleUser user)
        {
            _user = user;
            await CappuVoteViewModel.Load(user);
            await CappuVoteResultViewModel.Load();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _signalHelperFacade.VoteSignalHelper.FinalCappuCalled -= VoteSignalHelperOnFinalCappuCalled;

                CappuVoteViewModel.Dispose();
                CappuVoteResultViewModel?.Dispose();
                CappuGroupChatViewModel.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
