namespace DemoFullIdentityEF6.Data.ResponseModels
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Username { get; set; }
    }
}
