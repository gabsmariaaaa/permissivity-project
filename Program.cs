using System;
using PermissivityProject.Data;
using PermissivityProject.Models;
using PermissivityProject.Services;

namespace PermissivityProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Criar uma inst�ncia de SQLiteDatabase
            using (SQLiteDatabase db = new SQLiteDatabase())
            {
                // Criar as tabelas, caso n�o existam
                DatabaseSetup setup = new DatabaseSetup();
                setup.CreateTables(db); // Passando a inst�ncia db para o m�todo CreateTables

                // Criar um novo usu�rio (opcional)
                UserService userService = new UserService();
                User user = new User
                {
                    Name = "Jo�o",
                    Email = "joao@exemplo.com",
                    PasswordHash = "senha_segura",
                    Role = "Agente"
                };
                userService.AddUser(user);
            }
        }
    }
}