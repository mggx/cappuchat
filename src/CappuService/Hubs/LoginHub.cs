using CappuChat.DTOs;
using Chat.Server.Controller;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Server.Hubs
{
    public class LoginHub : BaseHub
    {
        private static readonly UserController UserController = new UserController();

        public SimpleLoginResponse Login(string username, string password)
        {
            username = username?.Trim();

            SimpleLoginResponse response = new SimpleLoginResponse();

            response.User = ExecuteControllerAction(() => UserController.Login(username, password), response);
            if (response.User == null)
                return response;

            response.ConnectionId = Context.ConnectionId;

            if (UsernameConnectionIdCache.ContainsKey(response.User.Username.ToLower()))
            {
                if (UsernameConnectionIdCache[response.User.Username.ToLower()] != Context.ConnectionId)
                    Clients.Client(UsernameConnectionIdCache[response.User.Username.ToLower()]).OnClientLoggedOut(Texts.Texts.OtherClientLoggedIn);
            }

            Add(response.User.Username);

            Clients.All.OnOnlineUsersChanged(GetOnlineUsers());

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
                    Remove(username);
                    VoteHub.ActiveCappuVote?.UserAnswerCache.Remove(username);
                }
            }

            Clients.All.OnOnlineUsersChanged(GetOnlineUsers());

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
