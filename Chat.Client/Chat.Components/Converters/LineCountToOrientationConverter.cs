using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ChatComponents.Converters
{
    public class LineCountToOrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int lineCount)
            {
                return lineCount > 1 ? Orientation.Vertical : Orientation.Horizontal;
            }

            return Orientation.Horizontal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
