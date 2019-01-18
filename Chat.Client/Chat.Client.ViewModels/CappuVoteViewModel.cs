using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Windows;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Chat.Client.ViewModels.Delegates;
using Chat.Shared.Models;

namespace Chat.Client.ViewModels
{
    public class CappuVoteViewModel : ViewModelBase
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;

        private SimpleUser _user;

        private SimpleCappuVote _simpleVote;
        public SimpleCappuVote SimpleVote
        {
            get { return _simpleVote; }
            set { _simpleVote = value; OnPropertyChanged(); }
        }

        public ObservableCollection<SimpleUser> OnlineCappuUsers { get; set; } = new ObservableCollection<SimpleUser>();

        public RelayCommand<SimpleUser> OpenPrivateChatCommand { get; }
        public RelayCommand CreateCappuVoteCommand { get; }
        public RelayCommand GoCommand { get; }

        public event OpenChatHandler OpenChat;
        public event VotedHandler Voted;

        public CappuVoteViewModel(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade), "Cannot create CappuVoteViewModel. Given signalHelperFacade is null.");
            _signalHelperFacade = signalHelperFacade;

            if (viewProvider == null)
                throw new ArgumentNullException(nameof(viewProvider), "Cannot create CappuVoteViewModel. Given viewProvider is null.");
            _viewProvider = viewProvider;

            OpenPrivateChatCommand = new RelayCommand<SimpleUser>(OpenPrivateChat);
            CreateCappuVoteCommand = new RelayCommand(CreateCappuVote, CanCreateVote);
            GoCommand = new RelayCommand(Vote, CanVote);

            Initialize();
        }

        private void OpenPrivateChat(SimpleUser targetUser)
        {
            OpenChat?.Invoke(targetUser);
        }

        private bool CanCreateVote()
        {
            return _simpleVote == null;
        }

        private void CreateCappuVote()
        {
            _signalHelperFacade.VoteSignalHelper.CreateVote();
        }

        private bool CanVote()
        {
            return true;
        }

        private async void Vote()
        {
            try
            {
                await _signalHelperFacade.VoteSignalHelper.Vote(true);
                Voted?.Invoke(this, _simpleVote);
            }
            catch (VoteFailedException e)
            {
                _viewProvider.ShowMessage("Error", e.Message);
            }
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
        }

        public async Task Load(SimpleUser user)
        {
            _user = user;

            try
            {
                var onlineUsers = await _signalHelperFacade.ChatSignalHelper.GetOnlineUsers();
                UpdateOnlineCappuUsers(onlineUsers);

                SimpleCappuVote activeVote = await _signalHelperFacade.VoteSignalHelper.GetActiveVote();
                if (activeVote != null)
                    UpdateActiveVote(activeVote, true);
            }
            catch (RequestFailedException e)
            {
                _viewProvider.ShowMessage(Texts.Texts.Error, e.Message);
            }
        }

        private void UpdateActiveVote(SimpleCappuVote vote, bool raiseVotedEvent = false)
        {
            SimpleVote = vote;
            if (vote?.UserAnswerCache.ContainsKey(_user.Username) == true && raiseVotedEvent)
            {
                Voted?.Invoke(this, _simpleVote);
            }
            CreateCappuVoteCommand.RaiseCanExecuteChanged();
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
                if (user.Username != _user.Username)
                    OnlineCappuUsers.Add(user);
            }
        }

        private void VoteSignalHelperOnCappuVoteCreated(SimpleCappuVote createdVote)
        {
            UpdateActiveVote(createdVote);
            _viewProvider.ShowToastNotification(Texts.Texts.CappuCalled, NotificationType.Information);
        }

        private void VoteSignalHelperOnCappuVoteChanged(SimpleCappuVote changedVote)
        {
            UpdateActiveVote(changedVote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _signalHelperFacade.LoginSignalHelper.OnlineUsersChanged -= ChatSignalHelperFacadeOnOnlineUsersChanged;
                _signalHelperFacade.VoteSignalHelper.VoteCreated -= VoteSignalHelperOnCappuVoteCreated;
                _signalHelperFacade.VoteSignalHelper.VoteChanged -= VoteSignalHelperOnCappuVoteChanged;
            }

            base.Dispose(disposing);
        }
    }
}
