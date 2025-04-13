namespace CustomerProductAPI.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<Product>? Products { get; set; }

    }
}
