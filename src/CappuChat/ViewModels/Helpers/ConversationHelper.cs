using Chat.Client.Framework;
using Chat.Client.ViewModels.Delegates;
using Chat.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Chat.Client.ViewModels.Helpers
{
    public class ConversationHelper : Disposable
    {
        public SimpleConversation Conversation { get; }
        public ObservableCollection<OwnSimpleMessage> Messages { get; }

        public event AddNewMessageHandler AddNewMessage;
        public event EventHandler<EventArgs> NewMessagesChanged;

        public ConversationHelper(SimpleConversation conversation, ObservableCollection<OwnSimpleMessage> messages)
        {
            Conversation = conversation;
            Messages = messages;

            Initialize();
        }

        private void Initialize()
        {
            Messages.CollectionChanged += ConversationCollectionOnCollectionChanged;
            SetLastMessage();
        }

        private void ConversationCollectionOnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetLastMessage();

            if (e.Action != NotifyCollectionChangedAction.Add)
                return;

            if (!AddNewMessage?.Invoke(this, Conversation) == true)
                return;

            foreach (var unused in e.NewItems)
            {
                Conversation.NewMessages++;
            }

            NewMessagesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetLastMessage()
        {
            Conversation.LastMessage = Messages.LastOrDefault()?.Message;
        }

        public void ResetNewMessages()
        {
            Conversation.NewMessages = 0;
            NewMessagesChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Messages.CollectionChanged -= ConversationCollectionOnCollectionChanged;
            }

            base.Dispose(disposing);
        }
    }
}
