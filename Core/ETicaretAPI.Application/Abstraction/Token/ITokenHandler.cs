using ETicaretAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstraction.Token
{
    public interface ITokenHandler
    {
        AccessToken CreateAccessToken(AppUser user);
    }
}
