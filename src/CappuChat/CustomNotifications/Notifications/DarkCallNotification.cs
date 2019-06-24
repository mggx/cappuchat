using Chat.Client.CustomNotifications.DisplayParts;
using System;
using System.Windows.Input;
using ToastNotifications.Core;
using ToastNotifications.Messages.Core;

namespace Chat.Client.CustomNotifications.Notifications
{
    public class DarkCallNotification : MessageBase<DarkCallDisplayPart>
    {
        public ICommand Command { get; set; }
        public string ButtonContent { get; set; }

        public DarkCallNotification(string message, string buttonContent, ICommand command) : base(message, new MessageOptions())
        {
            Command = command;
            ButtonContent = buttonContent;
        }


        protected override void UpdateDisplayOptions(DarkCallDisplayPart displayPart, MessageOptions options)
        {
        }

        protected override DarkCallDisplayPart CreateDisplayPart()
        {
            return new DarkCallDisplayPart(this);
        }
    }
}
