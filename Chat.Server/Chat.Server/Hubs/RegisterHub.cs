using Chat.Responses;
using Chat.Server.Controller;

namespace Chat.Server.Hubs
{
    public class RegisterHub : BaseHub
    {
        private static readonly UserController UserController = new UserController();

        public SimpleRegisterResponse Register(string username, string password)
        {
            return UserController.CreateSimpleUser(username, password);
        }
    }
}
