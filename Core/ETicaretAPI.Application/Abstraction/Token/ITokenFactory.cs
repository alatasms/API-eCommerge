using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstraction.Token
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 30);
    }
}
