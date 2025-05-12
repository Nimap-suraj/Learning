using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Username or Email is Required!")]
        [MaxLength(50, ErrorMessage = "MaxLength is 50")]
        [DisplayName("Username or Email")]
        public string UserNameOrEmail { get; set; }




        [Required(ErrorMessage = "This Field is Required!")]
        [MaxLength(50, ErrorMessage = "MaxLength is 50")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
