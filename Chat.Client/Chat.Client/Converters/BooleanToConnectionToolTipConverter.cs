using System;
using System.Globalization;
using System.Windows.Data;

namespace Chat.Client.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BooleanToConnectionToolTipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Cannot convert boolean to Context tooltip. Given value is null.");

            var boolean = (bool) value;
            if (boolean)
                return Texts.Texts.ConnectedToServer;
            return Texts.Texts.ClickToReconnect;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
