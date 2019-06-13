using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CappuChat.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Collapsing { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolean)) return Collapsing ? Visibility.Collapsed : Visibility.Hidden;
            if (boolean)
                return Visibility.Visible;
            return Collapsing ? Visibility.Collapsed : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
