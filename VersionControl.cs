using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Falcon.Data.Local;

namespace Falcon
{
    public class Main
    {
        public void CreateDatabase()
        {
            using(DataBase db = new DataBase("test_db"))
            {
                DataTable person = new DataTable("person");
                person.Columns.Add("id", typeof(int), "PRIMARY KEY");
                person.Columns.Add("first_name", typeof(string));
                db.Table.CreateFromDataTable(person);
            }
        }
    }
    public static class VersionControl
    {
        private const string version = "0.0.1";
        private const string autor = "Mohammad Ali Fathi";
        public static string GetVersion() { return version; }
        public static string GetAutor() { return autor; }
        public static string GetInfo() { return $"Autor : {autor} | Version : {version}"; }
    }
}