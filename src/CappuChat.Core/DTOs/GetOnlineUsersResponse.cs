using System;
using System.Collections.Generic;

namespace CappuChat.DTOs
{
    public class GetOnlineUsersResponse : BaseResponse
    {
        public IEnumerable<SimpleUser> OnlineUserList { get; set; }

        public GetOnlineUsersResponse()
        {
        }

        public GetOnlineUsersResponse(IEnumerable<SimpleUser> onlineUserList, bool success) : base(success)
        {
            OnlineUserList = onlineUserList ?? throw new ArgumentNullException(nameof(onlineUserList));
        }
    }
}
