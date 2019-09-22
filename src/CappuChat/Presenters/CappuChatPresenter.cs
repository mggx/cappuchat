using CappuChat;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Client.ViewModels;
using Chat.Client.ViewModels.Controllers;
using Chat.Client.ViewModels.Delegates;
using Chat.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Client.Presenters
{
    public class CappuChatPresenter : ViewModelBase, IDialog
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;

        private SimpleUser User { get; set; }

        private CappuChatViewModel _currentChatViewModel;
        public CappuChatViewModel CurrentChatViewModel {
            get { return _currentChatViewModel; }
            set { _currentChatViewModel = value; OnPropertyChanged(); _currentChatViewModel?.ConversationHelper.ResetNewMessages(); }
        }

        private int _newMessages;
        public int NewMessages {
            get { return _newMessages; }
            set { _newMessages = value; OnPropertyChanged(); }
        }

        public event AddNewMessageHandler AddNewMessage;

        public ObservableCollection<CappuChatViewModel> Conversations { get; } = new ObservableCollection<CappuChatViewModel>();

        public CappuChatPresenter(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            _signalHelperFacade = signalHelperFacade ?? throw new ArgumentNullException(nameof(signalHelperFacade));
            _viewProvider = viewProvider ?? throw new ArgumentNullException(nameof(viewProvider));

            Initialize();
        }

        private void Initialize()
        {
            InitializeChatSignalHelperFacadeEvents();
        }

        private void InitializeChatSignalHelperFacadeEvents()
        {
            _signalHelperFacade.ChatSignalHelper.PrivateMessageReceivedHandler += SignalHelperOnMessageReceived;
        }

        private void SignalHelperOnMessageReceived(MessageReceivedEventArgs eventArgs)
        {
            HandleMessageReceived(eventArgs.ReceivedMessage);
        }

        private void HandleMessageReceived(SimpleMessage receivedMessage)
        {
            var username = receivedMessage.Sender.Username;
            TryAddCappuChatViewModel(username, false, receivedMessage);
            _viewProvider.FlashWindow();
        }

        private bool TryGetConversationByUsername(string targetUsername, out CappuChatViewModel chatViewModel)
        {
            chatViewModel = Conversations.FirstOrDefault(con => con.Conversation.TargetUsername.Equals(targetUsername, StringComparison.CurrentCultureIgnoreCase));
            return chatViewModel != null;
        }

        public void TryAddCappuChatViewModel(string username, bool setAsCurrentChatViewModel = false, params SimpleMessage[] messages)
        {
            if (TryGetConversationByUsername(username, out var chatViewModel))
            {
                if (setAsCurrentChatViewModel)
                    CurrentChatViewModel = chatViewModel;
                return;
            }

            AddCappuChatViewModel(new SimpleConversation(username), setAsCurrentChatViewModel, messages);
        }

        private void AddCappuChatViewModel(SimpleConversation conversation, bool setAsCurrentChatViewModel = false, params SimpleMessage[] messages)
        {
            var chatViewModel = new CappuChatViewModel(_signalHelperFacade, conversation, _viewProvider);
            chatViewModel.ConversationHelper.AddNewMessage += ChatViewModelOnAddNewMessage;
            chatViewModel.ConversationHelper.NewMessagesChanged += ChatViewModelOnNewMessagesChanged;

            Conversations.Add(chatViewModel);

            chatViewModel.Load(messages);

            if (setAsCurrentChatViewModel)
                CurrentChatViewModel = chatViewModel;
        }

        private bool ChatViewModelOnAddNewMessage(object sender, SimpleConversation conversation)
        {
            return CurrentChatViewModel == null || conversation != CurrentChatViewModel.Conversation || AddNewMessage?.Invoke(this, conversation) == true;
        }

        private void ChatViewModelOnNewMessagesChanged(object sender, EventArgs e)
        {
            UpdateNewMessages();
        }

        private void UpdateNewMessages()
        {
            NewMessages = 0;

            foreach (var chatViewModel in Conversations)
            {
                NewMessages = NewMessages + chatViewModel.Conversation.NewMessages;
            }
        }

        public void Load(SimpleUser user)
        {
            User = user;
            LoadConversations();
            LoadPendingMessages();
        }

        private void LoadConversations()
        {
            var conversations = new CappuMessageController(User).GetConversations();
            foreach (var conversation in conversations)
            {
                AddCappuChatViewModel(conversation);
            }
        }

        private async Task LoadPendingMessages()
        {
            var pendingMessages = await _signalHelperFacade.ChatSignalHelper.GetPendingMessages().ConfigureAwait(true);
            if (pendingMessages.Any())
            {
                foreach (var pendingMessage in pendingMessages)
                {
                    HandleMessageReceived(pendingMessage);

                    if (TryGetConversationByUsername(pendingMessage.Sender.Username, out var chatViewModel))
                        chatViewModel.HandleReceivedMessage(pendingMessage, true);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _signalHelperFacade.ChatSignalHelper.PrivateMessageReceivedHandler -=
                    SignalHelperOnMessageReceived;

                foreach (var viewModel in Conversations)
                {
                    viewModel.ConversationHelper.AddNewMessage -= ChatViewModelOnAddNewMessage;
                    viewModel.ConversationHelper.NewMessagesChanged -= ChatViewModelOnNewMessagesChanged;
                    viewModel.Dispose();
                }

                Conversations.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
