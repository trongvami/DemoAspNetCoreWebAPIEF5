using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace GroceryStoreEF6.Model.ViewModel.Authentication.SignUp
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; } = null!;
        [Compare("Password", ErrorMessage ="The password and comfirmation password do not match")]
        public string ConfirmPassword { get; set; } = null !;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
