using System.Net;
using System.Net.Mail;

namespace DemoG02.Presentation.Utilities
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("OmarWael.Route@gmail.com", "zyghdegxwrnsllri");
            client.Send("OmarWael.Route@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
