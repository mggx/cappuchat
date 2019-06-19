using CappuChat;
using Chat.Server.DataAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;

namespace Chat.Server.DataAccess
{
    public static class UserRepository
    {
        public static SimpleUser Login(string username, string password)
        {
            IDbCommand dbCommand = DatabaseClient.GetDbCommand();

            dbCommand.CommandText = $"SELECT username FROM users WHERE username = @username COLLATE NOCASE AND password = @password";

            dbCommand.Parameters.Add(new SQLiteParameter("@username", username));
            dbCommand.Parameters.Add(new SQLiteParameter("@password", password));

            IDataReader reader = dbCommand.ExecuteReader();

            SimpleUser user = null;
            while (reader.Read())
            {
                string retrievedUsername = reader["username"] as string;
                if (string.IsNullOrWhiteSpace(retrievedUsername))
                   throw new UserNotFoundException("Username and password combination was not found.");
                user = new SimpleUser(retrievedUsername);
            }

            if (user == null)
                throw new UserNotFoundException("Username and password combination was not found.");

            SetUserOnline(username, true);
            return user;
        }

        public static void SetUserOnline(string username, bool online)
        {
            IDbCommand dbCommand = DatabaseClient.GetDbCommand();

            dbCommand.Parameters.Add(new SQLiteParameter("@username", username));
            dbCommand.Parameters.Add(new SQLiteParameter("@online", online));

            dbCommand.CommandText = "UPDATE users SET online = @online WHERE username = @username";
            dbCommand.ExecuteNonQuery();
        }

        public static SimpleUser CreateSimpleUser(string username, string password)
        {
            IDbCommand dbCommand = DatabaseClient.GetDbCommand();

            dbCommand.Parameters.Add(new SQLiteParameter("@username", username));
            dbCommand.Parameters.Add(new SQLiteParameter("@password", password));

            if (GetUserByUsername(username) != null)
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, CappuService.Properties.Strings.FailedToCreate_Reason,CappuService.Properties.Strings.UserAlreadyExists)
                );

            dbCommand.CommandText = "INSERT INTO users (username, password, online) values (@username, @password, 0)";

            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (InvalidOperationException connectionException)
            {
                throw new InvalidOperationException( 
                    string.Format(CultureInfo.CurrentCulture, CappuService.Properties.Strings.FailedToCreate_Reason, connectionException.Message),
                    connectionException
                );
            }

            return new SimpleUser(username);
        }

        public static IEnumerable<SimpleUser> GetAllUsers()
        {
            IDbCommand dbCommand = DatabaseClient.GetDbCommand();

            dbCommand.CommandText = "SELECT username FROM users";
            IDataReader reader = dbCommand.ExecuteReader();

            IList<SimpleUser> userList = new List<SimpleUser>();
            while (reader.Read())
            {
                userList.Add(new SimpleUser(reader["username"] as string));
            }

            return userList;
        }

        public static SimpleUser GetUserByUsername(string username)
        {
            IDbCommand dbCommand = DatabaseClient.GetDbCommand();

            dbCommand.Parameters.Add(new SQLiteParameter(nameof(username), username));

            dbCommand.CommandText = "SELECT username from users WHERE username = #username LIMIT 1";
            IDataReader reader = dbCommand.ExecuteReader();

            while (reader.Read())
            {
                return new SimpleUser(reader["username"] as string);
            }

            return null;
        }
    }
}
