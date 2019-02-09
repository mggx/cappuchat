using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Chat.Configurations;
using Chat.Configurations.Models;

namespace Chat.Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var proc in Process.GetProcessesByName("Chat.Client"))
            {
                proc.Kill();
            }

            var updaterPath = $"{Environment.CurrentDirectory}\\Chat.Updater.exe";
            var chatClientPath = $"{Environment.CurrentDirectory}\\Chat.Client.exe";

            if (!File.Exists(updaterPath))
                return;

            var serverConfigurationController = new ConfigurationController<ServerConfiguration>();
            var serverConfiguration = serverConfigurationController.ReadConfiguration(new ServerConfiguration()
            {
                Host = "localhost",
                FtpUser = "fallback",
                FtpPassword = string.Empty
            });

            var processInfo = new ProcessStartInfo(updaterPath);
            processInfo.Arguments += $"-assemblyPath={chatClientPath} ";
            processInfo.Arguments += $"-host={serverConfiguration.Host} ";
            processInfo.Arguments += $"-ftpuser={serverConfiguration.FtpUser} ";
            processInfo.Arguments += $"-ftppassword={serverConfiguration.FtpPassword} ";

            var process = Process.Start(processInfo);
            process?.WaitForExit();

            Process.Start(new ProcessStartInfo(chatClientPath));
        }
    }
}
