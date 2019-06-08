using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace CappuChat.Converters
{
    public static class StreamExtensions
    {
        public static BitmapImage ToBitmapImage(this MemoryStream memoryStream, FrameworkElement element = null)
        {
            if (memoryStream == Stream.Null)
                return new BitmapImage();

            memoryStream.Seek(0, SeekOrigin.Begin);

            var bitmap = BitmapFrame.Create(
                memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            memoryStream.Seek(0, SeekOrigin.Begin);

            if (element is Image image)
                ImageBehavior.SetAnimatedSource(image, bitmap);

            return new BitmapImage();
        }
    }
}
