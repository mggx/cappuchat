using Chat.Server.Controller;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using Chat.Server.Hubs;
using Microsoft.AspNet.SignalR;

namespace Chat.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Use localhost as url if -debug is in args

            string url = string.Empty;
            if (args.Length == 1)
            {
                if (args[0] == "-debug")
                    url = "http://localhost:1232";
            }
            else
                url = "http://*:5555";
                
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

            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = 3000000;
            app.MapSignalR(hubConfiguration);
        }
    }
}