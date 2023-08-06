using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication1.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is require !")]
        [EmailAddress]
        [DisplayName("Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is require !")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[DisplayName("Remember Me")]
        //public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();
    }
}
