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

        private SimpleVote _simpleVote;
        public SimpleVote SimpleVote
        {
            get { return _simpleVote; }
            set { _simpleVote = value; OnPropertyChanged(); }
        }

        private string _answer;
        public string Answer
        {
            get => _answer;
            set { _answer = value; VoteCommand.RaiseCanExecuteChanged(); }
        }

        public ObservableCollection<SimpleUser> OnlineCappuUsers { get; set; } = new ObservableCollection<SimpleUser>();

        public RelayCommand<SimpleUser> OpenPrivateChatCommand { get; }
        public RelayCommand CreateVoteCommand { get; }
        public RelayCommand VoteCommand { get; }

        public event OpenChatHandler OpenChat;
        public event VoteCreateHandler VoteCreate;
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
            CreateVoteCommand = new RelayCommand(CreateVote, CanCreateVote);
            VoteCommand = new RelayCommand(Vote, CanVote);

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

        private void CreateVote()
        {
            VoteCreate?.Invoke();
        }

        private bool CanVote()
        {
            return !string.IsNullOrWhiteSpace(_answer);
        }

        private async void Vote()
        {
            try
            {
                await _signalHelperFacade.VoteSignalHelper.Vote(SimpleVote.Answers.ToList().IndexOf(_answer));
                Voted?.Invoke();
            }
            catch (VoteFailedException e)
            {
                _viewProvider.ShowMessage("Error", e.Message);
            }
        }

        public async Task CreateVote(SimpleVote vote)
        {
            try
            {
                await _signalHelperFacade.VoteSignalHelper.CreateVote(vote);
            }
            catch (CreateVoteFailedException e)
            {
                _viewProvider.ShowMessage(Texts.Texts.Error, e.Message);
            }
        }

        private void Initialize()
        {
            InitializeSignalHelperFacadeEvents();
        }

        private void InitializeSignalHelperFacadeEvents()
        {
            _signalHelperFacade.LoginSignalHelper.OnlineUsersChanged += ChatSignalHelperFacadeOnOnlineUsersChanged;
            _signalHelperFacade.VoteSignalHelper.VoteCreated += VoteSignalHelperOnVoteCreated;
            _signalHelperFacade.VoteSignalHelper.VoteChanged += VoteSignalHelperOnVoteChanged;
        }

        public async Task Load(SimpleUser user)
        {
            _user = user;

            try
            {
                var onlineUsers = await _signalHelperFacade.ChatSignalHelper.GetOnlineUsers();
                SetOnlineUsers(onlineUsers);

                SimpleVote activeVote = await _signalHelperFacade.VoteSignalHelper.GetActiveVote();
                SetActiveVote(activeVote);
            }
            catch (RequestFailedException e)
            {
                _viewProvider.ShowMessage(Texts.Texts.Error, e.Message);
            }
        }

        private void SetActiveVote(SimpleVote vote)
        {
            SimpleVote = vote;
            CreateVoteCommand.RaiseCanExecuteChanged();
        }

        private void ChatSignalHelperFacadeOnOnlineUsersChanged(SignalHelpers.Contracts.Events.OnlineUsersChangedEventArgs eventArgs)
        {
            SetOnlineUsers(eventArgs.OnlineUsers);
        }

        private void SetOnlineUsers(IEnumerable<SimpleUser> onlineUsers)
        {
            OnlineCappuUsers.Clear();
            foreach (var user in onlineUsers)
            {
                if (user.Username != _user.Username)
                    OnlineCappuUsers.Add(user);
            }
        }

        private void VoteSignalHelperOnVoteCreated(SimpleVote createdVote)
        {
            SetActiveVote(createdVote);
        }

        private void VoteSignalHelperOnVoteChanged(SimpleVote changedVote)
        {
            SetActiveVote(changedVote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _signalHelperFacade.LoginSignalHelper.OnlineUsersChanged -= ChatSignalHelperFacadeOnOnlineUsersChanged;
                _signalHelperFacade.VoteSignalHelper.VoteCreated -= VoteSignalHelperOnVoteCreated;
                _signalHelperFacade.VoteSignalHelper.VoteChanged -= VoteSignalHelperOnVoteChanged;
            }

            base.Dispose(disposing);
        }
    }
}
