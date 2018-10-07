using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using Chat.Server;
using Microsoft.Owin;

namespace Chat.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string url = "http://localhost:1232/";

            DataAccess.DataAccess.InitializeDatabase();

            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}