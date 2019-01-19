using System.Windows;
using Chat.Client.CustomNotifications.Notifications;
using ToastNotifications.Core;
using ToastNotifications.Messages.Core;

namespace Chat.Client.CustomNotifications.DisplayParts
{
    /// <summary>
    /// Interaction logic for CappuCallDisplayPart.xaml
    /// </summary>
    public partial class CappuCallDisplayPart
    {
        private MessageBase<CappuCallDisplayPart> _customNotificationBase;

        public CappuCallDisplayPart(MessageBase<CappuCallDisplayPart> customNotificationBase)
        {
            _customNotificationBase = customNotificationBase;
            DataContext = customNotificationBase; // this allows to bind ui with data in notification
            InitializeComponent();
        }

        private void GoButtonOnClick(object sender, RoutedEventArgs e)
        {
            this.OnClose();
        }
    }
}
