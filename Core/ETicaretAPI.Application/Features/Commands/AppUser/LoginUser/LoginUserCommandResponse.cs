using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandResponse:BaseResponse
    {

    }

    public class LoginUserSuccessCommandResponse : LoginUserCommandResponse
    {
        public AccessToken? AccessToken { get; set; }

    }

    public class LoginUserErrorCommandResponse : LoginUserCommandResponse
    {

    }

   
}
