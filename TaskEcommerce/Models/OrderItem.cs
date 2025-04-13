using System.Text.Json.Serialization;

namespace TaskEcommerce.Models
{
    public class OrderItem
    {
        [JsonIgnore]
        public int OrderItemId { get; set; }

        [JsonIgnore]
        public int OrderId { get; set; }
        [JsonIgnore]

        public Order? Order { get; set; }

        public int ProductId { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }

        public int Quantity { get; set; } = 1;
    }

}
