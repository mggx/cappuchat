using System;
using System.Threading.Tasks;
using CappuChat;
using CappuChat.DTOs;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Microsoft.AspNet.SignalR.Client;

namespace Chat.Client.SignalHelpers
{
    public class RegisterSignalHelper : IRegisterSignalHelper
    {
        private readonly IHubProxy _hubProxy;

        public RegisterSignalHelper(IHubProxy hubProxy)
        {
            _hubProxy = hubProxy ?? throw new ArgumentNullException(nameof(hubProxy));
        }

        public async Task<SimpleUser> Register(string username, string password)
        {
            var serverResponse =  await _hubProxy.Invoke<SimpleRegisterResponse>("Register", username, password).ConfigureAwait(false);
            if (!serverResponse.Success)
                throw new RequestFailedException(serverResponse.ErrorMessage);
            return serverResponse.User;
        }
    }
}
