using System;
using System.Globalization;
using System.Windows.Data;

namespace Chat.Client.Converters
{
    public class BooleanToSaveModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool givenValue = (bool)value;
            if (givenValue)
                return "Save mode is on";
            else
                return "Save mode is off";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
