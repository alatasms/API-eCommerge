using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandResponse : BaseResponse
    {
    }
    public class FacebookLoginCommandSuccessResponse : FacebookLoginCommandResponse
    {
        public AccessToken AccessToken { get; set; }
    }
    public class FacebookLoginCommandErrorResponse : FacebookLoginCommandResponse
    {

    }
}
