using System.ComponentModel.DataAnnotations;

namespace WebServices.Model
{
    public class Category
    {
        public int Id { get; set; }

        [MinLength(6,ErrorMessage ="Name must be Greater than 6")]
        public required string Name { get; set; }
        public required string CategoryNumber { get;set; }   

    }
}
