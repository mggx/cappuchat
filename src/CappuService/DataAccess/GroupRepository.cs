using CappuChat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Chat.Server.DataAccess
{
    public static class GroupRepository
    {
        public static SimpleGroup CreateGroup(string uniqueName, string groupName, int ownerId)
        {
            IDbCommand dbCommand = DatabaseClient.GetDbCommand();
            dbCommand.Parameters.Add(new SQLiteParameter("@uniquename", uniqueName));
            dbCommand.Parameters.Add(new SQLiteParameter("@name", groupName));
            dbCommand.CommandText = $"INSERT INTO groups (uniquename, name, ownerId) values (@uniquename, @name, {ownerId})";
            ExecuteNonQuery(dbCommand, "Creating group failed.");
            return new SimpleGroup(uniqueName, groupName);
        }

        public static void AddUserToGroup(int groupId, int userId)
        {
            IDbCommand dbCommand = DatabaseClient.GetDbCommand();
            dbCommand.CommandText = $"INSERT INTO usergroups (groupid, userid) values ({groupId}, {userId})";
            ExecuteNonQuery(dbCommand, "Adding user to group failed.");
        }

        public static void DeleteUserFromGroup(int groupId, int userId)
        {
            IDbCommand dbCommand = DatabaseClient.GetDbCommand();
            dbCommand.CommandText = $"DELETE FROM usergroups WHERE groupid = {groupId} AND userid = {userId}";
            ExecuteNonQuery(dbCommand, "Deleting user from group failed.");
        }

        private static void ExecuteNonQuery(IDbCommand dbCommand, string onErrorMessage)
        {
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (InvalidOperationException e)
            {
                throw new AggregateException(onErrorMessage, e);
            }

            throw new Exception();
        }

        public static IEnumerable<SimpleGroup> GetGroupsOfUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            IDbCommand dbCommand = DatabaseClient.GetDbCommand();

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
