using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandResponse:BaseResponse
    {

    }
    public class GoogleLoginCommandSuccessResponse : GoogleLoginCommandResponse
    {
        public AccessToken AccessToken { get; set; }
    }
    public class GoogleLoginCommandErrorResponse : GoogleLoginCommandResponse
    {

    }

}
