using System;

namespace CappuChat.DTOs
{
    public class SimpleLoginResponse : BaseResponse
    {
        public SimpleUser User { get; set; }
        public string ConnectionId { get; set; }

        public SimpleLoginResponse()
        {
        }

        public SimpleLoginResponse(bool success, SimpleUser user, string connectionId) : base(success)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Cannot create SimpleLoginResponse. Given user is null.");

            if (string.IsNullOrWhiteSpace(connectionId))
                throw new ArgumentNullException(nameof(connectionId), "Cannot create SimpleLoginResponse. Given connectionId is invalid.");

            User = user;
            ConnectionId = connectionId;
        } 

        public SimpleLoginResponse(bool success, string errorMessage, SimpleUser user, string connectionId) : base(success, errorMessage)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Cannot create SimpleLoginResponse. Given user is null.");

            if (string.IsNullOrWhiteSpace(connectionId))
                throw new ArgumentNullException(nameof(connectionId), "Cannot create SimpleLoginResponse. Given connectionId is invalid.");

            User = user;
            ConnectionId = connectionId;
        }
    }
}
