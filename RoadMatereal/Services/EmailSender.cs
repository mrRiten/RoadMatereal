using System.Net;
using System.Net.Mail;

namespace RoadMatereal.Services
{
    public class EmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailSender(string smtpServer, int smtpPort, string smtpUsername,
            string smtpPassword)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
        }

        public async Task SendEmailAsync(string from, string to, string subject,
            string body)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_smtpUsername, 
                    _smtpPassword);
                client.EnableSsl = true;

                var message = new MailMessage(from, to, subject, body);
                await client.SendMailAsync(message);
            }
        }
    }
}
