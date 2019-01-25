using System;
using System.Globalization;
using System.Windows.Data;

namespace Chat.Client.Converters
{
    public class StringToPartialStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value?.ToString();
            if (!int.TryParse(parameter?.ToString(), out var prefixLength) || s?.Length <= prefixLength)
                return s;
            return s?.Substring(0, prefixLength);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
