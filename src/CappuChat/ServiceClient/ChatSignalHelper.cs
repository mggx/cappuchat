using CappuChat;
using CappuChat.DTOs;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Delegates;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Chat.Client.SignalHelpers.Helper;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Chat.Client.SignalHelpers
{
    public class ChatSignalHelper : IChatSignalHelper
    {
        private readonly IHubProxy _chatHubProxy;

        public event MessageReceivedHandler MessageReceivedHandler;
        public event MessageReceivedHandler PrivateMessageReceivedHandler;


        public ChatSignalHelper(IHubProxy chatHubProxy)
        {
            _chatHubProxy = chatHubProxy ?? throw new ArgumentNullException(nameof(chatHubProxy));

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
            var serverResponse = await _chatHubProxy.Invoke<GetOnlineUsersResponse>("GetOnlineUsers").ConfigureAwait(false);

            if (!serverResponse.Success)
                throw new RequestFailedException(serverResponse.ErrorMessage);

            return serverResponse.OnlineUserList;
        }

        public async Task SendMessage(SimpleMessage message)
        {
            await _chatHubProxy.Invoke("SendMessage", CipherHelper.EncryptMessage(message)).ConfigureAwait(false);
        }

        public async Task SendPrivateMessage(SimpleMessage message)
        {
            await _chatHubProxy.Invoke<BaseResponse>("SendPrivateMessage", CipherHelper.EncryptMessage(message)).ConfigureAwait(false);
        }
    }
}
