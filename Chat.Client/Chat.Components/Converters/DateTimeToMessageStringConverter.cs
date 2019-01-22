using System;
using System.Globalization;
using System.Windows.Data;

namespace ChatComponents.Converters
{
    public class DateTimeToMessageStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
                return $"{dateTime:hh:mm} {dateTime.ToString("tt", CultureInfo.InvariantCulture)}";
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
