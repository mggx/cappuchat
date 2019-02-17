using System.Windows.Input;
using Chat.Client.CustomNotifications.Notifications;
using ToastNotifications;

namespace Chat.Client.CustomNotifications.Extensions
{
    public static class CustomCallExtensions
    {
        public static void ShowDarkMessage(this Notifier notifier, string message, string buttonContent, ICommand command = null)
        {
            notifier.Notify<DarkCallNotification>(() => new DarkCallNotification(message, buttonContent, command));
        }
    }
}
