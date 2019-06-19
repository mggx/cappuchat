using CappuChat.DTOs;
using Chat.Server.Controller;

namespace Chat.Server.Hubs
{
    public class GroupHub : BaseHub
    {
        public static SimpleCreateGroupResponse CreateGroup(string groupName)
        {
            SimpleCreateGroupResponse response = new SimpleCreateGroupResponse();

            if (string.IsNullOrWhiteSpace(groupName))
            {
                response.Success = false;
                response.ErrorMessage = "Invalid Group Name";
                return response;
            }

            response.Group = ExecuteControllerAction(() => GroupController.CreateGroup(groupName, 1), response);
            return response;
        }
    }
}
