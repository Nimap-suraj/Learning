using System.ComponentModel.DataAnnotations;

namespace WebServices.Model
{
    public class User
    {
        public int Id { get; set; }

       
        [MinLength(5,ErrorMessage ="User name must be Greater Than 5")]

        public required string UserName { get;set; }
        [MinLength(5,ErrorMessage ="Password name must be Greater Than 5")]
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}
 