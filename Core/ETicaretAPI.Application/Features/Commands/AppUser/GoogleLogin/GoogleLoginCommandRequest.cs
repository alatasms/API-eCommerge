using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.Features.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E = ETicaretAPI.Application.Dto_s;
using ETicaretAPI.Application.Abstraction.Services;
using AutoMapper;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{

    public class GoogleLoginCommandRequest : IRequest<GoogleLoginCommandResponse>
    {
        public string IdToken { get; set; }
        public string Provider { get; set; }

        public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
        {
            readonly IAuthService _authService;
            readonly IMapper _mapper;

            public GoogleLoginCommandHandler(IAuthService authService, IMapper mapper)
            {
                _authService = authService;
                _mapper = mapper;
            }

            public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
            {
                E.Authentication.CreateUserResponse response = await _authService.GoogleLogin(request.IdToken,request.Provider);

                if (response.IsSucceeded)
                    return _mapper.Map<GoogleLoginCommandSuccessResponse>(response);
                else
                    return _mapper.Map<GoogleLoginCommandErrorResponse>(response);
            }
        }
    }
    
}
