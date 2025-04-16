using System.ComponentModel.DataAnnotations;

namespace WebServices.DTO
{
    public class CategoryDTO
    {
        //[MinLength(6, ErrorMessage = "Name must be Greater than 6")]
        public required string Name { get; set; }
        //[MinLength(6, ErrorMessage = "CategoryName must be Greater than 6")]
        public required string CategoryNumber { get; set; }
    }
}
