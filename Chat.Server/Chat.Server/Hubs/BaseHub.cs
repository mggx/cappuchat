using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Chat.Server.Hubs
{
    public class BaseHub : Hub
    {
        protected static readonly Dictionary<string, string> UsernameConnectionIdCache = new Dictionary<string, string>();

        protected bool Add(string username)
        {
            string connectionId = string.Empty;
            if (UsernameConnectionIdCache.TryGetValue(username, out connectionId))
            {
                UsernameConnectionIdCache.Remove(username);
                UsernameConnectionIdCache.Add(username, Context.ConnectionId);
                return false;
            }

            UsernameConnectionIdCache.Add(username, Context.ConnectionId);
            return true;
        }

        protected bool Remove(string username)
        {
            return UsernameConnectionIdCache.Remove(username);
        }

        protected string GetUsernameByConnectionId(string connectionId)
        {
            foreach (var pair in UsernameConnectionIdCache)
            {
                if (pair.Value == connectionId)
                    return pair.Key;
            }

            return string.Empty;
        }

        public override Task OnConnected()
        {
            Console.WriteLine($"OnConnected called. ConnectionId: {Context.ConnectionId}");
            return base.OnConnected();
        }
    }
}
