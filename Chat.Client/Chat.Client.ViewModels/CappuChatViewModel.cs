using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Client.ViewModels.Controllers;
using Chat.Client.ViewModels.Helpers;
using Chat.Models;
using Chat.Shared.Models;
using System;
using System.Collections.Generic;

namespace Chat.Client.ViewModels
{
    public class CappuChatViewModel : CappuChatViewModelBase
    {
        private CappuMessageController _cappuMessageController;

        public ConversationHelper ConversationHelper { get; set; } 
        public SimpleConversation Conversation { get; }

        public CappuChatViewModel(ISignalHelperFacade signalHelperFacade, SimpleConversation conversation) : base(signalHelperFacade, false)
        {
            if (conversation == null)
                throw new ArgumentNullException(nameof(conversation),
                    "Cannot create CappuChatViewModel. Given conversation is null.");
            Conversation = conversation;

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
                Messages.Add(message);
            ConversationHelper = new ConversationHelper(Conversation, Messages);
        }

        private void InitializeSignalHelperFacadeEvents()
        {
            SignalHelperFacade.ChatSignalHelper.PrivateMessageReceivedHandler += ChatSignalHelperOnMessageReceived;
        }

        protected override void ChatSignalHelperOnMessageReceived(MessageReceivedEventArgs eventArgs)
        {
            if (!eventArgs.ReceivedMessage.Sender.Username.Equals(Conversation.TargetUsername, StringComparison.CurrentCultureIgnoreCase))
                return;
            Messages.Add(eventArgs.ReceivedMessage);
            _cappuMessageController.StoreMessage(eventArgs.ReceivedMessage);
        }

        protected override void SendMessage(string message)
        {
            var simpleMessage = new SimpleMessage(User, new SimpleUser(Conversation.TargetUsername), message);
            simpleMessage.MessageSentDateTime = DateTime.Now;

            _cappuMessageController.StoreOwnMessage(simpleMessage);
            Messages.Add(simpleMessage);
            simpleMessage.IsLocalMessage = false;
            SignalHelperFacade.ChatSignalHelper.SendPrivateMessage(simpleMessage);
        }

        public void Load(SimpleMessage message)
        {
            Messages.Add(message);
            _cappuMessageController.StoreMessage(message);
        }

        public void Load(IEnumerable<SimpleMessage> messages)
        {
            foreach (var message in messages)
            {
                Load(message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SignalHelperFacade.ChatSignalHelper.PrivateMessageReceivedHandler -= ChatSignalHelperOnMessageReceived;
                ConversationHelper.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
