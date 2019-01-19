using System.Windows.Input;

namespace Chat.Client.Framework
{
    public interface IViewProvider
    {
        void Show(IDialog dialog);
        void ShowDialog(IModalDialog modalDialog);
        void Close(IDialog dialog);
        void Hide(IDialog dialog);
        void ShowMessage(string title, string message);
        void BringToFront(IDialog dialog);
        void ShowToastNotification(string message, NotificationType notificationType, ICommand command = null);
        void FlashWindow(bool checkFocus = true);
        void BringToFront();
    }
}
