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
            // Criar uma instância de SQLiteDatabase
            using (SQLiteDatabase db = new SQLiteDatabase())
            {
                // Criar as tabelas, caso não existam
                DatabaseSetup setup = new DatabaseSetup();
                setup.CreateTables(db); // Passando a instância db para o método CreateTables

                // Criar um novo usuário (opcional)
                UserService userService = new UserService();
                User user = new User
                {
                    Name = "João",
                    Email = "joao@exemplo.com",
                    PasswordHash = "senha_segura",
                    Role = "Agente"
                };
                userService.AddUser(user);
            }
        }
    }
}