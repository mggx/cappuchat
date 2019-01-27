using System;
using System.Globalization;
using System.Windows.Data;

namespace Chat.Client.Converters
{
    public class BooleanToVotedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolean)) return Texts.Texts.SomethingWentWrong;
            if (boolean)
                return Texts.Texts.Voted;
            return Texts.Texts.NotVoted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
