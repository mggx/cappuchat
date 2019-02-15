using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        public RelayCommand<string> SendSpongeMessageCommand { get; }
        public RelayCommand ClearMessagesCommand { get; set; }

        public CappuChatViewModelBase(ISignalHelperFacade signalHelperFacade, bool initialize = true)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade),
                    "Cannot create CappuChatViewModelBase. Given signalHelperFacade is null.");
            SignalHelperFacade = signalHelperFacade;

            SendMessageCommand = new RelayCommand<string>(SendMessage, CanSendMessage);
            SendSpongeMessageCommand = new RelayCommand<string>(SendSpongeMessage);
            ClearMessagesCommand = new RelayCommand(ClearMessages);

            if (initialize)
                Initialize();
        }

        private void SendSpongeMessage(string message)
        {
            var randomizer = new Random();
            var final =
                message.Select(x => randomizer.Next() % 2 == 0 ?
                (char.IsUpper(x) ? x.ToString().ToLower().First() : x.ToString().ToUpper().First()) : x);
            var randomUpperLower = new string(final.ToArray());
            SendMessage(randomUpperLower);
        }

        protected virtual void Initialize()
        {
            User = SignalHelperFacade.LoginSignalHelper.User;
        }

        //private bool CanSendSpongeMessage(string message)
        //{
        //    return !string.IsNullOrWhiteSpace(message);
        //}

        protected virtual bool CanSendMessage(string message)
        {
            var result = !string.IsNullOrWhiteSpace(message);
            return result;
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
