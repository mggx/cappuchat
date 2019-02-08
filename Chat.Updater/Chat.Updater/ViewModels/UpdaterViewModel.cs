using Chat.Updater.Annotations;
using Chat.Updater.Extensions;
using FluentFTP;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

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

        private string _statusString;
        public string StatusString
        {
            get { return _statusString; }
            set { _statusString = value; OnPropertyChanged(); }
        }

        public UpdaterViewModel(string assemblyPath)
        {
            Update(assemblyPath);
        }

        private async void Update(string assemblyPath)
        {
            StatusString = "Searching for update...";

            Version currentAssemblyVersion;

            if (File.Exists(assemblyPath))
            {
                var currentAssemblyName = AssemblyName.GetAssemblyName(assemblyPath);
                currentAssemblyVersion = currentAssemblyName.Version;
            }
            else
                currentAssemblyVersion = Version.Parse("0.0");

            var client = new FtpClient("localhost")
            {
                Credentials = new NetworkCredential("asd", "asd")
            };

            client.Connect();

            try
            {
                await Task.Run(() =>
                {
                    foreach (FtpListItem item in client.GetListing(""))
                    {
                        if (!IsNewVersion(item, currentAssemblyVersion))
                            continue;

                        Application.Current.Dispatcher.Invoke(() => StatusString = "Update found. Downloading...");

                        using (var fileStream = File.Create($"{Environment.CurrentDirectory}/{item.Name}"))
                        {
                            Stream stream = new MemoryStream();
                            if (!client.Download(stream, $"/{item.Name}")) continue;
                            stream.Seek(0, SeekOrigin.Begin);
                            stream.CopyTo(fileStream);

                            Application.Current.Dispatcher.Invoke(() => StatusString = "Extracting update...");

                            using (var archive = new ZipArchive(fileStream))
                                archive.ExtractToDirectory(Environment.CurrentDirectory, true);
                        }

                        Application.Current.Dispatcher.Invoke(() => StatusString = "Deleting temporary files...");

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
                Application.Current.Dispatcher.Invoke(() => StatusString = "Done.");
                client.Dispose();
                Application.Current.Shutdown(0);
            }
        }

        private bool IsNewVersion(FtpListItem item, Version currentVersion)
        {
            if (item.Type != FtpFileSystemObjectType.File || currentVersion >= GetVersionFromZipName(item.Name))
                return false;
            return true;
        }

        private Version GetVersionFromZipName(string zipName)
        {
            var versionString = zipName.Remove(zipName.LastIndexOf(".", StringComparison.Ordinal));
            return Version.TryParse(versionString, out Version version) ? version : Version.Parse("1.0");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
