using System.Windows.Input;
using Chat.Client.CustomNotifications.Notifications;
using ToastNotifications;

namespace Chat.Client.CustomNotifications.Extensions
{
    public static class CustomCallExtensions
    {
        public static void ShowCappuCallMessage(this Notifier notifier, string message, ICommand command = null)
        {
            notifier.Notify<CappuCallNotification>(() => new CappuCallNotification(message, command));
        }
    }
}
