using System.Runtime.CompilerServices;
using Chat.Responses;
using Chat.Server.DataAccess;
using Chat.Server.DataAccess.Exceptions;
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

        public SimpleRegisterResponse CreateSimpleUser(string username, string password)
        {
            SimpleRegisterResponse simpleRegisterResponse = new SimpleRegisterResponse { Success = true };

            try
            {
                _userRepository.CreateSimpleUser(username, password);
            }
            catch (UserCreationFailedException exception)
            {
                simpleRegisterResponse.Success = false;
                simpleRegisterResponse.ErrorMessage = exception.Message;
            }

            simpleRegisterResponse.User = new SimpleUser(username);
            return simpleRegisterResponse;
        }

        public SimpleLoginResponse Login(string username, string password)
        {
            SimpleLoginResponse simpleLoginResponse = new SimpleLoginResponse { Success = true };

            try
            {
                simpleLoginResponse.User = _userRepository.Login(username, password);
            }
            catch (UserNotFoundException exception)
            {
                simpleLoginResponse.Success = false;
                simpleLoginResponse.ErrorMessage = exception.Message;
            }

            return simpleLoginResponse;
        }
    }
}
