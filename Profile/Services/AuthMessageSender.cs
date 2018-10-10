using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Threading.Tasks;

namespace Profile.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        readonly ILogger<AuthMessageSender> logger;

        public AuthMessageSender(ILogger<AuthMessageSender> logger)
        {
            this.logger = logger;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            logger.LogInformation("Email: {email}, Subject: {subject}, Message: {message}", email, subject, message);


            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Claxe Inlogserver", "xxxx@claxe.com"));
            mailMessage.To.Add(new MailboxAddress(email));
            mailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            //bodyBuilder.TextBody = message;
            mailMessage.Body = bodyBuilder.ToMessageBody();

            using (var mclient = new SmtpClient())
            {
                mclient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                mclient.Connect("mail.claxe.com", 25, false);
                mclient.Authenticate("xxxxxx", "xxxxxxx");
                mclient.Send(mailMessage);
                mclient.Disconnect(true);
            }

            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            logger.LogInformation("SMS: {number}, Message: {message}", number, message);
            return Task.FromResult(0);
        }
    }
}
