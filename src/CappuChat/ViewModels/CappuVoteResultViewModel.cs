using CappuChat;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Client.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Client.ViewModels
{
    public class CappuVoteResultViewModel : ViewModelBase
    {
        private readonly ISignalHelperFacade _signalHelperFacade;

        private SimpleCappuVote _activeVote;
        private IEnumerable<SimpleUser> _onlineUsers;

        public SimpleUser User => _signalHelperFacade?.LoginSignalHelper.User;

        public ObservableCollection<UsersVotes> UserVotes { get; } = new ObservableCollection<UsersVotes>();

        public RelayCommand FinalCappuCallCommand { get; }

        public CappuVoteResultViewModel(ISignalHelperFacade signalHelperFacade)
        {
            _signalHelperFacade = signalHelperFacade ?? throw new ArgumentNullException(nameof(signalHelperFacade));

            FinalCappuCallCommand = new RelayCommand(FinalCappuCall, CanFinalCappuCall);

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
        }

        private async void VoteSignalHelperOnVoteChanged(SimpleCappuVote changedVote)
        {
            await LoadVotes().ConfigureAwait(false);
            FinalCappuCallCommand.RaiseCanExecuteChanged();
        }

        private async void LoginSignalHelperOnOnlineUsersChanged(OnlineUsersChangedEventArgs eventArgs)
        {
            await LoadVotes(eventArgs.OnlineUsers).ConfigureAwait(false);
            FinalCappuCallCommand.RaiseCanExecuteChanged();
        }

        private bool CanFinalCappuCall()
        {
            return _activeVote?.UserAnswerCache.Values.Count(vote => vote) == _onlineUsers?.Count();
        }

        private async void FinalCappuCall()
        {
            await _signalHelperFacade.VoteSignalHelper.FinalCappuCall().ConfigureAwait(false);
        }

        public async Task Load()
        {
            await LoadVotes().ConfigureAwait(false);
        }

        private async Task LoadVotes()
        {
            var onlineUsers = await _signalHelperFacade.ChatSignalHelper.GetOnlineUsers().ConfigureAwait(false);
            await LoadVotes(onlineUsers).ConfigureAwait(false);
            FinalCappuCallCommand.RaiseCanExecuteChanged();
        }

        private async Task LoadVotes(IEnumerable<SimpleUser> users)
        {
            _activeVote = await _signalHelperFacade.VoteSignalHelper.GetActiveVote().ConfigureAwait(false);

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
