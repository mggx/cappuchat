using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CappuChat.Converters
{
    [ValueConversion(typeof(bool), typeof(SolidColorBrush))]
    public class BooleanToConnectionColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? new SolidColorBrush(Colors.AliceBlue) : new SolidColorBrush(Color.FromRgb(128, 0, 0));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
