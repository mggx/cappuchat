using System;
using System.Globalization;
using System.Windows.Data;

namespace CappuChat.Converters
{
    public class BooleanToAppNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool givenValue = (bool)value;
            if (givenValue)
                return "Mail";
            else
                return "CappuChat";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
