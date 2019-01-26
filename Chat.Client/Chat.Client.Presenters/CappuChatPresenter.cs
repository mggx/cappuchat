using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Client.ViewModels;
using Chat.Client.ViewModels.Controllers;
using Chat.Models;
using Chat.Shared.Models;

namespace Chat.Client.Presenters
{
    public class CappuChatPresenter : ViewModelBase, IDialog
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;

        private SimpleUser User { get; set; }

        private CappuChatViewModel _currentChatViewModel;
        public CappuChatViewModel CurrentChatViewModel
        {
            get { return _currentChatViewModel; }
            set { _currentChatViewModel = value; OnPropertyChanged(); _currentChatViewModel?.ConversationHelper.ResetNewMessages(); }
        }

        public ObservableCollection<CappuChatViewModel> Conversations { get; set; } = new ObservableCollection<CappuChatViewModel>();

        public CappuChatPresenter(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade), "Cannot create CappuChatPresenter. Given signalHelperFacade is null.");
            _signalHelperFacade = signalHelperFacade;

            if (viewProvider == null)
                throw new ArgumentNullException(nameof(viewProvider), "Cannot create CappuChatPresenter. Given viewProvider is null.");
            _viewProvider = viewProvider;

            Initialize();
        }

        private void Initialize()
        {
            InitializeChatSignalHelperFacadeEvents();
        }

        private void InitializeChatSignalHelperFacadeEvents()
        {
            _signalHelperFacade.ChatSignalHelper.PrivateMessageReceivedHandler += ChatSignalHelperOnPrivateMessageReceived;
        }

        private void ChatSignalHelperOnPrivateMessageReceived(MessageReceivedEventArgs eventArgs)
        {
            var username = eventArgs.ReceivedMessage.Sender.Username;
            TryAddCappuChatViewModel(username, false, eventArgs.ReceivedMessage);
            _viewProvider.FlashWindow();
        }

        private bool CheckForExistingConversation(string targetUsername)
        {
            var conversation = Conversations.FirstOrDefault(con => con.Conversation.TargetUsername == targetUsername);
            return conversation != null;
        }

        public void TryAddCappuChatViewModel(string username, bool setAsCurrentChatViewModel = false, params SimpleMessage[] messages)
        {
            if (CheckForExistingConversation(username))
            {
                if (setAsCurrentChatViewModel)
                    CurrentChatViewModel = Conversations.FirstOrDefault(cappuChatViewModel =>
                        cappuChatViewModel.Conversation.TargetUsername == username);
                return;
            }

            AddCappuChatViewModel(new SimpleConversation(username), setAsCurrentChatViewModel, messages);
        }

        private void AddCappuChatViewModel(SimpleConversation conversation, bool setAsCurrentChatViewModel = false, params SimpleMessage[] messages)
        {
            var chatViewModel = new CappuChatViewModel(_signalHelperFacade, conversation);
            chatViewModel.Load(messages);
            chatViewModel.ConversationHelper.AddNewMessage += ChatViewModelOnAddNewMessage;

            Conversations.Add(chatViewModel);

            if (setAsCurrentChatViewModel)
                CurrentChatViewModel = chatViewModel;
        }

        private bool ChatViewModelOnAddNewMessage(object sender, SimpleConversation conversation)
        {
            return conversation != CurrentChatViewModel.Conversation;
        }

        public void Load(SimpleUser user)
        {
            User = user;
            LoadConversations();
        }

        private void LoadConversations()
        {
            var conversations = new CappuMessageController(User).GetConversations();
            foreach (var conversation in conversations)
            {
                AddCappuChatViewModel(conversation);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _signalHelperFacade.ChatSignalHelper.PrivateMessageReceivedHandler -=
                    ChatSignalHelperOnPrivateMessageReceived;

                foreach (var viewModel in Conversations)
                {
                    viewModel.ConversationHelper.AddNewMessage -= ChatViewModelOnAddNewMessage;
                    viewModel.Dispose();
                }

                Conversations.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
