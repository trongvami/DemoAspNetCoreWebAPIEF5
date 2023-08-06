using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class AddLoginUserManager
    {
        public ExternalLoginInfo? ExternalLoginInfo { get; set; }
        public string? email { get; set; }
    }
}
