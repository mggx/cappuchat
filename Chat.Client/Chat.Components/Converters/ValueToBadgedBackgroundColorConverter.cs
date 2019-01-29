using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ChatComponents.Converters
{
    class ValueToBadgedBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int givenValue = (int)value;
            if (givenValue > 0)
                if ((string)parameter == "Foreground")
                    return Brushes.White;
                else
                    return Brushes.Red;
            else
                return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
