using CappuChat;
using Chat.Server.DataAccess;
using System;
using System.Collections.Generic;

namespace Chat.Server.Controller
{
    public class GroupController
    {
        private readonly GroupRepository _groupRepository;

        public GroupController()
        {
            _groupRepository = new GroupRepository();
        }

        public SimpleGroup CreateGroup(string groupName, int ownerId)
        {
            string uniqueName = Guid.NewGuid().ToString();
            return _groupRepository.CreateGroup(uniqueName, groupName, ownerId);
        }

        public void AddUserToGroup(int groupId, int userId)
        {
            _groupRepository.AddUserToGroup(groupId, userId);
        }

        public void DeleteUserFromGroup(int groupId, int userId)
        {
            _groupRepository.DeleteUserFromGroup(groupId, userId);
        }

        public IEnumerable<SimpleGroup> GetGroupsOfUser(string username)
        {
            return _groupRepository.GetGroupsOfUser(username);
        }
    }
}
