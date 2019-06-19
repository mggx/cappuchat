using CappuChat;
using Chat.Server.DataAccess;
using System;
using System.Collections.Generic;

namespace Chat.Server.Controller
{
    public static class GroupController
    {
        public static SimpleGroup CreateGroup(string groupName, int ownerId)
        {
            string uniqueName = Guid.NewGuid().ToString();
            return GroupRepository.CreateGroup(uniqueName, groupName, ownerId);
        }

        public static void AddUserToGroup(int groupId, int userId)
        {
            GroupRepository.AddUserToGroup(groupId, userId);
        }

        public static void DeleteUserFromGroup(int groupId, int userId)
        {
            GroupRepository.DeleteUserFromGroup(groupId, userId);
        }

        public static IEnumerable<SimpleGroup> GetGroupsOfUser(string username)
        {
            return GroupRepository.GetGroupsOfUser(username);
        }
    }
}
