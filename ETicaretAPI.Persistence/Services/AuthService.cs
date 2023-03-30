using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.Dto_s.Authentication;
using ETicaretAPI.Application.Dto_s.Facebook;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Application.Features.Common;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using E = ETicaretAPI.Domain.Entities.Identity;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<E.AppUser> _userManager;
        readonly HttpClient _httpClient;
        readonly ITokenHandler _tokenHandler;
        readonly IConfiguration _configuration;
        readonly SignInManager<E.AppUser> _signInManager;
        readonly IUserService _userService;
        readonly IMailService _mailService;

        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<E.AppUser> userManager, ITokenHandler tokenHandler, SignInManager<E.AppUser> signInManager, IUserService userService, IMailService mailService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _userService = userService;
            _mailService = mailService;
        }

        async Task<BaseResponse> CreateUserexternalAsync(AppUser user,string email, string name, UserLoginInfo info)
        {
            return new CreateUserResponse();
        }



        public async Task<CreateUserResponse> FacebookLogin(string authToken, string provider)
        {
            string appAccessTokenString = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");

            AppAccessToken? appAccessToken = JsonSerializer.Deserialize<AppAccessToken>(appAccessTokenString);

            string sessionInfoAccessTokenString = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={appAccessToken.AccessToken}");

            UserAccessTokenValidation? validation = JsonSerializer.Deserialize<UserAccessTokenValidation>(sessionInfoAccessTokenString);


            //TODO Develop this algorithm a bit more.


            if (validation?.Data.IsValid !=null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,first_name,last_name&access_token={authToken}");
                //TODO fix. Can't get information
                FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);
                var info = new UserLoginInfo(provider, userInfo.UserId, provider);
                E.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                bool result = user != null;

                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(userInfo.Email);
                    result = user != null;
                    if (user == null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = userInfo.Email,
                            FirsName = userInfo.FirstName,
                            LastName = userInfo.LastName,
                            UserName = userInfo.Email
                        };

                        var identityResult = await _userManager.CreateAsync(user);
                        result = identityResult.Succeeded;
                    }
                }

                if (result)
                {
                    await _userManager.AddLoginAsync(user, info);
                    AccessToken token = _tokenHandler.CreateAccessToken(user);
                    await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 5);
                    return new CreateUserResponse()
                    {
                        IsSucceeded = true,
                        Message = ResponseMessages.UserLoggedInWithFacebookSuccessfully.ToString(),
                        AccessToken = token
                    };
                }
                else
                {
                    return new CreateUserResponse()
                    {
                        IsSucceeded = false,
                        Message = ResponseMessages.UserFacebookLoggedInFailed.ToString(),
                    };
                }
            }
            throw new Exception("Invalid External Authentication");
        }

        public async Task<CreateUserResponse> GoogleLogin(string idToken, string provider)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> {_configuration["ExternalLoginSettings:Google:Client_ID"] }
            };

            var payloads = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo(provider, payloads.Subject, provider);

            E.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payloads.Email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payloads.Email,
                        FirsName = payloads.GivenName,
                        LastName = payloads.FamilyName,
                        UserName = payloads.Email
                    };

                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
            {
                await _userManager.AddLoginAsync(user, info);

                AccessToken token = _tokenHandler.CreateAccessToken(user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 5);
                return new CreateUserResponse()
                {
                    IsSucceeded = true,
                    Message = ResponseMessages.UserLoggedInWithGoogleSuccessfully.ToString(),
                    AccessToken = token
                };
            }
            else
            {
                return new CreateUserResponse()
                {
                    IsSucceeded = false,
                    Message = ResponseMessages.UserGoogleLoggedInFailed.ToString(),
                };
            }
        }

        public async Task<CreateUserResponse> Login(string usernameOrEmail, string password)
        {
            E.AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(usernameOrEmail);

            if (user == null)
                throw new Exception("Kullanıcı Adı veya Parola hatalı");
            if (!user.EmailConfirmed)
                    return new CreateUserResponse
                    {
                        IsSucceeded = false,
                        Message = ResponseMessages.EmailShouldBeConfirmed.ToString(),
                        AccessToken=null
                    };
                
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded) //TODO Authentication başarılı!
            {
                //TODO Yetkileri Belirteceğiz. 5'i appsettings'e al.
                AccessToken token = _tokenHandler.CreateAccessToken(user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration,5);
                return new CreateUserResponse()
                {
                    IsSucceeded = true,
                    Message = ResponseMessages.UserLoggedInSuccessfully.ToString(),
                    AccessToken = token
                };
            }

            return new CreateUserResponse
            {
                IsSucceeded = false,
                Message = ResponseMessages.UserLoggedInFailed.ToString()
            };
        }

        public async Task<CreateUserResponse> RefreshToken(string refreshToken)
        {
            AppUser? user= _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                AccessToken token = _tokenHandler.CreateAccessToken(user);
                ; await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);
                return new()
                {
                    AccessToken = token,
                    IsSucceeded = true,
                    Message = ResponseMessages.UserLoggedInSuccessfully.ToString()
                };
            }
            else
                return new()
                {
                    IsSucceeded = false,
                    Message = ResponseMessages.UserGoogleLoggedInFailed.ToString()
                };
        }

        public async Task GeneratePasswordResetTokenAsync(string email)
        {
            AppUser? user = await _userManager.FindByEmailAsync(email);

            if (user!=null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                resetToken=resetToken.UrlEncode();
                

                await _mailService.SendPasswordResetMailAsync(email, user.Id, resetToken);
            }
        }

        public async Task<bool> VerifyPasswordResetTokenAsync(string resetToken, string userId)
        
        {
            AppUser? user =await _userManager.FindByIdAsync(userId);
            if (user!=null)
            {
               resetToken= resetToken.UrlDecode();

                return await _userManager.VerifyUserTokenAsync(user,
                    _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword",
                    resetToken);
            }
            return false;
        }

        public async Task<bool> EmailConfirmTokenAsync(string email)
        {
            AppUser? user = await _userManager.FindByEmailAsync(email);
            if (user!=null)
            { 
                string confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                confirmToken = confirmToken.UrlEncode();

                await _mailService.SendEmailConfirmationLinkAsync(email, user.Id, confirmToken);

            }
            return new();
        }

        public async Task<bool> VerifyEmailConfirmTokenAsync(string userId, string confirmationToken)
        {
            AppUser? user =await _userManager.FindByIdAsync(userId);
            if (user!=null)
            {
               confirmationToken= confirmationToken.UrlDecode();

                return await _userManager.VerifyUserTokenAsync(user,
                    _userManager.Options.Tokens.EmailConfirmationTokenProvider, "EmailConfirmation",
                    confirmationToken);
            }
            return false;
        }
    }
}
