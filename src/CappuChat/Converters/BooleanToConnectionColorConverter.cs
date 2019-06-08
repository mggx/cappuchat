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
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Cannot convert boolean to ConnectionColor. Given value is null.");

            var boolean = (bool) value;
            if (boolean)
                return new SolidColorBrush(Colors.AliceBlue);
            return new SolidColorBrush(Color.FromRgb(128, 0, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
