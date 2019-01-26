using Chat.Shared.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Responses;

namespace Chat.Server.Hubs
{
    public class BaseHub : Hub
    {
        protected static readonly Dictionary<string, string> UsernameConnectionIdCache = new Dictionary<string, string>();

        protected bool Add(string username)
        {
            username = username.ToLower();
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
            username = username.ToLower();
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

        protected IList<SimpleUser> GetOnlineUsers()
        {
            IList<SimpleUser> onlineUsers = new List<SimpleUser>();
            foreach (var pair in UsernameConnectionIdCache)
            {
                onlineUsers.Add(new SimpleUser(pair.Key));
            }

            return onlineUsers;
        }

        protected T ExecuteControllerAction<T, T1>(Func<T> controllerAction, T1 response) where T1 : BaseResponse
        {
            try
            {
                return controllerAction.Invoke();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorMessage = e.Message;
            }

            return default(T);
        }

        public override Task OnConnected()
        {
            Console.WriteLine($"OnConnected called. ConnectionId: {Context.ConnectionId}");
            return base.OnConnected();
        }
    }
}
