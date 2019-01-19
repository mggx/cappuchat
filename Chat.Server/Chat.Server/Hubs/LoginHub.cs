using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Responses;
using Chat.Server.Controller;
using Chat.Shared.Models;
using Chat.Server.Hubs;
using Microsoft.AspNet.SignalR;

namespace Chat.Server.Hubs
{
    public class LoginHub : BaseHub
    {
        private static readonly UserController UserController = new UserController();

        public SimpleLoginResponse Login(string username, string password)
        {
            SimpleLoginResponse response = UserController.Login(username, password);
            if (!response.Success)
                return response;

            response.ConnectionId = Context.ConnectionId;

            if (UsernameConnectionIdCache.ContainsKey(username))
            {
                if (UsernameConnectionIdCache[username] != Context.ConnectionId)
                    Clients.Client(UsernameConnectionIdCache[username]).OnClientLoggedOut(Texts.Texts.OtherClientLoggedIn);
            }

            Add(username);

            IList<SimpleUser> onlineUsers = new List<SimpleUser>();
            foreach (var pair in UsernameConnectionIdCache)
            {
                onlineUsers.Add(new SimpleUser(pair.Key));
            }

            Clients.All.OnOnlineUsersChanged(onlineUsers);

            return response;
        }

        public void Logout()
        {
            RemoveClientFromCache();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            RemoveClientFromCache();
            return base.OnDisconnected(stopCalled);
        }

        private void RemoveClientFromCache()
        {
            var userId = Context.ConnectionId;
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var username = UsernameConnectionIdCache.FirstOrDefault(pair => pair.Value == userId).Key;
                if (username != null)
                {
                    UsernameConnectionIdCache.Remove(username);
                    VoteHub.ActiveCappuVote?.UserAnswerCache.Remove(username);
                }
            }

            IList<SimpleUser> onlineUsers = new List<SimpleUser>();
            foreach (var pair in UsernameConnectionIdCache)
            {
                onlineUsers.Add(new SimpleUser(pair.Key));
            }

            Clients.All.OnOnlineUsersChanged(onlineUsers);

            Console.WriteLine($"User with userId {userId} removed from usernames. ConnectionId: {Context.ConnectionId}");
        }

        public override Task OnReconnected()
        {
            string userId = Context.Headers.Get("userId");
            if (string.IsNullOrWhiteSpace(userId))
                return base.OnReconnected();

            var username = UsernameConnectionIdCache.FirstOrDefault(pair => pair.Value == userId).Key;
            if (username == null)
                return base.OnReconnected();

            Add(username);

            Clients.Caller.OnHeaderChanged(Context.ConnectionId);

            return base.OnReconnected();
        }
    }
}
