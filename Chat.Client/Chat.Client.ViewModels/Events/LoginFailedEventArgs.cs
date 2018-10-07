using System;

namespace Chat.Client.ViewModels.Events
{
    public class LoginFailedEventArgs : EventArgs
    {
        public string Reason { get; }

        public LoginFailedEventArgs(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentNullException(nameof(reason), "Cannot create LoginFailedEventArgs. Given reason is invalid.");
            Reason = reason;
        }
    }
}
