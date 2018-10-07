using System;
using System.Threading.Tasks;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Chat.Responses;
using Chat.Shared.Models;
using Microsoft.AspNet.SignalR.Client;

namespace Chat.Client.SignalHelpers
{
    public class RegisterSignalHelper : IRegisterSignalHelper
    {
        private readonly IHubProxy _hubProxy;

        public RegisterSignalHelper(IHubProxy hubProxy)
        {
            if (hubProxy == null)
                throw new ArgumentNullException(nameof(hubProxy), "Cannot create ChatSignalHelper. Given hubProxy is null.");
            _hubProxy = hubProxy;
        }

        public async Task<SimpleUser> Register(string username, string password)
        {
            Task<SimpleRegisterResponse> task = _hubProxy.Invoke<SimpleRegisterResponse>("Register", username, password);
            if (task == null)
                throw new NullServerResponseException("Retrieved null server response.");

            SimpleRegisterResponse serverResponse = await task;
            if (!serverResponse.Success)
                throw new RequestFailedException(serverResponse.ErrorMessage);
            return serverResponse.User;
        }
    }
}
