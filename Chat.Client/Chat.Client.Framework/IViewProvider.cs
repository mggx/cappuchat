namespace Chat.Client.Framework
{
    public interface IViewProvider
    {
        void Show(IDialog dialog);
        void ShowDialog(IModalDialog modalDialog);
        void Close(IDialog dialog);
        void Hide(IDialog dialog);
        void ShowMessage(string title, string message);
        void Focus(IDialog dialog);
        void ShowToastNotification(string message, NotificationType notificationType);
        void FlashWindow(bool checkFocus = true);
    }
}
