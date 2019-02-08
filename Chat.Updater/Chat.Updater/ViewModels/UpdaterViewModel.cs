using Chat.Updater.Annotations;
using FluentFTP;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Chat.Updater.Extensions;

namespace Chat.Updater.ViewModels
{
    public class UpdaterViewModel : INotifyPropertyChanged
    {
        private bool _isUpdating = true;
        public bool IsUpdating
        {
            get { return _isUpdating; }
            set { _isUpdating = value; OnPropertyChanged(); }
        }

        public UpdaterViewModel(string assemblyPath)
        {
            Update(assemblyPath);
        }

        private async void Update(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
                throw new ArgumentException($"Could not find file {assemblyPath}");

            IsUpdating = true;

            try
            {
                await Task.Run(() =>
                {
                    FtpClient client = new FtpClient("localhost");
                    client.Credentials = new NetworkCredential("asd", "asd");
                    client.Connect();

                    FileInfo fileInfo = new FileInfo(assemblyPath);
                    DateTime lastModified = fileInfo.LastWriteTime;

                    foreach (FtpListItem item in client.GetListing(""))
                    {
                        if (item.Type != FtpFileSystemObjectType.File) continue;
                        if (lastModified >= item.Modified) continue;

                        using (var fileStream = File.Create($"{Environment.CurrentDirectory}/{item.Name}"))
                        {
                            Stream stream = new MemoryStream();
                            if (!client.Download(stream, $"/{item.Name}")) continue;
                            stream.Seek(0, SeekOrigin.Begin);
                            stream.CopyTo(fileStream);

                            using (ZipArchive archive = new ZipArchive(fileStream))
                            {
                                archive.ExtractToDirectory(Environment.CurrentDirectory, true);
                            }
                        }

                        File.Delete($"{Environment.CurrentDirectory}/{item.Name}");
                        break;
                    }
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                IsUpdating = false;
                Application.Current.Shutdown(0);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
