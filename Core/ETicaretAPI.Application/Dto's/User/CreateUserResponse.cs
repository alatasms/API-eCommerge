using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Dto_s.User
{
    public class CreateUserResponse :BaseResponse
    {
        public AccessToken? AccessToken { get; set; }
    }
}
