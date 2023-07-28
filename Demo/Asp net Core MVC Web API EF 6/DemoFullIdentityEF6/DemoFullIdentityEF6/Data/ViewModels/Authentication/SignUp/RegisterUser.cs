using System.ComponentModel.DataAnnotations;

namespace DemoFullIdentityEF6.Data.ViewModels.Authentication.SignUp
{
    public class RegisterUser
    {
        //[Required(ErrorMessage = "User Name is required")]
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string Service { get; set; }
        public DateTime Dob { get; set; }

        //[EmailAddress]
        //[Required(ErrorMessage = "Password is required")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Email is required")]
        public string Password { get; set; }
        public string Phone { get; set; }
    }
}
