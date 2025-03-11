using System;
using System.Data.SQLite;

namespace PermissivityProject.Data
{
    public class SQLiteDatabase
    {
        private SQLiteConnection connection;
        private string connectionString = @"Data Source=App_Data\permissivity.db;Version=3;";

        public SQLiteDatabase()
        {
            connection = new SQLiteConnection(connectionString);
        }

        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        public SQLiteConnection GetConnection()
        {
            return connection;
        }
    }
}
