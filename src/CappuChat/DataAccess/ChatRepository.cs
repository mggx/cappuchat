using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using CappuChat;
using Chat.Models;

namespace Chat.DataAccess
{
    public class ChatRepository
    {
        public IEnumerable<SimpleMessage> GetConversationByUsernames(string localusername, string targetusername)
        {
            Int64 conversationId = GetConversationIdByUsernames(localusername, targetusername);
            return GetConversationByConversationId(conversationId);
        }

        private IEnumerable<SimpleMessage> GetConversationByConversationId(Int64 id)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();

            dbCommand.CommandText =
                $"SELECT * FROM messages WHERE conversationid = @conversationid ORDER BY datetime(messagesentdatetime)";

            dbCommand.Parameters.Add(new SQLiteParameter("@conversationid", id));

            IDataReader reader = dbCommand.ExecuteReader();

            IList<SimpleMessage> conversation = new List<SimpleMessage>();
            while (reader.Read())
            {
                string message = (string) reader["message"];
                string username = (string) reader["username"];
                string dateTimeString = (string) reader["messagesentdatetime"];
                DateTime dateTime = DateTime.Parse(dateTimeString);

                SimpleMessage simpleMessage = new SimpleMessage(new SimpleUser(username), new SimpleUser("testempfaengername"), message)
                {
                    MessageSentDateTime = dateTime
                };

                conversation.Add(simpleMessage);
            }

            return conversation;
        }

        private Int64 GetConversationIdByUsernames(string localusername, string targetUsername)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();

            dbCommand.Parameters.Add(new SQLiteParameter("@targetusername", targetUsername));
            dbCommand.Parameters.Add(new SQLiteParameter("@localusername", localusername));

            dbCommand.CommandText = "SELECT id FROM conversations WHERE targetusername = @targetusername AND localusername = @localusername";

            IDataReader dataReader = dbCommand.ExecuteReader();

            Int64 conversationId = 0;

            while (dataReader.Read())
            {
                conversationId = (Int64) dataReader["id"];
            }

            if (conversationId != 0) return conversationId;

            CreateConversation(localusername, targetUsername);
            conversationId = GetConversationIdByUsernames(localusername, targetUsername);

            return conversationId;
        }

        private void CreateConversation(string localusername, string targetusername)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();

            dbCommand.CommandText =
                "INSERT INTO conversations (localusername, targetusername) values (@localusername, @targetusername)";

            dbCommand.Parameters.Add(new SQLiteParameter("@localusername", localusername));
            dbCommand.Parameters.Add(new SQLiteParameter("@targetusername", targetusername));

            dbCommand.ExecuteNonQuery();
        }

        public void InsertMessage(SimpleMessage message)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();

            string username = message.IsLocalMessage ? message.Sender.Username : message.Receiver.Username;
            string targetUsername = message.IsLocalMessage ? message.Receiver.Username : message.Sender.Username;

            dbCommand.Parameters.Add(new SQLiteParameter("@conversationid",
                GetConversationIdByUsernames(username, targetUsername)));
            dbCommand.Parameters.Add(new SQLiteParameter("@message", message.Message));
            dbCommand.Parameters.Add(new SQLiteParameter("@messagesentdatetime",
                message.MessageSentDateTime.ToString(CultureInfo.CurrentCulture)));
            dbCommand.Parameters.Add(new SQLiteParameter("@username", message.Sender.Username));
            dbCommand.Parameters.Add(new SQLiteParameter("@targetusername", message.Receiver.Username));

            dbCommand.CommandText =
                "INSERT INTO messages (conversationid, message, messagesentdatetime, username, targetusername) values (@conversationid, @message, @messagesentdatetime, @username, @targetusername)";
            
            dbCommand.ExecuteNonQuery();
        }

        public IEnumerable<SimpleConversation> GetConversations(SimpleUser user)
        {
            IDbCommand dbCommand = DataAccess.GetDbCommand();

            string localusername = user.Username;

            dbCommand.Parameters.Add(new SQLiteParameter("@localusername", localusername));

            dbCommand.CommandText = "SELECT id, targetusername FROM conversations WHERE localusername = @localusername";

            IDataReader dataReader = dbCommand.ExecuteReader();

            IList<SimpleConversation> conversations = new List<SimpleConversation>();
            while (dataReader.Read())
            {
                conversations.Add(new SimpleConversation(dataReader["targetusername"] as string));
            }

            return conversations;
        }
    }
}
