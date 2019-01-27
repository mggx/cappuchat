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

        public ObservableCollection<object> OnlineCappuUsers { get; set; } = new ObservableCollection<object>();

        public RelayCommand CreateCappuVoteCommand { get; }
        public RelayCommand<bool> GoCommand { get; }

        public CappuVoteViewModel(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade), "Cannot create CappuVoteViewModel. Given signalHelperFacade is null.");
            _signalHelperFacade = signalHelperFacade;

            if (viewProvider == null)
                throw new ArgumentNullException(nameof(viewProvider), "Cannot create CappuVoteViewModel. Given viewProvider is null.");
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
            if (await CheckForActiveVote())
            {
                _viewProvider.ShowMessage(Texts.Texts.TitleVoteAlreadyCreated, Texts.Texts.VoteAlreadyCreated);
                return;
            }

            try
            {
                await _signalHelperFacade.VoteSignalHelper.CreateVote();
            }
            catch (CreateVoteFailedException e)
            {
                _viewProvider.ShowMessage(Texts.Texts.Error, e.Message);
            }
        }

        private bool CanVote(bool focus = false)
        {
            return !_simpleVote?.UserAnswerCache.ContainsKey(_user.Username.ToLower()) == true;
        }

        private async void Vote(bool focus = false)
        {
            try
            {
                await _signalHelperFacade.VoteSignalHelper.Vote(true);

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
            _user = user;

            try
            {
                var onlineUsers = await _signalHelperFacade.ChatSignalHelper.GetOnlineUsers();
                UpdateOnlineCappuUsers(onlineUsers);
                await CheckForActiveVote();
            }
            catch (RequestFailedException e)
            {
                _viewProvider.ShowMessage(Texts.Texts.Error, e.Message);
            }
        }

        private async Task<bool> CheckForActiveVote()
        {
            SimpleCappuVote activeVote = await _signalHelperFacade.VoteSignalHelper.GetActiveVote();
            if (activeVote != null)
                UpdateActiveVote(activeVote, true);
            return activeVote != null;
        }

        private void UpdateActiveVote(SimpleCappuVote vote, bool raiseVotedEvent = false)
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
            _viewProvider.ShowToastNotification(Texts.Texts.CappuCalled, NotificationType.CappuCall, GoCommand);
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
