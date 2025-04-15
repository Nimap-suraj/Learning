namespace TaskEcommerce.DTO
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required int ProductPrice { get; set; }

        public int ProductStock { get; set; }

        //[ForeignKey("CategoryId")]
        public required int CategoryId { get; set; }

    }
}
