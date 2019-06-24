using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CappuChat.Converters
{
    public class BooleanToHorizontalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return HorizontalAlignment.Center;
            bool boolean = (bool) value;
            if (boolean)
                return HorizontalAlignment.Right;
            return HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
