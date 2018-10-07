using System;
using System.Collections.Generic;
using Chat.DataAccess;
using Chat.Shared.Models;

namespace Chat.Client.ViewModels.Controllers
{
    public class CappuMessageController
    {
        private readonly ChatRepository _chatRepository;
        private readonly SimpleUser _user;

        public CappuMessageController(SimpleUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Cannot create CappuMessageController. Given user is null.");
            _user = user;
            _chatRepository = new ChatRepository();
        }

        public IEnumerable<SimpleMessage> GetConversation(SimpleUser user, SimpleUser targetUser)
        {
            return _chatRepository.GetConversationByUsernames(user.Username, targetUser.Username);
        }

        public void StoreOwnMessage(SimpleMessage message)
        {
            message.IsLocalMessage = true;
            StoreMessage(message);
        }

        public void StoreMessage(SimpleMessage message)
        {
            _chatRepository.InsertMessage(message);
        }
    }
}
