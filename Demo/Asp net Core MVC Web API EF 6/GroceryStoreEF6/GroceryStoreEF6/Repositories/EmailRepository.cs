using GroceryStoreEF6.Common;
using GroceryStoreEF6.Model.ResponseModel;
using GroceryStoreEF6.Services;
using MailKit.Net.Smtp;
using MimeKit;

namespace GroceryStoreEF6.Repositories
{
    public class EmailRepository : IEmailService
    {
        public EmailRepository()
        {
            
        }
        //private readonly EmailConfiguration _emailConfig;
        //public EmailRepository(EmailConfiguration emailConfig)
        //{
        //    _emailConfig = emailConfig;
        //}
        public void SendEmail(MessageResponse message)
        {
            var _emailConfig = Utilities.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            var emailMessage = CreateEmailMessage(message, _emailConfig);
            Send(emailMessage, _emailConfig);
        }

        private MimeMessage CreateEmailMessage(MessageResponse message, EmailConfiguration _emailConfig) { 
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email",_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage, EmailConfiguration _emailConfig) {
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
            finally {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
