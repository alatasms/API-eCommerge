using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.RefreshToken
{

    public class RefreshTokenCommandResponse : BaseResponse
    {

    }

    public class RefreshTokenSuccessCommandResponse : RefreshTokenCommandResponse
    {
        public AccessToken? AccessToken { get; set; }

    }

    public class RefreshTokenErrorCommandResponse : RefreshTokenCommandResponse
    {

    }
}
