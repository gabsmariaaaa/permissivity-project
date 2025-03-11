using System;
using System.Data.SQLite;
using PermissivityProject.Models;
using PermissivityProject.Data;

namespace PermissivityProject.Services
{
    public class UserService
    {
        private SQLiteDatabase db;

        public UserService()
        {
            db = new SQLiteDatabase();
        }

        public void AddUser(User user)
        {
            string query = @"INSERT INTO Users (Name, Email, PasswordHash, Role)
                             VALUES (@Name, @Email, @PasswordHash, @Role);";

            using (SQLiteConnection conn = db.GetConnection())
            {
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                cmd.Parameters.AddWithValue("@Role", user.Role);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
