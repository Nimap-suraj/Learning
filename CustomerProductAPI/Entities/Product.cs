using System.ComponentModel.DataAnnotations;

namespace CustomerProductAPI.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public  required string  Name { get; set; }

        public required decimal Price { get; set; }

        public List<Customer>? Customers { get; set; }

    }
}
