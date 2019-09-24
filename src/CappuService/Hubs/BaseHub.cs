using CappuChat;
using CappuChat.DTOs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Server.Hubs
{
    public class BaseHub : Hub
    {
        protected static readonly Dictionary<string, string> UsernameConnectionIdCache = new Dictionary<string, string>();
        protected static readonly IList<SimpleUser> OnlineUsers = new List<SimpleUser>();

        protected bool Add(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Cannot add user with an empty name.");
            var normalizedUserName = NormalizeUsernameForCache(username);
            if (TryGetUserIDFromCache(normalizedUserName, out _))
            {
                UsernameConnectionIdCache.Remove(normalizedUserName);
                UsernameConnectionIdCache.Add(normalizedUserName, Context.ConnectionId);
                OnlineUsers.Remove(GetSimpleUser(username));
                OnlineUsers.Add(new SimpleUser(username) { IsActive = true });
                return false;
            }

            UsernameConnectionIdCache.Add(normalizedUserName, Context.ConnectionId);
            OnlineUsers.Add(new SimpleUser(username) { IsActive = true });
            return true;
        }

        protected void ChangeUserStatus(string username, bool status)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Cannot add user with an empty name.");

            OnlineUsers.FirstOrDefault(x => x.Username.ToLower() == username.ToLower()).IsActive = status;
        }

        protected static bool TryGetUserIDFromCache(string username, out string id)
        {
            return UsernameConnectionIdCache.TryGetValue(NormalizeUsernameForCache(username), out id);
        }

        protected static string NormalizeUsernameForCache(string username)
        {
            return username.ToUpperInvariant();
        }

        protected static bool Remove(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Cannot remove user with an empty name.");

            var normalizeUsername = NormalizeUsernameForCache(username);
            var simpleUser = GetSimpleUser(username);
            OnlineUsers.Remove(simpleUser);
            return UsernameConnectionIdCache.Remove(normalizeUsername);
        }

        protected static string GetUsernameByConnectionId(string connectionId)
        {
            foreach (var pair in UsernameConnectionIdCache)
            {
                if (pair.Value == connectionId)
                    return GetCorrectUsername(pair.Key);
            }
            return string.Empty;
        }

        protected static IList<SimpleUser> GetOnlineUserList()
        {
            return OnlineUsers;
        }

        protected static string GetCorrectUsername(string username)
        {
            var simpleUser = GetSimpleUser(username);
            return simpleUser == null ? string.Empty : simpleUser.Username;
        }

        protected static SimpleUser GetSimpleUser(string username)
        {
            return OnlineUsers.FirstOrDefault(user => user.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "It is very much intended to catch any and all exceptions.")]
        protected static T ExecuteControllerAction<T, T1>(Func<T> controllerAction, T1 response) where T1 : BaseResponse
        {
            if (controllerAction == null)
                throw new ArgumentNullException(nameof(controllerAction));

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
