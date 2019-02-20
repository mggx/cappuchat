using System;
using System.Globalization;
using System.Windows.Data;

namespace Chat.Client.Converters
{
    public class BooleanToSafeModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool givenValue = (bool)value;
            if (givenValue)
                return "Safe mode is on";
            else
                return "Safe mode is off";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
