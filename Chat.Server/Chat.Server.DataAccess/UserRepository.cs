using System;
using Chat.Shared.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Chat.Server.DataAccess.Exceptions;

namespace Chat.Server.DataAccess
{
    public class UserRepository
    {
        public UserRepository()
        {
        }

        public SimpleUser Login(string username, string password)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();

            dbCommand.CommandText = $"SELECT username FROM users WHERE username = @username AND password = @password";

            dbCommand.Parameters.Add(new SQLiteParameter("@username", username));
            dbCommand.Parameters.Add(new SQLiteParameter("@password", password));

            IDataReader reader = dbCommand.ExecuteReader();

            SimpleUser user = null;
            while (reader.Read())
            {
                string retrievedUsername = reader["username"] as string;
                if (string.IsNullOrWhiteSpace(retrievedUsername))
                   throw new UserNotFoundException(Texts.Texts.UsernamePasswordCombinationNotFound);
                user = new SimpleUser(retrievedUsername);
            }

            if (user == null)
                throw new UserNotFoundException(Texts.Texts.UsernamePasswordCombinationNotFound);

            SetUserOnline(username, true);
            return user;
        }

        public void SetUserOnline(string username, bool online)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();

            dbCommand.Parameters.Add(new SQLiteParameter("@username", username));
            dbCommand.Parameters.Add(new SQLiteParameter("@online", online));

            dbCommand.CommandText = "UPDATE users SET online = @online WHERE username = @username";
            dbCommand.ExecuteNonQuery();
        }

        public void CreateSimpleUser(string username, string password)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();

            dbCommand.Parameters.Add(new SQLiteParameter("@username", username));
            dbCommand.Parameters.Add(new SQLiteParameter("@password", password));

            if (GetUserByUsername(username) != null)
                throw new UserCreationFailedException(Texts.Texts.CreatingUserFailed(Texts.Texts.UserAlreadyExist));

            dbCommand.CommandText = "INSERT INTO users (username, password, online) values (@username, @password, 0)";

            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (InvalidOperationException e)
            {
                throw new UserCreationFailedException(Texts.Texts.CreatingUserFailed(e.Message));
            }
        }

        public IEnumerable<SimpleUser> GetAllUsers()
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();

            dbCommand.CommandText = "SELECT username FROM users";
            IDataReader reader = dbCommand.ExecuteReader();

            IList<SimpleUser> userList = new List<SimpleUser>();
            while (reader.Read())
            {
                userList.Add(new SimpleUser(reader["username"] as string));
            }

            return userList;
        }

        public SimpleUser GetUserByUsername(string username)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();

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
