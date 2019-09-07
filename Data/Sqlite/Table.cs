using System.Collections.Generic;
using System.Text;

namespace Falcon.Data.Sqlite
{
    public enum ColumnType { NULL, INTEGER, REAL, TEXT, BLOB }
    public class Table
    {
        public readonly string table_name;
        private Dictionary<string, ColumnType> columns;
        public Table(string table_name)
        {
            this.table_name = table_name;
            columns = new Dictionary<string, ColumnType>();
        }
        public void AddColumn(string column_name, ColumnType column_type)
        {
            if (!columns.ContainsKey(column_name))
                columns.Add(column_name, column_type);
        }
        public void RemoveColumn(string column_name)
        {
            if (!columns.ContainsKey(column_name))
                columns.Remove(column_name);
        }
        public string CreateTableQuery()
        {
            StringBuilder column = new StringBuilder();
            foreach (var col in columns)
            {
                column.Append($"{col.Key} {col.Value.ToString()},");
            }
            column.Remove(column.Length - 1, 1);
            return $"CREATE TABLE {table_name} ({column.ToString()})";
        }
    }
}
