using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Entities
{
    [Index(nameof(Email),IsUnique = true)]
    [Index(nameof(UserName),IsUnique = true)]
    public class UserAccount
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="This Field is Required!")]
        [MaxLength(50,ErrorMessage ="MaxLength is 50")]
        public string FirstName { get; set; }


        [Required(ErrorMessage ="This Field is Required!")]
        [MaxLength(50,ErrorMessage ="MaxLength is 50")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "This Field is Required!")]
        [MaxLength(50,ErrorMessage ="MaxLength is 50")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "This Field is Required!")]
        [MaxLength(50,ErrorMessage ="MaxLength is 50")]
        public string Email { get; set; }


        [Required(ErrorMessage = "This Field is Required!")]
        [MaxLength(50,ErrorMessage ="MaxLength is 50")]
        public string Password { get; set; }


    }
}
