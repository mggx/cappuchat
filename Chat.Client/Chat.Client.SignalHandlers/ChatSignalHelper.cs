using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Delegates;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Chat.Client.SignalHelpers.Helper;
using Chat.Responses;
using Chat.Shared.Models;
using Microsoft.AspNet.SignalR.Client;

namespace Chat.Client.SignalHelpers
{
    public class ChatSignalHelper : IChatSignalHelper
    {
        private readonly IHubProxy _chatHubProxy;

        public event MessageReceivedHandler MessageReceivedHandler;
        public event MessageReceivedHandler PrivateMessageReceivedHandler;


        public ChatSignalHelper(IHubProxy chatHubProxy)
        {
            if (chatHubProxy == null)
                throw new ArgumentNullException(nameof(chatHubProxy), "Cannot create ChatSignalHelper. Given chatHubProxy is null.");
            _chatHubProxy = chatHubProxy;

            RegisterHubProxyEvents();
        }

        public void RegisterHubProxyEvents()
        {
            _chatHubProxy.On<SimpleMessage>("OnMessageReceived", ChatHubProxyOnMessageReceived);
            _chatHubProxy.On<SimpleMessage>("OnPrivateMessageReceived", ChatHubProxyOnPrivateMessageReceived);
        }

        private void ChatHubProxyOnMessageReceived(SimpleMessage receivedMessage)
        {
            receivedMessage.IsLocalMessage = false;

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageReceivedHandler?.Invoke(new MessageReceivedEventArgs(CipherHelper.DecryptMessage(receivedMessage)));
            });
        }

        private void ChatHubProxyOnPrivateMessageReceived(SimpleMessage message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                PrivateMessageReceivedHandler?.Invoke(new MessageReceivedEventArgs(CipherHelper.DecryptMessage(message)));
            });
        }

        public async Task<IEnumerable<SimpleUser>> GetOnlineUsers()
        {
            var task = _chatHubProxy.Invoke<GetOnlineUsersResponse>("GetOnlineUsers");
            if (task == null)
                throw new NullServerResponseException("Retrieved null task from server.");

            GetOnlineUsersResponse serverResponse = await task;

            if (!serverResponse.Success)
                throw new RequestFailedException(serverResponse.ErrorMessage);

            return serverResponse.OnlineUserList;
        }

        public async Task SendMessage(SimpleMessage message)
        {
            var task = _chatHubProxy.Invoke("SendMessage", CipherHelper.EncryptMessage(message));
            if (task == null)
                throw new NullServerResponseException("Retrieved null task from server.");
            await task;
        }

        public async Task SendPrivateMessage(SimpleMessage message)
        {
            var task = _chatHubProxy.Invoke<BaseResponse>("SendPrivateMessage", CipherHelper.EncryptMessage(message));
            if (task == null)
                throw new NullServerResponseException("Retrieved null task from server.");
            await task;
        }
    }
}
