using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Телефонный_справочник_с_фотографиями
{
    internal class DataBaseConnection
    {
        public SQLiteConnection Connection { get; set; }
        Database database;
        public DataBaseConnection(string basename)
        {
            Connection = new SQLiteConnection($"Data Source={basename};Vercion=3");
            Connection.Open();
            database.CreateDataBase();
        } 
    }
}
