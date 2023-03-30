using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Application.Features.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.EmailConfirmation
{
    public class EmailConfirmationCommandRequest : IRequest<EmailConfirmationCommandResponse>
    {
        public string userId { get; set; }
        public string confirmationToken { get; set; }
    }

    public class EmailConfirmationCommandHandler : IRequestHandler<EmailConfirmationCommandRequest, EmailConfirmationCommandResponse>
    {
        readonly IUserService _userService;

        public EmailConfirmationCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<EmailConfirmationCommandResponse> Handle(EmailConfirmationCommandRequest request, CancellationToken cancellationToken)
        {
             await _userService.ConfirmEmailAsync(request.userId, request.confirmationToken);
            return new()
            {
                IsSucceeded = true,
                Message = ResponseMessages.EmailConfirmedSuccessfully.ToString()
            };
        }
    }
}
