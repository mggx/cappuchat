using System;
using Chat.Shared.Models;

namespace Chat.Client.Viewmodels.Events
{
    public class LoginSucceededEventArgs : EventArgs
    {
        public SimpleUser User { get; }

        public LoginSucceededEventArgs(SimpleUser user)
        {
            User = user;
        }
    }
}
