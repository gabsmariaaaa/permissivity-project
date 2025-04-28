namespace PermissivityProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; } // <- agora pode ser nulo
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
    }

}