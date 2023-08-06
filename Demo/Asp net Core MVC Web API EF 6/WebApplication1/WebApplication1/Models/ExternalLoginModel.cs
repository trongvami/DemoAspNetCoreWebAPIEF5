namespace WebApplication1.Models
{
    public class ExternalLoginModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public bool isPersistent { get; set; }
        public bool bypassTwoFactor { get; set; }
    }
}
