using Chat.Server.DataAccess.Exceptions;
using Chat.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Chat.Server.DataAccess
{
    public class GroupRepository
    {
        public SimpleGroup CreateGroup(string uniqueName, string groupName, int ownerId)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();
            dbCommand.Parameters.Add(new SQLiteParameter("@uniquename", uniqueName));
            dbCommand.Parameters.Add(new SQLiteParameter("@name", groupName));
            dbCommand.CommandText = $"INSERT INTO groups (uniquename, name, ownerId) values (@uniquename, @name, {ownerId})";
            ExecuteNonQuery(dbCommand, Texts.Texts.CreatingGroupFailed);
            return new SimpleGroup(uniqueName, groupName);
        }

        public void AddUserToGroup(int groupId, int userId)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();
            dbCommand.CommandText = $"INSERT INTO usergroups (groupid, userid) values ({groupId}, {userId})";
            ExecuteNonQuery(dbCommand, Texts.Texts.AddingUserToGroupFailed);
        }

        public void DeleteUserFromGroup(int groupId, int userId)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();
            dbCommand.CommandText = $"DELETE FROM usergroups WHERE groupid = {groupId} AND userid = {userId}";
            ExecuteNonQuery(dbCommand, Texts.Texts.DeletingUserFromGroupFailed);
        }

        private void ExecuteNonQuery(IDbCommand dbCommand, Func<string, string> errorAction)
        {
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (InvalidOperationException e)
            {
                throw new GroupException(errorAction(e.Message));
            }

            throw new Exception();
        }

        public IEnumerable<SimpleGroup> GetGroupsOfUser(string username)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();

            dbCommand.CommandText =
                "SELECT groups.uniquename, groups.name FROM groups INNER JOIN usergroups ON groups.id = usergroups.groupid";
            IDataReader reader = dbCommand.ExecuteReader();

            List<SimpleGroup> groups = new List<SimpleGroup>();

            while (reader.Read())
            {
                groups.Add(new SimpleGroup(reader["uniquename"] as string, reader["name"] as string));
            }

            return groups;
        }
    }
}
