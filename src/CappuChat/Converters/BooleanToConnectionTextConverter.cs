using System;
using System.Globalization;
using System.Windows.Data;

namespace CappuChat.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BooleanToConnectionTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Cannot convert boolean to text. Given value is null.");

            var boolean = (bool) value;
            if (boolean)
                return CappuChat.Properties.Strings.ConnectedToServer;
            return CappuChat.Properties.Strings.NotConnectedToServer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
