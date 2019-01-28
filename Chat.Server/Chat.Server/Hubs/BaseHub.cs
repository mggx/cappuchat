using Chat.Shared.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Responses;

namespace Chat.Server.Hubs
{
    public class BaseHub : Hub
    {
        protected static readonly Dictionary<string, string> UsernameConnectionIdCache = new Dictionary<string, string>();
        protected static readonly IList<SimpleUser> OnlineUsers = new List<SimpleUser>();

        protected bool Add(string username)
        {
            var lowerUsername = username.ToLower();
            string connectionId = string.Empty;
            if (UsernameConnectionIdCache.TryGetValue(lowerUsername, out connectionId))
            {
                UsernameConnectionIdCache.Remove(lowerUsername);
                UsernameConnectionIdCache.Add(lowerUsername, Context.ConnectionId);
                OnlineUsers.Add(new SimpleUser(username));
                return false;
            }

            UsernameConnectionIdCache.Add(lowerUsername, Context.ConnectionId);
            OnlineUsers.Add(new SimpleUser(username));
            return true;
        }

        protected bool Remove(string username)
        {
            var lowerUsername = username.ToLower();
            var simpleUser = OnlineUsers.FirstOrDefault(user =>
                user.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase));
            OnlineUsers.Remove(simpleUser);
            return UsernameConnectionIdCache.Remove(lowerUsername);
        }

        protected string GetUsernameByConnectionId(string connectionId)
        {
            foreach (var pair in UsernameConnectionIdCache)
            {
                if (pair.Value == connectionId)
                    return GetCorrectUsername(pair.Key);
            }
            return string.Empty;
        }

        protected IList<SimpleUser> GetOnlineUsers()
        {
            return OnlineUsers;
        }

        protected string GetCorrectUsername(string username)
        {
            var simpleUser = OnlineUsers.FirstOrDefault(user => user.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase));
            return simpleUser == null ? string.Empty : simpleUser.Username;
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
