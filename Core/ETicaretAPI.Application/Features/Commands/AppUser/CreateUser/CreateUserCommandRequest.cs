using AutoMapper;
using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Application.Dto_s.User;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using E = ETicaretAPI.Domain.Entities.Identity;
using E1 = ETicaretAPI.Application.Dto_s.User;

namespace ETicaretAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }


        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
        {
            readonly IUserService _userService;
            readonly IMapper _mapper;

            public CreateUserCommandHandler(IUserService userService, IMapper mapper)
            {
                _userService = userService;
                _mapper = mapper;
            }

            public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
            {

                CreateUserResponse response = await _userService.CreateUser(_mapper.Map<E1.CreateUser>(request));

                return _mapper.Map<CreateUserCommandResponse>(response);

            }
        }
    }
}
