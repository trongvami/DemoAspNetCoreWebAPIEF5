using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class LoginVerifyModel
    {
        [Required]
        public string? code { get; set; }
        [EmailAddress]
        [Required]
        public string? email { get; set; }
    }
}
