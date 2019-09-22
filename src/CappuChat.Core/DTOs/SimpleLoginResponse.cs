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
            if (string.IsNullOrWhiteSpace(connectionId))
                throw new ArgumentNullException(nameof(connectionId));
            ConnectionId = connectionId;

            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public SimpleLoginResponse(bool success, string errorMessage, SimpleUser user, string connectionId) : base(success, errorMessage)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                throw new ArgumentNullException(nameof(connectionId));
            ConnectionId = connectionId;

            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}
