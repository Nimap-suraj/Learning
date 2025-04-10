namespace EcommerceApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }


        // Navigation Key
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
