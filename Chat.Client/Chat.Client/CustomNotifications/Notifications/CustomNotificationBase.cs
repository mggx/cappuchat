using System.ComponentModel;
using System.Runtime.CompilerServices;
using Chat.Client.Annotations;
using Chat.Client.CustomNotifications.DisplayParts;
using ToastNotifications.Core;

namespace Chat.Client.CustomNotifications.Notifications
{
    public abstract class CustomNotificationBase : NotificationBase, INotifyPropertyChanged
    {
        protected abstract NotificationDisplayPart GetDisplayPart();

        public override NotificationDisplayPart DisplayPart => GetDisplayPart();

        private string _message;
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        protected CustomNotificationBase(string message)
        {
            Message = message;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
