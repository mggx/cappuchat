using System;
using System.Collections.Generic;
using Chat.Shared.Models;

namespace Chat.Client.SignalHelpers.Contracts.Events
{
    public class OnlineUsersChangedEventArgs : EventArgs
    {
        public IEnumerable<SimpleUser> OnlineUsers { get; set; }

        public OnlineUsersChangedEventArgs(IEnumerable<SimpleUser> onlineUsers)
        {
            if (onlineUsers == null)
                throw new ArgumentNullException(nameof(onlineUsers), "Cannot create OnlineUsersChangedEventArgs. Given onlineUsers is null.");

            OnlineUsers = onlineUsers;
        }
    }
}
