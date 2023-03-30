using AutoMapper;
using ETicaretAPI.Application.Dto_s.User;
using ETicaretAPI.Application.Features.Commands.AppUser.CreateUser;
using ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Application.Features.Commands.AppUser.RefreshToken;
using ETicaretAPI.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E = ETicaretAPI.Application.Dto_s;

namespace ETicaretAPI.Application.Mapping.AutoMapper
{
    public class ConversionsProfile : Profile
    {
        public ConversionsProfile()
        {
            CreateMap<CreateUserCommandRequest, CreateUser>();
            CreateMap<CreateUserResponse, CreateUserCommandResponse>();
            CreateMap<E.Authentication.CreateUserResponse, FacebookLoginCommandSuccessResponse>();
            CreateMap<E.Authentication.CreateUserResponse, FacebookLoginCommandErrorResponse>();
            CreateMap<E.Authentication.CreateUserResponse, GoogleLoginCommandSuccessResponse>();
            CreateMap<E.Authentication.CreateUserResponse, GoogleLoginCommandErrorResponse>();
            CreateMap<E.Authentication.CreateUserResponse, LoginUserSuccessCommandResponse>();
            CreateMap<E.Authentication.CreateUserResponse, LoginUserErrorCommandResponse>();
            CreateMap<E.Authentication.CreateUserResponse, RefreshTokenSuccessCommandResponse>();
            CreateMap<E.Authentication.CreateUserResponse, RefreshTokenErrorCommandResponse>();

        }
    }
}
