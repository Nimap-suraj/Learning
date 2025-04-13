using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskEcommerce.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public  string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public bool isDeleted { get; set; } = false;

        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
    }
}
