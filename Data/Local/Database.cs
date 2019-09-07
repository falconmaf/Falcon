using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Falcon.Data.Local
{
    public class DataBase : IDisposable
    {
        #region Properties
        private string _name;
        /// <summary>
        /// Database File Name
        /// </summary>
        public string Name
        {
            get { return _name; }
            protected internal set { _name = value; }
        }
        private string _path;
        /// <summary>
        /// Database File Full Path
        /// </summary>
        public string Path
        {
            get { return _path; }
            protected internal set { _path = value; }
        }
        private string _dir;
        /// <summary>
        /// Database File Directory
        /// </summary>
        public string Directory
        {
            get { return _dir; }
            protected internal set { _dir = value; }
        }
        private SQLiteConnection _connection;
        /// <summary>
        /// Database Connection object
        /// </summary>
        public SQLiteConnection Connection
        {
            get { return _connection; }
            protected internal set { _connection = value; }
        }

        private TableCreator _table_creator;

        public TableCreator Table
        {
            get { return _table_creator; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Create Database object {Directory : BaseDirectory , Name : db}
        /// </summary>
        public DataBase() : this(AppDomain.CurrentDomain.BaseDirectory, "db") { }
        /// <summary>
        /// Create Database object on BaseDirectory of application path
        /// </summary>
        /// <param name="Name">Name of Database File</param>
        public DataBase(string Name) : this(AppDomain.CurrentDomain.BaseDirectory, Name) { }
        /// <summary>
        /// Create Database object
        /// </summary>
        /// <param name="Directory">Database Directory</param>
        /// <param name="Name">Database File Name</param>
        public DataBase(string Directory, string Name) : this(Directory, Name, null) { }
        /// <summary>
        /// Create Database object with tables
        /// </summary>
        /// <param name="Directory">Database Directory</param>
        /// <param name="Name">Database File Name</param>
        /// <param name="tables">Database Tables</param>
        public DataBase(string Directory, string Name, params DataTable[] tables)
        {
            // Set Database Directory
            this.Directory = Directory;
            // Set Database Name
            this.Name = Name;
            // Create Database Path
            Path = string.Format("{0}{1}", Directory, Name);
            // Create Database Directory
            if (!System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);
            // Create Database File and Create Database Connection Object
            if (!File.Exists(Path))
                SQLiteConnection.CreateFile(Path);
            Connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Path));
            _table_creator = new TableCreator(Connection);
        }
        #endregion

        #region Database Methods
        /// <summary>
        /// Database Void Event Handler signature.
        /// </summary>
        public virtual event EventHandler Handler;
        /// <summary>
        /// Database Update Method for handler.
        /// </summary>
        public virtual void Update()
        {
            Handler?.Invoke(this, null);
        }
        /// <summary>
        /// Open connection on current database object
        /// </summary>
        protected internal void OpenConnection()
        {
            if (Connection != null && Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();
        }
        /// <summary>
        /// Close connection on current database object
        /// </summary>
        protected internal void CloseConnection()
        {
            if (Connection != null && Connection.State != System.Data.ConnectionState.Closed)
                Connection.Close();
        }
        #endregion

        #region IDisposable
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
        #endregion

        #region Table Methods



        // TODO CreateTable()
        //public virtual string CreateTable(DataTable table)
        //{
        //    //string query = "CREATE TABLE person ( Field1 INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Field2 INTEGER, Field3 TEXT, Field4 TEXT ); ";
        //    StringBuilder query = new StringBuilder();
        //    query.Append("CREATE TABLE IF NOT EXISTS ");
        //    query.Append(table.TableName);
        //    query.Append(" ( ");
        //    for (int i = 0; i < table.Columns.Count; i++)
        //    {
        //        query.Append(table.Columns[i].ColumnName);
        //        query.Append(" ");
        //        if (table.Columns[i].DataType == typeof(int))
        //            query.Append("INTEGER ");
        //        if (table.Columns[i].DataType == typeof(string))
        //            query.Append("TEXT ");
        //        if (!table.Columns[i].AllowDBNull)
        //            query.Append("NOT NULL ");
        //    }
        //    return query.ToString();
        //}
        #endregion
    }
}