using CappuChat;
using CappuChat.DTOs;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Delegates;
using Chat.Client.SignalHelpers.Contracts.Events;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Chat.Client.SignalHelpers.Helper;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Chat.Client.SignalHelpers
{
    public class LoginSignalHelper : ILoginSignalHelper
    {
        private readonly IHubProxy _loginHubProxy;

        public SimpleUser User { get; set; }

        public event EventHandler<string> ConnectionIdChanged;
        public event EventHandler<string> LoggedOutByServer;
        public event OnlineUsersChangedHandler OnlineUsersChanged;

        public LoginSignalHelper(IHubProxy loginHubProxy)
        {
            _loginHubProxy = loginHubProxy ?? throw new ArgumentNullException(nameof(loginHubProxy));

            RegisterHubProxyEvents();
        }

        private void RegisterHubProxyEvents()
        {
            _loginHubProxy.On<string>("OnReconnected", LoginHubProxyOnReconnected);
            _loginHubProxy.On<string>("OnClientLoggedOut", LoginHubProxyOnLoggedOut);
            _loginHubProxy.On<IEnumerable<SimpleUser>>("OnOnlineUsersChanged", ChatHubProxyOnOnlineUsersChanged);
        }

        private void LoginHubProxyOnLoggedOut(string reason)
        {
            Application.Current.Dispatcher.Invoke(() => { LoggedOutByServer?.Invoke(this, reason); });
        }

        private void LoginHubProxyOnReconnected(string connectionId)
        {
            Application.Current.Dispatcher.Invoke(() => { ConnectionIdChanged?.Invoke(this, connectionId); });
        }

        private void ChatHubProxyOnOnlineUsersChanged(IEnumerable<SimpleUser> obj)
        {
            Application.Current.Dispatcher.Invoke(() => { OnlineUsersChanged?.Invoke(new OnlineUsersChangedEventArgs(obj)); });
        }

        public async Task<SimpleUser> Login(string username, string password)
        {
            var serverResponse = await _loginHubProxy.Invoke<SimpleLoginResponse>("Login", username, password).ConfigureAwait(false);

            if (!serverResponse.Success)
                throw new LoginFailedException(serverResponse.ErrorMessage);

            User = serverResponse.User;

            return serverResponse.User;
        }

        public async Task Logout()
        {
            await _loginHubProxy.Invoke("Logout").ConfigureAwait(false);
        }

        public async Task SwitchStatus(string username, SessionSwitchReason switchReason)
        {
            await _loginHubProxy.Invoke("SwitchUserStatus", username).ConfigureAwait(false);
        }
    }
}
