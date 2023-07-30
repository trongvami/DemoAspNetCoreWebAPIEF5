using ServerSide.Models.ResponseModels;

namespace ServerSide.MailServices
{
    public interface IEmailService
    {
        public void SendEmail(MessageResponse message);
    }
}
