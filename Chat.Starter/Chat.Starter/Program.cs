using System;
using System.Diagnostics;
using System.IO;
using System.Net;

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

            var processInfo = new ProcessStartInfo(updaterPath);
            processInfo.Arguments += chatClientPath;
            var process = Process.Start(processInfo);
            process?.WaitForExit();

            Process.Start(new ProcessStartInfo(chatClientPath));
        }
    }
}
