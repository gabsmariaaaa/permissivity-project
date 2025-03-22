using PermissivityProject.Data;

namespace PermissivityProject
{
    public class DatabaseSetup
    {
        // Método que cria as tabelas no banco de dados
        public void CreateTables(SQLiteDatabase db)
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    Role TEXT NOT NULL
                );
            ";

            db.ExecuteQuery(createTableQuery);  // Executa a consulta SQL para criar a tabela
        }
    }
}
