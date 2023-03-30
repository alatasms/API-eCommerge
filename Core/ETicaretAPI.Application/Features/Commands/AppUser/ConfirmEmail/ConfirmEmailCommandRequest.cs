using ETicaretAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.ConfirmEmail
{
    public class ConfirmEmailCommandRequest : IRequest<ConfirmEmailCommandResponse>
    {
        public string Email { get; set; }
    }
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommandRequest, ConfirmEmailCommandResponse>
    {
        readonly IAuthService _authService;

        public ConfirmEmailCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ConfirmEmailCommandResponse> Handle(ConfirmEmailCommandRequest request, CancellationToken cancellationToken)
        {
           bool state= await _authService.EmailConfirmTokenAsync(request.Email);
            return new()
            {
                IsSucceeded = state
            };
        }
    }

}
