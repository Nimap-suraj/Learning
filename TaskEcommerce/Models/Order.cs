using System.Text.Json.Serialization;

namespace TaskEcommerce.Models
{
    public class Order
    {
        [JsonIgnore]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
       
        public List<OrderItem>? Items { get; set; }
        public bool IsDeleted { get; set; } // soft delete flag

      

    }

}
