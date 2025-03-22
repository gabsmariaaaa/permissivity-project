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
            
            using (SQLiteDatabase db = new SQLiteDatabase())
            {
                
                DatabaseSetup setup = new DatabaseSetup();
                setup.CreateTables(db); 

               
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