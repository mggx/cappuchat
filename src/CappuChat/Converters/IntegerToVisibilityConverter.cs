using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CappuChat.Converters
{
    public class IntegerToVisibilityConverter : IValueConverter
    {
        public Visibility FalseCaseVisibility { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int integer))
                return FalseCaseVisibility;
            return integer > 0 ? Visibility.Visible : FalseCaseVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
