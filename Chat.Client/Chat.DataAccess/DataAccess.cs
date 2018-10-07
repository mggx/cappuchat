using System.Data;
using System.Data.SQLite.Linq;

namespace Chat.DataAccess
{
    public class DataAccess
    {
        private static string _dataSource = "Data source=db.db3";

        private static IDbConnection _dbConnection;
        private static IDbConnection DbConnection
        {
            get
            {
                if (_dbConnection == null)
                    _dbConnection = SQLiteProviderFactory.Instance.CreateConnection();
                return _dbConnection;
            }
        }

        public static IDbCommand GetDbCommand()
        {
            if (DbConnection.State == ConnectionState.Closed)
            {
                DbConnection.ConnectionString = _dataSource;
                DbConnection.Open();
            }

            return DbConnection.CreateCommand();
        }

        public static void InitializeDatabase()
        {
            IDbCommand conversationsDbCommand = GetDbCommand();
            InitializeConversationsTable(conversationsDbCommand);

            IDbCommand messagesDbCommand = GetDbCommand();
            InitializeMessagesTable(messagesDbCommand);
        }

        private static void InitializeConversationsTable(IDbCommand dbCommand)
        {
            dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS conversations " +
                                    "(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                    "localusername TEXT, " +
                                    "targetusername TEXT);";
            dbCommand.ExecuteNonQuery();
        }

        private static void InitializeMessagesTable(IDbCommand messagesDbCommand)
        {
            messagesDbCommand.CommandText = "CREATE TABLE IF NOT EXISTS messages " +
                                            "(ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                            "ConversationID INTEGER, " +
                                            "username TEXT," +
                                            "Message TEXT," +
                                            "MessageSentDateTime TEXT);";
            messagesDbCommand.ExecuteNonQuery();
        }
    }
}
