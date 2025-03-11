using System;
using PermissivityProject.Data;
using PermissivityProject.Services;
using PermissivityProject.Models;

namespace PermissivityProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configura a conexão
            SQLiteDatabase db = new SQLiteDatabase();
            db.OpenConnection();

            // Cria as tabelas
            DatabaseSetup setup = new DatabaseSetup();
            setup.CreateTables();  // Agora funciona, pois o método existe

            // Criação de um usuário (opcional)
            UserService userService = new UserService();
            User user = new User
            {
                Name = "João",
                Email = "joao@exemplo.com",
                PasswordHash = "senha_segura",
                Role = "Agente"
            };
            userService.AddUser(user);

            db.CloseConnection();
        }
    }
}
