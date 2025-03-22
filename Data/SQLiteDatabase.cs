using System;
using System.Data.SQLite;

namespace PermissivityProject.Data
{
    public class SQLiteDatabase : IDisposable
    {
        private SQLiteConnection? connection;
        private string connectionString = @"Data Source=App_Data\permissivity.db;Version=3;";

        public SQLiteDatabase()
        {
            try
            {
                connection = new SQLiteConnection(connectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao abrir a conexão: " + ex.Message);
                throw; // Lança a exceção novamente para indicar falha
            }
        }

        public SQLiteConnection GetConnection()
        {
            if (connection == null) // Verifica se a conexão é nula
            {
                throw new InvalidOperationException("Conexão com o banco de dados não foi inicializada."); // Lança uma exceção se for nula
            }
            if (connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao abrir a conexão: " + ex.Message);
                    throw; // Lança a exceção novamente em caso de falha
                }
            }
            return connection;
        }

        public void ExecuteQuery(string query)
        {
            using (var command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }
    }
}