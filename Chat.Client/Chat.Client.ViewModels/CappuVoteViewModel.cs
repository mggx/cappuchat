using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<SimpleUser> OnlineCappuUsers { get; set; } = new ObservableCollection<SimpleUser>();

        public RelayCommand<SimpleUser> OpenPrivateChatCommand { get; }

        public event OpenChatHandler OpenChat; 

        public CappuVoteViewModel(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade), "Cannot create CappuVoteViewModel. Given signalHelperFacade is null.");
            _signalHelperFacade = signalHelperFacade;

            if (viewProvider == null)
                throw new ArgumentNullException(nameof(viewProvider), "Cannot create CappuVoteViewModel. Given viewProvider is null.");
            _viewProvider = viewProvider;

            OpenPrivateChatCommand = new RelayCommand<SimpleUser>(OpenPrivateChat);

            Initialize();
        }

        private void OpenPrivateChat(SimpleUser targetUser)
        {
            OpenChat?.Invoke(targetUser);
        }

        private void Initialize()
        {
            InitializeSignalHelperFacadeEvents();
        }

        private void InitializeSignalHelperFacadeEvents()
        {
            _signalHelperFacade.LoginSignalHelper.OnlineUsersChanged += ChatSignalHelperFacadeOnOnlineUsersChanged;
        }

        public async Task Load(SimpleUser user)
        {
            _user = user;

            try
            {
                var onlineUsers = await _signalHelperFacade.ChatSignalHelper.GetOnlineUsers();
                SetOnlineUsers(onlineUsers);
            }
            catch (RequestFailedException e)
            {
                _viewProvider.ShowMessage(Texts.Texts.Error, e.Message);
            }
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
    }
}
