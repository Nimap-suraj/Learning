namespace EcommerceApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        // Navigation Property
        public ICollection<Product> Products { get; set; }
    }
}
