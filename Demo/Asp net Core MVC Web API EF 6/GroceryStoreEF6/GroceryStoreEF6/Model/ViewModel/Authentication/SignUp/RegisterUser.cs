using System.ComponentModel.DataAnnotations;

namespace GroceryStoreEF6.Model.ViewModel.Authentication.SignUp
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Password is required")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        public string Password { get; set; }

    }
}
