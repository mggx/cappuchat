using System;
using System.Windows.Input;
using Chat.Client.CustomNotifications.DisplayParts;
using ToastNotifications.Core;
using ToastNotifications.Messages.Core;

namespace Chat.Client.CustomNotifications.Notifications
{
    public class CappuCallNotification : MessageBase<CappuCallDisplayPart>
    {
        public ICommand GoCommand { get; set; }

        //protected override NotificationDisplayPart GetDisplayPart()
        //{
        //    return new CappuCallDisplayPart(this);
        //}

        public CappuCallNotification(string message, ICommand command) : base(message, new MessageOptions())
        {
            if (command == null)
                throw new ArgumentNullException(nameof(ICommand), "Cannot create CappuCallNotification. Given command is null.");
            GoCommand = command;
        }

        protected override void UpdateDisplayOptions(CappuCallDisplayPart displayPart, MessageOptions options)
        {
        }

        protected override CappuCallDisplayPart CreateDisplayPart()
        {
            return new CappuCallDisplayPart(this);
        }
    }
}
