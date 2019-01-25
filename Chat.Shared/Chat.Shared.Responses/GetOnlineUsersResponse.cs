using Chat.Shared.Models;
using System;
using System.Collections.Generic;

namespace Chat.Responses
{
    public class GetOnlineUsersResponse : BaseResponse
    {
        public IEnumerable<SimpleUser> OnlineUserList { get; set; }

        public GetOnlineUsersResponse()
        {
        }

        public GetOnlineUsersResponse(IEnumerable<SimpleUser> onlineUserList, bool success) : base(success)
        {
            if (onlineUserList == null)
                throw new ArgumentNullException(nameof(onlineUserList), "Cannot create GetOnlineUsersResponse. onlineUserList is null.");
            OnlineUserList = onlineUserList;
        }

        public GetOnlineUsersResponse(IEnumerable<SimpleUser> onlineUserList, bool success, string errorMessage) : base(success, errorMessage)
        {
            if (onlineUserList == null)
                throw new ArgumentNullException(nameof(onlineUserList), "Cannot create GetOnlineUsersResponse. onlineUserList is null.");
            OnlineUserList = onlineUserList;
        }
    }
}
