using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskEcommerce.Models
{
    public class Product
    {
        //[Key]
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required int ProductPrice { get; set; }

        public int ProductStock { get; set; }

        //[ForeignKey("CategoryId")]
        public required int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; } 
    }
}
