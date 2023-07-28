using System.ComponentModel.DataAnnotations;

namespace WebClient.ViewModels
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
