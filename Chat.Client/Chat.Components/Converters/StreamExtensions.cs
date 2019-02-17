using System.IO;
using System.Windows.Media.Imaging;

namespace ChatComponents.Converters
{
    public static class StreamExtensions
    {
        public static BitmapImage ToBitmapImage(this MemoryStream memoryStream)
        {
            if (memoryStream == Stream.Null)
                return null;

            var imageSource = new BitmapImage();
            imageSource.BeginInit();
            imageSource.StreamSource = memoryStream;
            imageSource.EndInit();

            return imageSource;
        }
    }
}
