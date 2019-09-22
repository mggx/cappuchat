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
                return boolean ? CappuChat.Properties.Strings.PushNotificationsOn : CappuChat.Properties.Strings.PushNotificationsOff;
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
