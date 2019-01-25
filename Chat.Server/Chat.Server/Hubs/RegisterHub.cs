using Chat.Responses;
using Chat.Server.Controller;

namespace Chat.Server.Hubs
{
    public class RegisterHub : BaseHub
    {
        private static readonly UserController UserController = new UserController();

        public SimpleRegisterResponse Register(string username, string password)
        {
            SimpleRegisterResponse response = new SimpleRegisterResponse();

            if (string.IsNullOrWhiteSpace(username))
            {
                response.Success = false;
                response.ErrorMessage = Texts.Texts.InvalidUsername;
                return response;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                response.Success = false;
                response.ErrorMessage = Texts.Texts.InvalidPassword;
                return response;
            }

            response.User = ExecuteControllerAction(() => UserController.CreateSimpleUser(username, password), response);
            return response;
        }
    }
}
