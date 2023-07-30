using MimeKit;
using MailKit.Net.Smtp;
using ServerSide.Models.ResponseModels;
using ServerSide.Common;

namespace ServerSide.MailServices
{
    public class EmailRepository : IEmailService
    {
        public EmailRepository()
        {

        }

        public void SendEmail(MessageResponse message)
        {
            var _emailConfig = Utilities.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            var emailMessage = CreateEmailMessage(message, _emailConfig);
            Send(emailMessage, _emailConfig);
        }

        private MimeMessage CreateEmailMessage(MessageResponse message, EmailConfiguration _emailConfig)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(string.Format("{0}", message.Subject), _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            if (!string.IsNullOrEmpty(_emailConfig.CC))
            {
                var ccEmail = _emailConfig.CC.Split(';').Where(x => x.Length > 0).ToList();
                emailMessage.Cc.AddRange(ccEmail.Select(x => new MailboxAddress("email", x)).ToList());
            }
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage, EmailConfiguration _emailConfig)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                client.Send(mailMessage);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
