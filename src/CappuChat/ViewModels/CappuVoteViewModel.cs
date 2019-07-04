using CappuChat;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Chat.Client.ViewModels
{
    public class CappuVoteViewModel : ViewModelBase
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;

        private SimpleUser _user;

        private SimpleCappuVote _simpleVote;
        public SimpleCappuVote SimpleVote {
            get { return _simpleVote; }
            set { _simpleVote = value; OnPropertyChanged(); }
        }

        public ObservableCollection<object> OnlineCappuUsers { get; } = new ObservableCollection<object>();

        public RelayCommand CreateCappuVoteCommand { get; }
        public RelayCommand<bool> GoCommand { get; }

        public CappuVoteViewModel(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade));
            _signalHelperFacade = signalHelperFacade;

            if (viewProvider == null)
                throw new ArgumentNullException(nameof(viewProvider));
            _viewProvider = viewProvider;

            CreateCappuVoteCommand = new RelayCommand(CreateCappuVote, CanCreateVote);
            GoCommand = new RelayCommand<bool>(Vote, CanVote);

            Initialize();
        }

        private bool CanCreateVote()
        {
            return _simpleVote == null;
        }

        private async void CreateCappuVote()
        {
            if (await CheckForActiveVote().ConfigureAwait(false))
            {
                _viewProvider.ShowMessage(CappuChat.Properties.Strings.TitleVoteAlreadyCreated, CappuChat.Properties.Strings.VoteAlreadyCreated);
                return;
            }

            try
            {
                await _signalHelperFacade.VoteSignalHelper.CreateVote().ConfigureAwait(false);
            }
            catch (CreateVoteFailedException e)
            {
                _viewProvider.ShowMessage(CappuChat.Properties.Strings.Error, e.Message);
            }
        }

        private bool CanVote(bool focus = false)
        {
            return !_simpleVote?.UserAnswerCache.ContainsKey(_user.Username) == true;
        }

        private async void Vote(bool focus = false)
        {
            try
            {
                await _signalHelperFacade.VoteSignalHelper.Vote(true).ConfigureAwait(false);

                if (focus)
                    _viewProvider.BringToFront();
            }
            catch (VoteFailedException e)
            {
                _viewProvider.ShowMessage("Error", e.Message);
            }

            RaiseCanExecuteChanged();
        }

        private void Initialize()
        {
            InitializeSignalHelperFacadeEvents();
        }

        private void InitializeSignalHelperFacadeEvents()
        {
            _signalHelperFacade.LoginSignalHelper.OnlineUsersChanged += ChatSignalHelperFacadeOnOnlineUsersChanged;
            _signalHelperFacade.VoteSignalHelper.VoteCreated += VoteSignalHelperOnCappuVoteCreated;
            _signalHelperFacade.VoteSignalHelper.VoteChanged += VoteSignalHelperOnCappuVoteChanged;
            _signalHelperFacade.VoteSignalHelper.FinalCappuCalled += VoteSignalHelperOnFinalCappuCalled;
        }

        public async Task Load(SimpleUser user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));

            try
            {
                var onlineUsers = await _signalHelperFacade.ChatSignalHelper.GetOnlineUsers().ConfigureAwait(false);
                UpdateOnlineCappuUsers(onlineUsers);
                await CheckForActiveVote().ConfigureAwait(false);
            }
            catch (RequestFailedException e)
            {
                _viewProvider.ShowMessage(CappuChat.Properties.Strings.Error, e.Message);
            }
        }

        private async Task<bool> CheckForActiveVote()
        {
            SimpleCappuVote activeVote = await _signalHelperFacade.VoteSignalHelper.GetActiveVote().ConfigureAwait(false);
            if (activeVote != null)
                UpdateActiveVote(activeVote);
            return activeVote != null;
        }

        private void UpdateActiveVote(SimpleCappuVote vote)
        {
            SimpleVote = vote;
            RaiseCanExecuteChanged();
        }

        private void ChatSignalHelperFacadeOnOnlineUsersChanged(SignalHelpers.Contracts.Events.OnlineUsersChangedEventArgs eventArgs)
        {
            UpdateOnlineCappuUsers(eventArgs.OnlineUsers);
        }

        private void UpdateOnlineCappuUsers(IEnumerable<SimpleUser> onlineUsers)
        {
            OnlineCappuUsers.Clear();
            foreach (var user in onlineUsers)
            {
                if (!user.Username.Equals(_user.Username, StringComparison.CurrentCultureIgnoreCase))
                    OnlineCappuUsers.Add(user);
            }
        }

        private void VoteSignalHelperOnCappuVoteCreated(SimpleCappuVote createdVote)
        {
            UpdateActiveVote(createdVote);
            _viewProvider.ShowToastNotification(CappuChat.Properties.Strings.CappuCalled, CappuChat.Properties.Strings.GoCall, NotificationType.Dark, GoCommand);
        }

        private void VoteSignalHelperOnCappuVoteChanged(SimpleCappuVote changedVote)
        {
            UpdateActiveVote(changedVote);
        }

        private void VoteSignalHelperOnFinalCappuCalled()
        {
            RaiseCanExecuteChanged();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _signalHelperFacade.LoginSignalHelper.OnlineUsersChanged -= ChatSignalHelperFacadeOnOnlineUsersChanged;
                _signalHelperFacade.VoteSignalHelper.VoteCreated -= VoteSignalHelperOnCappuVoteCreated;
                _signalHelperFacade.VoteSignalHelper.VoteChanged -= VoteSignalHelperOnCappuVoteChanged;
                _signalHelperFacade.VoteSignalHelper.FinalCappuCalled -= VoteSignalHelperOnFinalCappuCalled;
            }

            base.Dispose(disposing);
        }

        private void RaiseCanExecuteChanged()
        {
            GoCommand.RaiseCanExecuteChanged();
            CreateCappuVoteCommand.RaiseCanExecuteChanged();
        }

        public void Reset()
        {
            _simpleVote = null;
        }
    }
}
