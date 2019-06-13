using CappuChat.DTOs;
using Chat.Server.Controller;

namespace Chat.Server.Hubs
{
    public class GroupHub : BaseHub
    {
        private static readonly GroupController GroupController = new GroupController();

        public SimpleCreateGroupResponse CreateGroup(string groupName)
        {
            SimpleCreateGroupResponse response = new SimpleCreateGroupResponse();

            if (string.IsNullOrWhiteSpace(groupName))
            {
                response.Success = false;
                response.ErrorMessage = Texts.Texts.InvalidGroupName;
                return response;
            }

            response.Group = ExecuteControllerAction(() => GroupController.CreateGroup(groupName, 1), response);
            return response;
        }
    }
}
