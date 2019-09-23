using Chat.Server.Hubs;
using Microsoft.Win32;
using System;

namespace CappuService.Hubs
{
    public class UserStatusHub : BaseHub
    {
        public void SetUserStatus(SessionSwitchReason switchReason)
        {
            Clients.Others.OnUserStatusChanged(switchReason);
        }
    }
}
