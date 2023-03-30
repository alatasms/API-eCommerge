using ETicaretAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.VerifyEmailConfirmToken
{
    public class VerifyEmailConfirmTokenCommandRequest : IRequest<VerifyEmailConfirmTokenCommandResponse>
    {
        public string UserId { get; set; }
        public string ConfirmationToken { get; set; }
    }

    public class VerifyEmailConfirmTokenCommandHandler : IRequestHandler<VerifyEmailConfirmTokenCommandRequest, VerifyEmailConfirmTokenCommandResponse>
    {
        readonly IAuthService _authService;

        public VerifyEmailConfirmTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<VerifyEmailConfirmTokenCommandResponse> Handle(VerifyEmailConfirmTokenCommandRequest request, CancellationToken cancellationToken)
        {
           bool state= await _authService.VerifyEmailConfirmTokenAsync(request.UserId, request.ConfirmationToken);
            return new()
            {
                IsSucceeded = state
            };
        }
    }
}
