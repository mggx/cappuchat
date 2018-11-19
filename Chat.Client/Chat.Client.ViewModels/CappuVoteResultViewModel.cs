using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Shared.Models;

namespace Chat.Client.ViewModels
{
    public class CappuVoteResultViewModel : ViewModelBase
    {
        private readonly ISignalHelperFacade _signalHelperFacade;

        private SimpleVote _activeVote;

        public ObservableCollection<string> OnlineUserList { get; set; } = new ObservableCollection<string>();

        public IEnumerable<string> UsersWhoAnswered => _activeVote.VoteAnswerCache.Keys;

        private double _voteProgress;
        public double VoteProgress
        {
            get => _voteProgress;
            set { _voteProgress = value; OnPropertyChanged(); }
        }

        public CappuVoteResultViewModel(ISignalHelperFacade signalHelperFacade)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade), "Cannot create CappuVoteResultViewModel. Given signalHelperFacade is null.");
            _signalHelperFacade = signalHelperFacade;

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

        private void VoteSignalHelperOnVoteChanged(SimpleVote changedVote)
        {
            _activeVote = changedVote;
            OnPropertyChanged(nameof(UsersWhoAnswered));

            double userCount = OnlineUserList.Count;
            if (userCount == 0)
                userCount = 1;
            double percentagePerUser = 100 / userCount;
            VoteProgress = UsersWhoAnswered.Count() * percentagePerUser;
        }

        private void LoginSignalHelperOnOnlineUsersChanged(OnlineUsersChangedEventArgs eventArgs)
        {
            OnlineUserList.Clear();

            foreach (SimpleUser user in eventArgs.OnlineUsers)
            {
                OnlineUserList.Add(user.Username);
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
