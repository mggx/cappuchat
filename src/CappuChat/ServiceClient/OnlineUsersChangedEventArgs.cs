using System;
using System.Collections.Generic;
using CappuChat;

namespace Chat.Client.SignalHelpers.Contracts.Events
{
    public class OnlineUsersChangedEventArgs : EventArgs
    {
        public IEnumerable<SimpleUser> OnlineUsers { get; set; }

        public OnlineUsersChangedEventArgs(IEnumerable<SimpleUser> onlineUsers)
        {
            OnlineUsers = onlineUsers ?? throw new ArgumentNullException(nameof(onlineUsers));
        }
    }
}
