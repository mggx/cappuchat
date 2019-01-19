using System.Collections.Generic;
using Chat.Shared.Models;

namespace Chat.Responses
{
    public class SimpleGetVoteScopeMessagesResponse : BaseResponse
    {
        public IList<SimpleMessage> VoteScopeMessages { get; set; } 
    }
}
