using System;
using System.Globalization;
using System.Windows.Data;

namespace Chat.Client.Converters
{
    public class BooleanToShowNotificationsStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolean)
                return boolean ? Texts.Texts.PushNotificationsOn : Texts.Texts.PushNotificationsOff;
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
