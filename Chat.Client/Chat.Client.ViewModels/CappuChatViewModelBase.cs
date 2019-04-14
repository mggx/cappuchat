using System;
using System.Collections.ObjectModel;
using System.IO;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Models;
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

        private string _messageImagePath;
        public string MessageImagePath
        {
            get { return _messageImagePath; }
            set { _messageImagePath = value; OnPropertyChanged(); }
        }

        public SimpleUser User { get; set; }
        
        public ObservableCollection<OwnSimpleMessage> Messages { get; set; } = new ObservableCollection<OwnSimpleMessage>();

        public RelayCommand<string> SendMessageCommand { get; }
        public RelayCommand ClearMessagesCommand { get; set; }
        public RelayCommand<string> DataDroppedCommand { get; }

        public CappuChatViewModelBase(ISignalHelperFacade signalHelperFacade)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade),
                    "Cannot create CappuChatViewModelBase. Given signalHelperFacade is null.");
            SignalHelperFacade = signalHelperFacade;

            SendMessageCommand = new RelayCommand<string>(SendMessage, CanSendMessage);
            ClearMessagesCommand = new RelayCommand(ClearMessages);
            DataDroppedCommand = new RelayCommand<string>(DataDropped);
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

        protected virtual void DataDropped(string filePath)
        {
            MessageImagePath = filePath;
        }

        protected virtual void RaiseCanExecuteChanged()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Messages.Clear();
            }

            base.Dispose(disposing);
        }

        protected abstract void SendMessage(string message);
        protected abstract void ChatSignalHelperOnMessageReceived(MessageReceivedEventArgs eventArgs);
    }
}
