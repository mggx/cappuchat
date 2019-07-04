using CappuChat;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.ViewModels;
using System;
using System.Threading.Tasks;

namespace Chat.Client.Presenters
{
    public class CappuVotePresenter : ViewModelBase
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;

        public CappuVoteViewModel CappuVoteViewModel { get; private set; }
        public CappuVoteResultViewModel CappuVoteResultViewModel { get; private set; }
        public CappuGroupChatViewModel CappuGroupChatViewModel { get; private set; }

        public CappuVotePresenter(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            _signalHelperFacade = signalHelperFacade ?? throw new ArgumentNullException(nameof(signalHelperFacade));
            _viewProvider = viewProvider ?? throw new ArgumentNullException(nameof(viewProvider));

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
            CappuVoteResultViewModel = new CappuVoteResultViewModel(_signalHelperFacade);
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
            _viewProvider.ShowToastNotification(CappuChat.Properties.Strings.GoGoCall, NotificationType.Information);
            CappuVoteViewModel.Reset();
        }

        public async Task Load(SimpleUser user)
        {
            await CappuVoteViewModel.Load(user).ConfigureAwait(false);
            await CappuVoteResultViewModel.Load().ConfigureAwait(false);
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
