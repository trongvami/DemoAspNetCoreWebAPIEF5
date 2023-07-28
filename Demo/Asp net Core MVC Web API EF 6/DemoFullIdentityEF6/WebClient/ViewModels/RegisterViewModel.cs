using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebClient.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name is require !")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middle Name is require !")]
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is require !")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Full Name is require !")]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Address is require !")]
        [DisplayName("Address")]
        public string Address { get; set; }

        public string? Service { get; set; }

        [Required(ErrorMessage = "Date of Birth is require !")]
        [DisplayName("Date of Birth")]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Email is require !")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is require !")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone is require !")]
        public string Phone { get; set; }



        //[DisplayName("Remember Me")]
        //public bool RememberMe { get; set; }
    }
}
