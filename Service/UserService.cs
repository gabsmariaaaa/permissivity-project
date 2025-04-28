using System.Data.SQLite;
using PermissivityProject.Models;
using PermissivityProject.Data;

namespace PermissivityProject.Services
{
    public class UserService
    {
        private readonly SQLiteDatabase _db;

        public UserService(SQLiteDatabase db)
        {
            _db = db;
        }

        public void AddUser(User user)
        {
            string query = @"INSERT INTO Users (Name, Email, PasswordHash, Role)
                         VALUES (@Name, @Email, @PasswordHash, @Role);";

            using (var conn = _db.GetConnection())
            {
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", user.Name ?? "");
                    cmd.Parameters.AddWithValue("@Email", user.Email ?? "");
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash ?? "");
                    cmd.Parameters.AddWithValue("@Role", user.Role ?? "Agente");

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public User? GetUserById(int id)
        {
            string query = @"SELECT * FROM Users WHERE Id = @Id";

            using (SQLiteConnection conn = _db.GetConnection())
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                PasswordHash = reader.GetString(3),
                                Role = reader.GetString(4)
                            };
                        }

                        return null;
                    }
                }
            }
        }

        public User? Authenticate(string email, string passwordHash)
        {
            string query = @"SELECT * FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash";

            using (SQLiteConnection conn = _db.GetConnection())
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                PasswordHash = reader.GetString(3),
                                Role = reader.GetString(4)
                            };
                        }

                        return null;
                    }
                }
            }
        }

        // Método para listar todos os usuários
        public List<User> GetAllUsers()
        {
            string query = @"SELECT * FROM Users";

            var users = new List<User>();

            using (SQLiteConnection conn = _db.GetConnection())
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                PasswordHash = reader.GetString(3),
                                Role = reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return users;
        }
    }
}
