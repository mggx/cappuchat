using System;
using System.Globalization;
using System.Windows.Data;

namespace CappuChat.Converters
{
    public class BooleanToVotedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolean)) return CappuChat.Properties.Strings.SomethingWentWrong;
            if (boolean)
                return CappuChat.Properties.Strings.Voted;
            return CappuChat.Properties.Strings.NotVoted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
