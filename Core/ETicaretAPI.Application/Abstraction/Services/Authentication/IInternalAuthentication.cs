using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E = ETicaretAPI.Application.Dto_s;

namespace ETicaretAPI.Application.Abstraction.Services.Authentication
{
    public interface IInternalAuthentication
    {
        Task<E.Authentication.CreateUserResponse> Login(string usernameOrEmail, string password);

        Task<E.Authentication.CreateUserResponse> RefreshToken(string refreshToken);


    }
}
