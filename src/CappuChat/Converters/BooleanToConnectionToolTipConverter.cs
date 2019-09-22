using System;
using System.Globalization;
using System.Windows.Data;

namespace CappuChat.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BooleanToConnectionToolTipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return boolValue ? CappuChat.Properties.Strings.ConnectedToServer : CappuChat.Properties.Strings.ClickToReconnect;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
