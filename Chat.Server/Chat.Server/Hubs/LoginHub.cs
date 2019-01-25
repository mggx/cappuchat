using Chat.Responses;
using Chat.Server.Controller;
using Chat.Server.DataAccess.Exceptions;
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
            SimpleLoginResponse response = new SimpleLoginResponse();

            response.User = ExecuteControllerAction(() => UserController.Login(username, password), response);
            if (response.User == null)
                return response;

            response.ConnectionId = Context.ConnectionId;

            if (UsernameConnectionIdCache.ContainsKey(username))
            {
                if (UsernameConnectionIdCache[username] != Context.ConnectionId)
                    Clients.Client(UsernameConnectionIdCache[username]).OnClientLoggedOut(Texts.Texts.OtherClientLoggedIn);
            }

            Add(username);

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
                    UsernameConnectionIdCache.Remove(username);
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
