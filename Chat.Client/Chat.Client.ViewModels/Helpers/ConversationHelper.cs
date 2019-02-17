using Chat.Client.Framework;
using Chat.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Chat.Client.ViewModels.Delegates;
using Chat.Client.ViewModels.Events;
using Chat.Shared.Models;

namespace Chat.Client.ViewModels.Helpers
{
    public class ConversationHelper : Disposable
    {
        public SimpleConversation Conversation { get; }
        public ObservableCollection<OwnSimpleMessage> Messages { get; set; }

        public event AddNewMessageHandler AddNewMessage;
        public event NewMessagesChangedHandler NewMessagesChanged;

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

            NewMessagesChanged?.Invoke(this, new NewMessagesChangedEventArgs());
        }

        private void SetLastMessage()
        {
            Conversation.LastMessage = Messages.LastOrDefault()?.Message;
        }

        public void ResetNewMessages()
        {
            Conversation.NewMessages = 0;
            NewMessagesChanged?.Invoke(this, new NewMessagesChangedEventArgs());
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
