using Chat.Server.DataAccess;
using Chat.Shared.Models;

namespace Chat.Server.Controller
{
    public class UserController
    {
        private readonly UserRepository _userRepository;

        public UserController()
        {
            _userRepository = new UserRepository();
        }

        public SimpleUser CreateSimpleUser(string username, string password)
        {
            return _userRepository.CreateSimpleUser(username, password);
        }

        public SimpleUser Login(string username, string password)
        {
            return _userRepository.Login(username, password);
        }
    }
}
