using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Application.Dto_s.User;
using ETicaretAPI.Application.Features.Common;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E = ETicaretAPI.Domain.Entities.Identity;


namespace ETicaretAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<E.AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateUser(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirsName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            }, model.Password);

            CreateUserResponse response = new() { IsSucceeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = ResponseMessages.UserCreated.ToString();
            else
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}\n";
                }

            return response;

        }
    }
}
