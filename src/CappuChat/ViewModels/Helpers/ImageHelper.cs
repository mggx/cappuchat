using Chat.Client.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Chat.Client.ViewModels.Helpers
{
    public class ImageHelper : Disposable
    {
        private static readonly Dictionary<string, MemoryStream> Images = new Dictionary<string, MemoryStream>();

        private static ImageHelper _instance;
        public static ImageHelper Instance => _instance ?? (_instance = new ImageHelper());

        private readonly string _imagesFolderPath = $@"{Environment.CurrentDirectory}\Images\";

        public ImageHelper()
        {
            if (!Directory.Exists(_imagesFolderPath))
                Directory.CreateDirectory(_imagesFolderPath);
        }

        public void AddImageStream(MemoryStream stream, string imageName)
        {
            if (Images.ContainsKey(imageName))
                return;

            Images.Add(imageName, stream);

            if (!File.Exists($@"{_imagesFolderPath}\{imageName}"))
            {
                using (var fileStream = File.Create($@"{_imagesFolderPath}\{imageName}"))
                {
                    stream.WriteTo(fileStream);
                }
            }
        }

        public Stream GetImageStream(string imageName)
        {
            return Images.ContainsKey(imageName) ? Images[imageName] : Stream.Null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var keyValuePair in Images)
                {
                    keyValuePair.Value.Dispose();
                    var filePath = $@"{_imagesFolderPath}\{keyValuePair.Key}";
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
            }

            base.Dispose(disposing);
        }
    }
}
