using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E = ETicaretAPI.Application.Dto_s;

namespace ETicaretAPI.Application.Abstraction.Services.Authentication
{
    public interface IExternalAuthentication
    {
        Task<E.Authentication.CreateUserResponse> FacebookLogin(string authToken, string provider);
        Task<E.Authentication.CreateUserResponse> GoogleLogin(string idToken, string provider);
    }
}
