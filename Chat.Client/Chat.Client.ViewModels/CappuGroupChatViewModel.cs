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
        private readonly IViewProvider _viewProvider;

        public event OpenChatHandler OpenChat;

        public RelayCommand<string> OpenPrivateChatCommand { get; set; }

        public CappuGroupChatViewModel(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider) : base(signalHelperFacade)
        {
            if (viewProvider == null)
                throw new ArgumentNullException(nameof(viewProvider),
                    "Cannot create CappuGroupChatViewModel. Given viewProvider is null");
            _viewProvider = viewProvider;

            OpenPrivateChatCommand = new RelayCommand<string>(OpenPrivateChat, CanOpenPrivateChat);
        }

        protected override void RaiseCanExecuteChanged()
        {
            base.RaiseCanExecuteChanged();
            OpenPrivateChatCommand?.RaiseCanExecuteChanged();
        }

        private bool CanOpenPrivateChat(string username)
        {
            return SelectedMessage != null || !string.IsNullOrWhiteSpace(username) && !User.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase);
        }

        private void OpenPrivateChat(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                OpenChat?.Invoke(new SimpleUser(username));
                return;
            }

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
            var simpleMessage = new SimpleMessage(User, message, 0) { MessageSentDateTime = DateTime.Now };
            await SignalHelperFacade.ChatSignalHelper.SendMessage(simpleMessage);
            simpleMessage.IsLocalMessage = true;
            Messages.Add(simpleMessage);
        }

        protected override void ChatSignalHelperOnMessageReceived(MessageReceivedEventArgs eventArgs)
        {
            Messages.Add(eventArgs.ReceivedMessage);
            _viewProvider.FlashWindow();
        }
    }
}
