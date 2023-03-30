using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstraction.Services
{
    public interface IMailService
    {
        public Task SendEmailAsync(string[] emails, string subject, string body, bool isbodyHtml=true);
        public Task SendEmailAsync(string email, string subject, string body, bool isbodyHtml = true);

        public Task SendPasswordResetMailAsync(string to, string userId, string resetToken);

        public Task SendEmailConfirmationLinkAsync(string to, string userId, string confirmationToken);

    }
}
