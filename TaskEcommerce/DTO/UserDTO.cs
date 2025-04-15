using System.ComponentModel.DataAnnotations;

namespace TaskEcommerce.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
     
    }
}
