 using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Application.Dto_s.User;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Common;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

            AppUser user = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirsName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

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

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {

            if (user!=null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate=accessTokenDate.AddMinutes(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
            throw new NotFoundUserException();


        }

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user!=null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult result=await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                    await _userManager.UpdateSecurityStampAsync(user);
                else
                    throw new PasswordChangeFailedException();

            }
        }

        public async Task ConfirmEmailAsync(string userId, string confirmationToken)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user!=null)
            {
                confirmationToken=confirmationToken.UrlDecode();
                IdentityResult result = await _userManager.ConfirmEmailAsync(user, confirmationToken);
                if (result.Succeeded)
                     await _userManager.UpdateSecurityStampAsync(user);
                else
                    throw new EmailConfirmationFailedException();
            }
 
        }
    }
}
