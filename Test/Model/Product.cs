using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int price { get; set; }

        [ForeignKey(nameof(Product.Id))]
        public int CategoryId { get; set; }
    }
}
