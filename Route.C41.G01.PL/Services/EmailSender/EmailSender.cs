using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Route.C41.G01.PL.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendAsync(string from, string recipients, string subject, string body)
        {
            var SenderEmail = _configuration["EmailSettings:SenderEmail"];
            var SenderPassword = _configuration["EmailSettings:SenderPassword"];
            var emailMessage = new MailMessage();
            emailMessage.From = new MailAddress(from);
            emailMessage.To.Add(recipients);
            emailMessage.Subject = subject;
            emailMessage.Body = $"<html><body>{body}</body></html>";
            emailMessage.IsBodyHtml = true;
            var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpClientServer"], int.Parse(_configuration["EmailSettings:SmtpClientPort"]))
            {
                Credentials = new NetworkCredential(SenderEmail, SenderPassword),
                EnableSsl = true
            };
            await smtpClient.SendMailAsync(emailMessage);
        }
    }
}
