using AutoMapper;
using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.Dto_s.Facebook;
using ETicaretAPI.Application.Features.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using E = ETicaretAPI.Application.Dto_s;


namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandRequest : IRequest<FacebookLoginCommandResponse>
    {
        public string AuthToken { get; set; }
        public string Provider { get; set; }
    }

    public class FacebookLoginCommmandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        readonly IAuthService _authService;
        readonly IMapper _mapper;

        public FacebookLoginCommmandHandler(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
            E.Authentication.CreateUserResponse response = await _authService.FacebookLogin(request.AuthToken,request.Provider);

            if (response.IsSucceeded)
            return _mapper.Map<FacebookLoginCommandSuccessResponse>(response);
            else
            return _mapper.Map<FacebookLoginCommandErrorResponse>(response);


        }
    }
}

