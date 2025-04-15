using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskEcommerce.Models
{
    public class Category
    {
        //[Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Name is Required!")]
        [MinLength(3, ErrorMessage = "Name Must be at Least 2 Character!")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Category Number is required.")]
        [MinLength(3, ErrorMessage = "Category Number must be at least 3 characters.")]
        public string? CategoryNumber { get; set; }

        [JsonIgnore]
        public List<Product>? Products { get; set; }
    }
}
