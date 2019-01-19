using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Chat.Shared.Models;

namespace Chat.Client.Converters
{
    public class UsersToHorizontalAlignmentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return HorizontalAlignment.Center;

            if (!(values[0] is SimpleUser sender))
                return HorizontalAlignment.Center;

            if (!(values[1] is SimpleUser user))
                return HorizontalAlignment.Center;

            return sender.Username.Equals(user.Username) ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
