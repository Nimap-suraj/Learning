using System.ComponentModel.DataAnnotations;

namespace WebServices.DTO
{
    public class UserDTO
    {
        [Required]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters long.")]
        public string UserName { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters long.")]
        public string Password { get; set; }

        [Required]
        [MinLength(7, ErrorMessage = "Email must be at least 7 characters long.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
