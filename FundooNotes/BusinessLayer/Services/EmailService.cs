using BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace BusinessLayer.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(
                _config["Smtp:Username"],
                _config["Smtp:Password"]
                ),
                EnableSsl = true
            };

            var mail = new MailMessage(
                _config["Smtp:From"]!,
                to,
                subject,
                body
            );

            await smtp.SendMailAsync(mail);
        }
    }
}