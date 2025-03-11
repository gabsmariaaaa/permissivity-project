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
            // Configura a conex�o
            SQLiteDatabase db = new SQLiteDatabase();
            db.OpenConnection();

            // Cria as tabelas
            DatabaseSetup setup = new DatabaseSetup();
            setup.CreateTables();  // Agora funciona, pois o m�todo existe

            // Cria��o de um usu�rio (opcional)
            UserService userService = new UserService();
            User user = new User
            {
                Name = "Jo�o",
                Email = "joao@exemplo.com",
                PasswordHash = "senha_segura",
                Role = "Agente"
            };
            userService.AddUser(user);

            db.CloseConnection();
        }
    }
}
