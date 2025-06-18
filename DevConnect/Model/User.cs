using System.ComponentModel.DataAnnotations;

namespace DevConnect.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required, MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(3)]
        public string Password { get; set; } = string.Empty;

        public string Roles { get; set; } = UserRole.Developer; // Default Role
    }
    public class UserRole
    {
        public const string Admin = "Admin";
        public const string Developer = "Developer";
    }
}
