using System.IO;
using System.Data.SQLite;
using Falcon.Debug;

namespace Falcon.Data.Sqlite
{
    public class Database
    {
        private Logger logger;
        private SQLiteConnection connection;
        private string database_name;
        private string path = @"C:/ProgramData/";
        public Database(string directory_name, string database_name)
        {
            this.database_name = database_name;
            path += directory_name;
            logger = new Logger(path);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string database_path = $"{path}/{database_name}";
            connection = new SQLiteConnection($"Data Source={database_path}; Version=3;");
            if (!File.Exists(database_path))
            {
                SQLiteConnection.CreateFile(database_path);
                logger.Log($"Database Created -- {this.database_name}");
            }
            else
            {
                logger.Log($"Database Found -- {this.database_name}");
            }
        }
        public void OpenConnection()
        {
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
        }
        public void CloseConnection()
        {
            if (connection.State != System.Data.ConnectionState.Closed)
                connection.Close();
        }
        public object CreateTable(Table table)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(table.CreateTableQuery(), connection))
            {
                try
                {
                    OpenConnection();
                    var o = cmd.ExecuteNonQuery();
                    logger.Log($"Table '{table.table_name}' Created in -- {this.database_name}");
                    CloseConnection();
                    return o;
                }
                catch (SQLiteException e)
                {
                    logger.Log($"{e.Message} -- {this.database_name}");
                }
                CloseConnection();
                return null;
            }
        }
        public object InsertInto(string table_name, string column_name, string values)
        {
            SQLiteCommand command = new SQLiteCommand($"INSERT INTO {table_name} {column_name} values {values}", connection);
            return command.ExecuteNonQuery();
        }
    }
}