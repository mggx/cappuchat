using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Chat.Client.ViewModels.Controllers;

namespace Chat.Client.ViewModels
{
    public class CappuChatViewModel : ViewModelBase, IDialog
    {
        private readonly object _lockObject = new object();

        private readonly SimpleUser _user;
        private readonly SimpleUser _targetUser;

        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly CappuMessageController _cappuMessageController;

        private ObservableCollection<SimpleMessage> _messages = new ObservableCollection<SimpleMessage>();
        public ObservableCollection<SimpleMessage> Messages
        {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged(); }
        }

        public event EventHandler<SimpleUser> PrivateMessageWindowClosed;

        public AsyncRelayCommand<string> SendMessageCommand { get; }

        public RelayCommand CloseCommand { get; }

        public CappuChatViewModel(ISignalHelperFacade signalHelperFacade, SimpleUser user, SimpleUser targetUser)
        {
            _signalHelperFacade = signalHelperFacade;
            _user = user;
            _targetUser = targetUser;
            _cappuMessageController = new CappuMessageController(user);

            SendMessageCommand = new AsyncRelayCommand<string>(SendMessage, ErrorHandler);
            CloseCommand = new RelayCommand(Close);

            BindingOperations.EnableCollectionSynchronization(_messages, _lockObject);

            Initialize();
        }

        private void Initialize()
        {
            IEnumerable<SimpleMessage> conversation = _cappuMessageController.GetConversation(_user, _targetUser);
            foreach (var message in conversation)
            {
                Messages.Add(message);
            }

            InitializeSignalHelperFacadeEvents();
        }

        private void InitializeSignalHelperFacadeEvents()
        {
            _signalHelperFacade.ChatSignalHelper.PrivateMessageReceivedHandler += ChatSignalHelperOnMessageReceived;
        }

        private void ChatSignalHelperOnMessageReceived(MessageReceivedEventArgs eventArgs)
        {
            if (eventArgs.ReceivedMessage.Sender.Username == _targetUser.Username)
            {
                Messages.Add(eventArgs.ReceivedMessage);
                _cappuMessageController.StoreMessage(eventArgs.ReceivedMessage);
            }
        }

        private void SendMessage(string message)
        {
            var simpleMessage = new SimpleMessage(_user, _targetUser, message);
            simpleMessage.MessageSentDateTime = DateTime.Now;
            Messages.Add(simpleMessage);
            _cappuMessageController.StoreOwnMessage(simpleMessage);
            _signalHelperFacade.ChatSignalHelper.SendPrivateMessage(simpleMessage);
        }

        private void Close()
        {
            PrivateMessageWindowClosed?.Invoke(this, _targetUser);
        }

        protected override void ErrorHandlerOnExceptionOcurred(Exception exception)
        {
            base.ErrorHandlerOnExceptionOcurred(exception);
            throw exception;
        }

        public override void Dispose()
        {
            _signalHelperFacade.ChatSignalHelper.PrivateMessageReceivedHandler -= ChatSignalHelperOnMessageReceived;

            base.Dispose();
        }
    }
}
