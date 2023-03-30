using ETicaretAPI.Application.Abstraction.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstraction.Services
{
    public interface IAuthService : IExternalAuthentication, IInternalAuthentication
    {
        public Task GeneratePasswordResetTokenAsync(string email);
        Task<bool> VerifyPasswordResetTokenAsync(string resetToken, string userId);

        public Task<bool> EmailConfirmTokenAsync(string email);
        Task<bool> VerifyEmailConfirmTokenAsync(string userId, string confirmationToken);



    }
}
