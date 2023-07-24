using System.ComponentModel.DataAnnotations;

namespace GroceryStoreEF6.Model.ViewModel.Authentication.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage ="User Name is require")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is require")]
        public string? Password { get; set; }
    }
}
