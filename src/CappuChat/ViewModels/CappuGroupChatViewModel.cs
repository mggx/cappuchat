using CappuChat;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Client.ViewModels.Delegates;
using Chat.Client.ViewModels.Extensions;
using Chat.Client.ViewModels.Helpers;
using Chat.Client.ViewModels.Providers;
using Chat.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace Chat.Client.ViewModels
{
    public class CappuGroupChatViewModel : CappuChatViewModelBase
    {
        private readonly IViewProvider _viewProvider;

        private readonly ImageHelper _imageHelper = new ImageHelper();

        public ProgressProvider ProgressProvider { get; } = new ProgressProvider();

        public event OpenChatHandler OpenChat;

        public RelayCommand<string> OpenPrivateChatCommand { get; set; }

        public CappuGroupChatViewModel(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider) : base(signalHelperFacade)
        {
            _viewProvider = viewProvider ?? throw new ArgumentNullException(nameof(viewProvider));

            OpenPrivateChatCommand = new RelayCommand<string>(OpenPrivateChat, CanOpenPrivateChat);

            Initialize();
        }

        protected override void RaiseCanExecuteChanged()
        {
            base.RaiseCanExecuteChanged();
            OpenPrivateChatCommand?.RaiseCanExecuteChanged();
        }

        private bool CanOpenPrivateChat(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return SelectedMessage?.Sender.Username.Equals(username, StringComparison.OrdinalIgnoreCase) == false;
            return !User.Username.Equals(username, StringComparison.OrdinalIgnoreCase);
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
            SignalHelperFacade.ChatSignalHelper.MessageReceivedHandler += SignalHelperOnMessageReceived;
        }

        protected override async void SendMessage(string message)
        {
            using (ProgressProvider.StartProgress())
            {
                var simpleMessage = new OwnSimpleMessage(User, message) { MessageSentDateTime = DateTime.Now };

                if (File.Exists(MessageImagePath))
                {
                    using (var fileStream = File.OpenRead(MessageImagePath))
                    {
                        var imageName = Guid.NewGuid().ToString();
                        var memoryStream = new MemoryStream();
                        fileStream.CopyTo(memoryStream);

                        simpleMessage.ImageStream = memoryStream;
                        simpleMessage.Base64ImageString = Convert.ToBase64String(memoryStream.ToArray());
                        simpleMessage.ImageName = imageName;

                        _imageHelper.AddImageStream(memoryStream, imageName);
                    }
                }

                MessageImagePath = string.Empty;

                simpleMessage.IsLocalMessage = true;
                Messages.Add(simpleMessage);

                simpleMessage.ImageUploading = true;
                await SignalHelperFacade.ChatSignalHelper.SendMessage(simpleMessage).ConfigureAwait(true);
                simpleMessage.ImageUploading = false;
            }
        }

        protected override void SignalHelperOnMessageReceived(MessageReceivedEventArgs eventArgs)
        {
            var ownSimpleMessage = new OwnSimpleMessage(eventArgs.ReceivedMessage);

            if (!string.IsNullOrWhiteSpace(eventArgs.ReceivedMessage.Base64ImageString))
            {
                var memoryStream = new MemoryStream(Convert.FromBase64String(eventArgs.ReceivedMessage.Base64ImageString));
                ownSimpleMessage.ImageStream = memoryStream;
                _imageHelper.AddImageStream(memoryStream, eventArgs.ReceivedMessage.ImageName);
            }

            Messages.Add(ownSimpleMessage);
            Application.Current.Dispatcher.Invoke(() => { });

            string message = eventArgs.ReceivedMessage.Message;
            string username = SignalHelperFacade.LoginSignalHelper.User.Username;

            if (message.Contains($"@{username}", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!_viewProvider.IsMainWindowFocused())
                    _viewProvider.ShowToastNotification($"{eventArgs.ReceivedMessage.Sender.Username}: {message}", NotificationType.Dark);
            }

            _viewProvider.FlashWindow();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imageHelper.Dispose();
                SignalHelperFacade.ChatSignalHelper.MessageReceivedHandler -= SignalHelperOnMessageReceived;
            }

            base.Dispose(disposing);
        }

        protected override async void ChangedStatus(SessionSwitchReason switchReason)
        {
            await SignalHelperFacade.LoginSignalHelper.SwitchStatus(User.Username, switchReason).ConfigureAwait(false);
        }
    }
}
