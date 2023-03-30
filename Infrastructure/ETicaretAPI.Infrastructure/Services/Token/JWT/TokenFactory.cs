using ETicaretAPI.Application.Abstraction.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Token.JWT
{
    public class TokenFactory : ITokenFactory
    {
        public string GenerateToken(int size = 30)
        {
            var randomNumber=new byte[30];
            using var rng=RandomNumberGenerator.Create(); //RandomNumberGenerator is IDisposable
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
