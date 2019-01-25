using System;
using System.Collections.ObjectModel;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Shared.Models;

namespace Chat.Client.ViewModels
{
    public abstract class CappuChatViewModelBase : ViewModelBase
    {
        protected readonly ISignalHelperFacade SignalHelperFacade;

        private SimpleMessage _selectedMessage;
        public SimpleMessage SelectedMessage
        {
            get { return _selectedMessage; }
            set { _selectedMessage = value; OnPropertyChanged(); RaiseCanExecuteChanged(); }
        }

        public SimpleUser User { get; set; }
        
        public ObservableCollection<SimpleMessage> Messages { get; set; } = new ObservableCollection<SimpleMessage>();

        public RelayCommand<string> SendMessageCommand { get; }
        public RelayCommand ClearMessagesCommand { get; set; }

        public CappuChatViewModelBase(ISignalHelperFacade signalHelperFacade, bool initialize = true)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade),
                    "Cannot create CappuChatViewModelBase. Given signalHelperFacade is null.");
            SignalHelperFacade = signalHelperFacade;

            SendMessageCommand = new RelayCommand<string>(SendMessage, CanSendMessage);
            ClearMessagesCommand = new RelayCommand(ClearMessages);

            if (initialize)
                Initialize();
        }

        protected virtual void Initialize()
        {
            User = SignalHelperFacade.LoginSignalHelper.User;
        }

        protected virtual bool CanSendMessage(string message)
        {
            return !string.IsNullOrWhiteSpace(message);
        }

        protected virtual void ClearMessages()
        {
            Messages.Clear();
        }

        protected virtual void RaiseCanExecuteChanged()
        {
        }

        protected abstract void SendMessage(string message);
        protected abstract void ChatSignalHelperOnMessageReceived(MessageReceivedEventArgs eventArgs);
    }
}
