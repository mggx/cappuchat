using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CappuChat.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolVal)
            {
                return boolVal ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                throw new ArgumentException(CappuChat.Properties.Errors.ConverterInputArgumentInvalid, nameof(value));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
