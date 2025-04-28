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
                throw; 
            }
        }

        public SQLiteConnection GetConnection()
        {
            if (connection == null)
            {
                throw new InvalidOperationException("Conexão com o banco de dados não foi inicializada.");
            }

            // Não fechamos a conexão aqui, pois ela pode ser reutilizada posteriormente
            if (connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao abrir a conexão: " + ex.Message);
                    throw;
                }
            }
            return connection; // Retornamos a conexão aberta
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