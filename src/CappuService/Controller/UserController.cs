using CappuChat;
using Chat.Server.DataAccess;

namespace Chat.Server.Controller
{
    public static class UserController
    {
        public static SimpleUser CreateSimpleUser(string username, string password)
        {
            return UserRepository.CreateSimpleUser(username, password);
        }

        public static SimpleUser Login(string username, string password)
        {
            return UserRepository.Login(username, password);
        }

        public static SimpleUser GetUser(string username)
        {
            return UserRepository.GetUserByUsername(username);
        }
    }
}
