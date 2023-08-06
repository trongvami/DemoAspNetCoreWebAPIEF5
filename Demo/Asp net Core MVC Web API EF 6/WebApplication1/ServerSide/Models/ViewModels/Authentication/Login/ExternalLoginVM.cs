namespace ServerSide.Models.ViewModels.Authentication.Login
{
    public class ExternalLoginVM
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public bool isPersistent { get; set; }
        public bool bypassTwoFactor { get; set; }
    }
}
