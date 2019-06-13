using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Chat.Client.Styles;

namespace CappuChat.Converters
{
    public class BooleanToOnlineUsernameColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Cannot convert boolean to ConnectionColor. Given value is null.");

            var boolean = (bool) value;
            if (boolean)
                return ProgramColors.AccentLightColor3;
            return Colors.Teal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
