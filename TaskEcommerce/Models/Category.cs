using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskEcommerce.Models
{
    public class Category
    {
        //[Key]
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string CategoryNumber { get; set; }
        [JsonIgnore]
        public List<Product>? Products { get; set; }
    }
}
