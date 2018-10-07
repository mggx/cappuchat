using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Linq;

namespace Chat.Server.DataAccess
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
            IDbCommand dbCommand = GetDbCommand();
            InitializeUsersTable(dbCommand);
        }

        private static void InitializeUsersTable(IDbCommand dbCommand)
        {
            dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS users " +
                                    "(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                    "username TEXT NOT NULL, " +
                                    "password TEXT NOT NULL, " +
                                    "online INTEGER NOT NULL);";
            dbCommand.ExecuteNonQuery();
        }
    }
}
