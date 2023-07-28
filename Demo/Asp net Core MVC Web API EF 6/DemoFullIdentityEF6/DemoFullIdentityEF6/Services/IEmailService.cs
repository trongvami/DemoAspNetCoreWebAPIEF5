using DemoFullIdentityEF6.Data.ResponseModels;

namespace DemoFullIdentityEF6.Services
{
    public interface IEmailService
    {
        public void SendEmail(MessageResponse message);
    }
}
