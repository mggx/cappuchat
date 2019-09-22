using CappuChat;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Client.ViewModels.Controllers;
using Chat.Client.ViewModels.Helpers;
using Chat.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Chat.Client.ViewModels
{
    public class CappuChatViewModel : CappuChatViewModelBase
    {
        private CappuMessageController _cappuMessageController;

        private readonly IViewProvider _viewProvider;

        public ConversationHelper ConversationHelper { get; set; }
        public SimpleConversation Conversation { get; }

        public CappuChatViewModel(ISignalHelperFacade signalHelperFacade, SimpleConversation conversation, IViewProvider viewProvider) : base(signalHelperFacade)
        {
            _viewProvider = viewProvider ?? throw new ArgumentNullException(nameof(viewProvider));
            Conversation = conversation ?? throw new ArgumentNullException(nameof(conversation));

            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            InitializeCappuMessageController();
            InitializeConversation();
            InitializeSignalHelperFacadeEvents();
        }

        private void InitializeCappuMessageController()
        {
            _cappuMessageController = new CappuMessageController(User);
        }

        private void InitializeConversation()
        {
            IEnumerable<SimpleMessage> conversation = _cappuMessageController.GetConversation(new SimpleUser(Conversation.TargetUsername));
            foreach (var message in conversation)
                Messages.Add(new OwnSimpleMessage(message));
            ConversationHelper = new ConversationHelper(Conversation, Messages);
        }

        private void InitializeSignalHelperFacadeEvents()
        {
            SignalHelperFacade.ChatSignalHelper.PrivateMessageReceivedHandler += SignalHelperOnMessageReceived;
        }

        protected override void SignalHelperOnMessageReceived(MessageReceivedEventArgs eventArgs)
        {
            HandleReceivedMessage(eventArgs.ReceivedMessage);
        }

        public void HandleReceivedMessage(SimpleMessage receivedMessage, bool pendingMessage = false)
        {
            var sender = receivedMessage.Sender.Username;

            if (!sender.Equals(Conversation.TargetUsername, StringComparison.CurrentCultureIgnoreCase))
                return;

            var message = receivedMessage.Message;
            var messageToShow = message.Replace("--urgent", string.Empty);

            if (!_viewProvider.IsMainWindowFocused() && !pendingMessage)
                _viewProvider.ShowToastNotification(
                    string.Format(CultureInfo.CurrentCulture, CappuChat.Properties.Strings.PrivateMessageNotification_UserName_Message, sender, messageToShow),
                    NotificationType.Dark,
                    message.Contains("--urgent")
                );

            Messages.Add(new OwnSimpleMessage(receivedMessage));
            _cappuMessageController.StoreMessage(receivedMessage);
        }

        protected override void SendMessage(string message)
        {
            var simpleMessage = new OwnSimpleMessage(User, new SimpleUser(Conversation.TargetUsername), message);
            simpleMessage.MessageSentDateTime = DateTime.Now;

            _cappuMessageController.StoreOwnMessage(simpleMessage);
            Messages.Add(simpleMessage);
            simpleMessage.IsLocalMessage = false;
            SignalHelperFacade.ChatSignalHelper.SendPrivateMessage(simpleMessage);
        }

        public void Load(SimpleMessage message)
        {
            Messages.Add(new OwnSimpleMessage(message));
            _cappuMessageController.StoreMessage(message);
        }

        public void Load(IEnumerable<SimpleMessage> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            foreach (var message in messages)
            {
                Load(message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SignalHelperFacade.ChatSignalHelper.PrivateMessageReceivedHandler -= SignalHelperOnMessageReceived;
                ConversationHelper.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
