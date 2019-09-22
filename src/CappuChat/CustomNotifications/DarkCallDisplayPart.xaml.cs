using System.Windows;
using ToastNotifications.Messages.Core;

namespace Chat.Client.CustomNotifications.DisplayParts
{
    public partial class DarkCallDisplayPart
    {
        private readonly MessageBase<DarkCallDisplayPart> _customNotificationBase;

        public DarkCallDisplayPart(MessageBase<DarkCallDisplayPart> customNotificationBase)
        {
            _customNotificationBase = customNotificationBase;
            DataContext = customNotificationBase;
            InitializeComponent();
        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            this.OnClose();
        }
    }
}
