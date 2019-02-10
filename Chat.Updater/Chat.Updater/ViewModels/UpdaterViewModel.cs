using Chat.Updater.Annotations;
using Chat.Updater.ArgumentTool;
using Chat.Updater.Extensions;
using FluentFTP;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace Chat.Updater.ViewModels
{
    public class UpdaterViewModel : INotifyPropertyChanged
    {
        private readonly UpdaterArguments _updaterArguments;

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

        public UpdaterViewModel(UpdaterArguments arguments)
        {
            _updaterArguments = arguments;
        }

        public async void Update()
        {
            StatusString = "Searching for update...";

            Version currentAssemblyVersion;

            if (File.Exists(_updaterArguments.GetAssemblyPath))
            {
                var currentAssemblyName = AssemblyName.GetAssemblyName(_updaterArguments.GetAssemblyPath);
                currentAssemblyVersion = currentAssemblyName.Version;
            }
            else
                currentAssemblyVersion = Version.Parse("0.0");

            var client = new FtpClient(_updaterArguments.GetHost)
            {
                Credentials = new NetworkCredential(_updaterArguments.GetFtpUser, _updaterArguments.GetFtpPassword)
            };

            client.Connect();

            try
            {
                await Task.Run(() =>
                {
                    var newestFtpItem = client.GetListing("").OrderByDescending(item => item.Modified).FirstOrDefault();
                    if (newestFtpItem == null || !IsNewVersion(newestFtpItem, currentAssemblyVersion))
                        return;

                    Application.Current.Dispatcher.Invoke(() => StatusString = "Update found. Downloading...");

                    using (var fileStream = File.Create($"{Environment.CurrentDirectory}/{newestFtpItem.Name}"))
                    {
                        Stream stream = new MemoryStream();
                        if (!client.Download(stream, $"/{newestFtpItem.Name}")) return;
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fileStream);

                        Application.Current.Dispatcher.Invoke(() => StatusString = "Extracting update...");

                        using (var archive = new ZipArchive(fileStream))
                            archive.ExtractToDirectory(Environment.CurrentDirectory, true);
                    }

                    Application.Current.Dispatcher.Invoke(() => StatusString = "Deleting temporary files...");

                    File.Delete($"{Environment.CurrentDirectory}/{newestFtpItem.Name}");
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    client.Dispose();
                    Application.Current.Shutdown(0);
                    Environment.Exit(0);
                    return StatusString = "Done.";
                });
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
