using ETicaretAPI.Application.Abstraction.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using Mailjet.Client;

namespace ETicaretAPI.Infrastructure.Services.Mail
{
    public class EmailService : IMailService
    {
        readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string body, bool isbodyHtml = true)
        {
            await SendEmailAsync(new[] { email }, subject, body);
        }

        public async Task SendEmailAsync(string[] emails, string subject, string body, bool isbodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml= isbodyHtml;
            foreach (var email in emails)
                mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["Mail:Username"], "Intern Control-AU ", Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Port = int.Parse(_configuration["Mail:Port"]);
            smtp.Host = _configuration["Mail:Host"];
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mail);
            
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            StringBuilder mail = new();
            mail.Append("Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br><strong><a target=\"_blank\" href=\"");
            mail.Append(_configuration["AngularClientUrl"]);
            mail.Append("/update-password/");
            mail.Append(userId);
            mail.Append("/");
            mail.Append(resetToken);
            mail.Append("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Eğer ki bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br>NG - Mini|E-Ticaret");

            await SendEmailAsync(to, "Şifre Yenileme Talebi", mail.ToString());
        }

        public async Task SendEmailConfirmationLinkAsync(string to, string userId, string confirmationToken)
        {
            StringBuilder mail = new();
            mail.Append("Merhaba<br>Aşağıdaki linkten mail adresinizi doğrulayınız.<br><strong><a target=\"_blank\" href=\"");
            mail.Append(_configuration["AngularClientUrl"]);
            mail.Append("/confirm-email/");
            mail.Append(userId);
            mail.Append("/");
            mail.Append(confirmationToken);
            mail.Append("\">Mail adresinizi doğrulamak için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Eğer ki bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br>NG - Mini|E-Ticaret");

            await SendEmailAsync(to, "Email Doğrulaması", mail.ToString());
        }



    }
}
