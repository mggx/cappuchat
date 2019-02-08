using System;
using System.Diagnostics;

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

            var processInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/Chat.Updater.exe", $"{Environment.CurrentDirectory}/Chat.Client.exe");
            var process = Process.Start(processInfo);
            process?.WaitForExit();
            Process.Start(new ProcessStartInfo($"{Environment.CurrentDirectory}/Chat.Client.exe"));
        }
    }
}
