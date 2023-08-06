using Microsoft.AspNetCore.Identity;

namespace ServerSide.Models.ViewModels.Authentication.Login
{
    public class AddLoginUserManager
    {
        public ExternalLoginInfo? ExternalLoginInfo { get; set; }
        public string? email { get; set; }
    }
}
