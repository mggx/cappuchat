using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Chat.Client.ViewModels.Models;
using Chat.Shared.Models;

namespace Chat.Client.ViewModels
{
    public class CappuVoteResultViewModel : ViewModelBase
    {
        private readonly ISignalHelperFacade _signalHelperFacade;

        private SimpleCappuVote _activeVote;
        private IEnumerable<SimpleUser> _onlineUsers;
        private IViewProvider _viewProvider;

        public SimpleUser User => _signalHelperFacade?.LoginSignalHelper.User;

        public ObservableCollection<UsersVotes> UserVotes { get; set; } = new ObservableCollection<UsersVotes>();
        public ObservableCollection<SimpleMessage> Broadcasts { get; set; } = new ObservableCollection<SimpleMessage>();

        public RelayCommand FinalCappuCallCommand { get; }
        public RelayCommand<string> SendBroadcastCommand { get; }

        public CappuVoteResultViewModel(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade), "Cannot create CappuVoteResultViewModel. Given signalHelperFacade is null.");
            _signalHelperFacade = signalHelperFacade;

            if (viewProvider == null)
                throw new ArgumentNullException(nameof(viewProvider), "Cannot create CappuVoteResultViewModel. Given viewProvider is null.");
            _viewProvider = viewProvider;

            FinalCappuCallCommand = new RelayCommand(FinalCappuCall, CanFinalCappuCall);
            SendBroadcastCommand = new RelayCommand<string>(SendBroadcast, CanSendBroadcast);

            Initialize();
        }

        private void Initialize()
        {
            InitializeSignalHelperFacadeEvents();
        }

        private void InitializeSignalHelperFacadeEvents()
        {
            _signalHelperFacade.VoteSignalHelper.VoteChanged += VoteSignalHelperOnVoteChanged;
            _signalHelperFacade.LoginSignalHelper.OnlineUsersChanged += LoginSignalHelperOnOnlineUsersChanged;
            _signalHelperFacade.ChatSignalHelper.MessageReceivedHandler += ChatSignalHelperOnMessageReceived;
        }

        private async void VoteSignalHelperOnVoteChanged(SimpleCappuVote changedVote)
        {
            await LoadVotes();
            FinalCappuCallCommand.RaiseCanExecuteChanged();
        }

        private async void LoginSignalHelperOnOnlineUsersChanged(OnlineUsersChangedEventArgs eventArgs)
        {
            await LoadVotes(eventArgs.OnlineUsers);
            FinalCappuCallCommand.RaiseCanExecuteChanged();
        }

        private void ChatSignalHelperOnMessageReceived(MessageReceivedEventArgs eventArgs)
        {
            Broadcasts.Add(eventArgs.ReceivedMessage);
            _viewProvider.FlashWindow();
        }

        private bool CanFinalCappuCall()
        {
            return _activeVote?.UserAnswerCache.Values.Count(vote => vote) == _onlineUsers?.Count();
        }

        private async void FinalCappuCall()
        {
            await _signalHelperFacade.VoteSignalHelper.FinalCappuCall();
        }

        private bool CanSendBroadcast(string message)
        {
            return !string.IsNullOrWhiteSpace(message);
        }

        private void SendBroadcast(string message)
        {
            var user = _signalHelperFacade.LoginSignalHelper.User;
            _signalHelperFacade.ChatSignalHelper.SendMessage(new SimpleMessage(user, user, message){MessageSentDateTime = DateTime.Now});
        }

        public async Task Load(SimpleCappuVote cappuVote)
        {
            if (cappuVote == null)
                throw new InvalidOperationException("Cannot load CappuVoteResultViewModel. Given cappuVote is null.");
            _activeVote = cappuVote;

            await LoadVotes();
            await LoadVoteScopedBroadcastMessages();
        }

        private async Task LoadVotes()
        {
            var onlineUsers = await _signalHelperFacade.ChatSignalHelper.GetOnlineUsers();
            await LoadVotes(onlineUsers);
            FinalCappuCallCommand.RaiseCanExecuteChanged();
        }

        private async Task LoadVotes(IEnumerable<SimpleUser> users)
        {
            _activeVote = await _signalHelperFacade.VoteSignalHelper.GetActiveVote();

            var onlineUsers = users as SimpleUser[] ?? users.ToArray();
            _onlineUsers = onlineUsers;

            UserVotes.Clear();

            foreach (var user in onlineUsers)
            {
                bool voted = _activeVote?.UserAnswerCache.ContainsKey(user.Username) == true;
                UserVotes.Add(new UsersVotes { Username = user.Username, Voted = voted });
            }

            OnPropertyChanged(nameof(UserVotes));
        }

        private async Task LoadVoteScopedBroadcastMessages()
        {
            Broadcasts.Clear();

            foreach (SimpleMessage message in await _signalHelperFacade.VoteSignalHelper.GetVoteScopeMessages())
            {
                Broadcasts.Add(message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _signalHelperFacade.VoteSignalHelper.VoteChanged -= VoteSignalHelperOnVoteChanged;
                _signalHelperFacade.LoginSignalHelper.OnlineUsersChanged -= LoginSignalHelperOnOnlineUsersChanged;
            }

            base.Dispose(disposing);
        }
    }
}
