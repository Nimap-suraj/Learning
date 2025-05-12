using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class SignUpViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This Field is Required!")]
        [MaxLength(50, ErrorMessage = "MaxLength is 50")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "This Field is Required!")]
        [MaxLength(50, ErrorMessage = "MaxLength is 50")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "This Field is Required!")]
        [MaxLength(50, ErrorMessage = "MaxLength is 50")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "This Field is Required!")]
        [MaxLength(50, ErrorMessage = "MaxLength is 50")]
        [EmailAddress(ErrorMessage ="Please Enter Valid Email Address")]
        public string Email { get; set; }

        [DataType(DataType.Password)]

        [Required(ErrorMessage = "This Field is Required!")]
        [MaxLength(50, ErrorMessage = "MaxLength is 50")]
        public string Password { get; set; }
        [DataType(DataType.Password)]


        [Required(ErrorMessage = "This Field is Required!")]
        [MaxLength(50, ErrorMessage = "MaxLength is 50")]
        [Compare("Password",ErrorMessage ="Password is not correct")]
        public string ConfirmPassword { get; set; }

    }
}
