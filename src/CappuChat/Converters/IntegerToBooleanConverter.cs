using System;
using System.Globalization;
using System.Windows.Data;

namespace CappuChat.Converters
{
    public class IntegerToBooleanConverter : IValueConverter
    {
        public bool Inverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int integer)
                return integer > 0 && !Inverse;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
