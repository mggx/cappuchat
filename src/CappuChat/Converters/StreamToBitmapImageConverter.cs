using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;

namespace CappuChat.Converters
{
    public class StreamToBitmapImageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return null;

            if (values[0] is Image image)
            {
                if (values[1] is MemoryStream memoryStream)
                {
                    return memoryStream.ToBitmapImage(image);
                }
            }

            return Stream.Null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
