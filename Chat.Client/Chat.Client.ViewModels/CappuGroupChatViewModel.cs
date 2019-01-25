using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Client.ViewModels.Delegates;
using Chat.Shared.Models;
using System;

namespace Chat.Client.ViewModels
{
    public class CappuGroupChatViewModel : CappuChatViewModelBase
    {
        
        public event OpenChatHandler OpenChat;

        public RelayCommand OpenPrivateChatCommand { get; set; }

        public CappuGroupChatViewModel(ISignalHelperFacade signalHelperFacade) : base(signalHelperFacade)
        {
            OpenPrivateChatCommand = new RelayCommand(OpenPrivateChat, CanOpenPrivateChat);
        }

        protected override void RaiseCanExecuteChanged()
        {
            base.RaiseCanExecuteChanged();
            OpenPrivateChatCommand?.RaiseCanExecuteChanged();
        }

        private bool CanOpenPrivateChat()
        {
            return SelectedMessage != null;
        }

        private void OpenPrivateChat()
        {
            OpenChat?.Invoke(SelectedMessage.Sender);
        }

        protected override void Initialize()
        {
            base.Initialize();
            InitializeSignalHelperFacadeEvents();
        }

        private void InitializeSignalHelperFacadeEvents()
        {
            SignalHelperFacade.ChatSignalHelper.MessageReceivedHandler += ChatSignalHelperOnMessageReceived;
        }

        protected override async void SendMessage(string message)
        {
            var simpleMessage = new SimpleMessage(User, message) { MessageSentDateTime = DateTime.Now };
            await SignalHelperFacade.ChatSignalHelper.SendMessage(simpleMessage);
            simpleMessage.IsLocalMessage = true;
            Messages.Add(simpleMessage);
        }

        protected override void ChatSignalHelperOnMessageReceived(MessageReceivedEventArgs eventArgs)
        {
            Messages.Add(eventArgs.ReceivedMessage);
        }
    }
}
