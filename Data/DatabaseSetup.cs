using System;
using System.Data.SQLite;

namespace PermissivityProject.Data
{
    public class DatabaseSetup
    {
        private SQLiteDatabase db;

        public DatabaseSetup()
        {
            db = new SQLiteDatabase();
        }

        // Método para criar as tabelas no banco de dados
        public void CreateTables()
        {
            string createUserTableQuery = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    Role TEXT NOT NULL
                );";

            // Abrir a conexão com o banco de dados
            using (SQLiteConnection conn = db.GetConnection())
            {
                SQLiteCommand cmd = new SQLiteCommand(createUserTableQuery, conn);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Tabelas criadas com sucesso!");
        }
    }
}
