using AutoMapper;
using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Application.Dto_s.Authentication;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.RefreshToken
{
    public class RefreshTokenCommandRequest : IRequest<RefreshTokenCommandResponse>
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommandRequest, RefreshTokenCommandResponse>
    {
        readonly IAuthService _authService;
        readonly IMapper _mapper;

        public RefreshTokenCommandHandler(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
        {
            CreateUserResponse response= await _authService.RefreshToken(request.RefreshToken);
            if (response.IsSucceeded)
                return _mapper.Map<RefreshTokenSuccessCommandResponse>(response);
            else
                return _mapper.Map<RefreshTokenErrorCommandResponse>(response);
        }
    }
}
