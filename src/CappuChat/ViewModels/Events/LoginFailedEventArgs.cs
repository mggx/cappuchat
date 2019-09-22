using System;

namespace Chat.Client.ViewModels.Events
{
    public class LoginFailedEventArgs : EventArgs
    {
        public string Reason { get; }

        public LoginFailedEventArgs(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException(CappuChat.Properties.Errors.CannotCreateLoginFailedArgsWithoutAReason, nameof(reason));
            Reason = reason;
        }
    }
}
