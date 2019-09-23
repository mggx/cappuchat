using System;
using System.Threading.Tasks;
using CappuChat.DTOs;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Win32;

namespace CappuChat.ServiceClient
{
    public class UserStatusSignalHelper : IUserStatusSignalHelper
    {
        private readonly IHubProxy _userHubProxy;

        public UserStatusSignalHelper(IHubProxy userHubProxy)
        {
            _userHubProxy = userHubProxy ?? throw new ArgumentNullException(nameof(userHubProxy));

            _userHubProxy.On<SessionSwitchReason>("OnUserStatusChanged", UserStatusHubProxyOnUserStatusChanged);
        }

        private void UserStatusHubProxyOnUserStatusChanged(SessionSwitchReason switchReason)
        {

        }

        public async Task SetUserStatus(SessionSwitchReason switchReason)
        {
            await _userHubProxy.Invoke<BaseResponse>("SetUserStatus", switchReason).ConfigureAwait(false);
        }
    }
}
