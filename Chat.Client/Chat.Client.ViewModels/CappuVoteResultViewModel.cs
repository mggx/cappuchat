using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<UsersVotes> UserVotes { get; set; } = new ObservableCollection<UsersVotes>();

        public RelayCommand FinalCappuCallCommand { get; }

        public CappuVoteResultViewModel(ISignalHelperFacade signalHelperFacade)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade), "Cannot create CappuVoteResultViewModel. Given signalHelperFacade is null.");
            _signalHelperFacade = signalHelperFacade;

            FinalCappuCallCommand = new RelayCommand(FinalCappuCall, CanFinalCappuCall);

            Initialize();
        }

        private void Initialize()
        {
            UpdateVotes();
            InitializeSignalHelperFacadeEvents();
        }

        private async void UpdateVotes()
        {
            var onlineUsers = await _signalHelperFacade.ChatSignalHelper.GetOnlineUsers();
            UpdateVotes(onlineUsers);
            FinalCappuCallCommand.RaiseCanExecuteChanged();
        }

        private void UpdateVotes(IEnumerable<SimpleUser> users)
        {
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

        private void InitializeSignalHelperFacadeEvents()
        {
            _signalHelperFacade.VoteSignalHelper.VoteChanged += VoteSignalHelperOnVoteChanged;
            _signalHelperFacade.LoginSignalHelper.OnlineUsersChanged += LoginSignalHelperOnOnlineUsersChanged;
        }

        private void VoteSignalHelperOnVoteChanged(SimpleCappuVote changedVote)
        {
            _activeVote = changedVote;
            UpdateVotes();
            FinalCappuCallCommand.RaiseCanExecuteChanged();
        }

        private void LoginSignalHelperOnOnlineUsersChanged(OnlineUsersChangedEventArgs eventArgs)
        {
            UpdateVotes(eventArgs.OnlineUsers);
            FinalCappuCallCommand.RaiseCanExecuteChanged();
        }

        private bool CanFinalCappuCall()
        {
            return _activeVote.UserAnswerCache.Values.Count(vote => vote) == _onlineUsers?.Count();
        }

        private async void FinalCappuCall()
        {
            await _signalHelperFacade.VoteSignalHelper.FinalCappuCall();
        }

        public void Load(SimpleCappuVote cappuVote)
        {
            if (cappuVote == null)
                throw new InvalidOperationException("Cannot load CappuVoteResultViewModel. Given cappuVote is null.");
            _activeVote = cappuVote;
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
