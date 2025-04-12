using System.ComponentModel.DataAnnotations;

namespace TaskEcommerce.Models
{
    public class Category
    {
        //[Key]
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string CategoryNumber { get; set; }
        public List<Product>? Products { get; set; }
    }
}
