using System;
using System.Globalization;
using System.Windows.Data;

namespace CappuChat.Converters
{
    public class BooleanToShowNotificationsStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolean)
                return boolean ? Chat.Texts.Texts.PushNotificationsOn : Chat.Texts.Texts.PushNotificationsOff;
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
