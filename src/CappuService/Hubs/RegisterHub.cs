using CappuChat.DTOs;
using Chat.Server.Controller;
using System;

namespace Chat.Server.Hubs
{
    public class RegisterHub : BaseHub
    {
        public static SimpleRegisterResponse Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException(CappuService.Properties.Strings.EmptyStringNotAllowed, nameof(username));

            username = username.Trim();

            SimpleRegisterResponse response = new SimpleRegisterResponse();

            if (string.IsNullOrWhiteSpace(username))
            {
                response.Success = false;
                response.ErrorMessage = "The given username is invalid";
                return response;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                response.Success = false;
                response.ErrorMessage = "The given password was invalid";
                return response;
            }

            response.User = ExecuteControllerAction(() => UserController.CreateSimpleUser(username, password), response);
            return response;
        }
    }
}
