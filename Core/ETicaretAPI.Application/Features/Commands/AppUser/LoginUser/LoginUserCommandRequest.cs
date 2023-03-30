using AutoMapper;
using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.Features.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ETicaretAPI.Application.Features.Commands.AppUser.LoginUser.LoginUserCommandResponse;
using E = ETicaretAPI.Application.Dto_s;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandRequest : IRequest<LoginUserCommandResponse>
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }

        public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
        {
            readonly IAuthService _authService;
            readonly IMapper _mapper;

            public LoginUserCommandHandler(IAuthService authService, IMapper mapper)
            {
                _authService = authService;
                _mapper = mapper;
            }

            public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
            {
                E.Authentication.CreateUserResponse response = await _authService.Login(request.UsernameOrEmail, request.Password);

                if (response.IsSucceeded)
                    return _mapper.Map<LoginUserSuccessCommandResponse>(response);
                else
                    return _mapper.Map<LoginUserErrorCommandResponse>(response);
            }
        }
    }
}
