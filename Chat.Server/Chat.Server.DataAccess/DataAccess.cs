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
            get { return _dbConnection ?? (_dbConnection = SQLiteProviderFactory.Instance.CreateConnection()); }
        }

        public static IDbCommand GetDbCommand()
        {
            if (DbConnection.State != ConnectionState.Closed)
                return DbConnection.CreateCommand();
            DbConnection.ConnectionString = _dataSource;
            DbConnection.Open();

            return DbConnection.CreateCommand();
        }

        public static void InitializeDatabase()
        {
            IDbCommand dbCommand = GetDbCommand();
            InitializeUsersTable(dbCommand);
            InitializeGroupsTable(dbCommand);
            InitializeUserGroupsTable(dbCommand);
        }

        private static void InitializeUsersTable(IDbCommand dbCommand)
        {
            dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS users " +
                                    "(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                    "username TEXT NOT NULL, " +
                                    "password TEXT NOT NULL, " +
                                    "online INTEGER NOT NULL)," +
                                    "picturedata BLOB;";
            dbCommand.ExecuteNonQuery();
        }

        private static void InitializeGroupsTable(IDbCommand dbCommand)
        {
            dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS groups" +
                                    "(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                    "uniquename TEXT NOT NULL UNIQUE, " +
                                    "name TEXT NOT NULL, " +
                                    "ownerId INTEGER NOT NULL);";
            dbCommand.ExecuteNonQuery();
        }

        private static void InitializeUserGroupsTable(IDbCommand dbCommand)
        {
            dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS usergroups" +
                                    "(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                    "groupid INTEGER NOT NULL, " +
                                    "userid INTEGER NOT NULL);";
            dbCommand.ExecuteNonQuery();
        }
    }
}
