using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.Dto_s.Authentication;
using ETicaretAPI.Application.Dto_s.Facebook;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Application.Features.Common;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<E.AppUser> userManager, ITokenHandler tokenHandler, SignInManager<E.AppUser> signInManager)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
        }
        public async Task<CreateUserResponse> FacebookLogin(string authToken, string provider)
        {
            string appAccessTokenString = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");

            AppAccessToken? appAccessToken = JsonSerializer.Deserialize<AppAccessToken>(appAccessTokenString);

            string sessionInfoAccessTokenString = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={appAccessToken.AccessToken}");

            UserAccessTokenValidation? validation = JsonSerializer.Deserialize<UserAccessTokenValidation>(sessionInfoAccessTokenString);


            //  Bu algoritmayı biraz daha geliştir.


            if (validation?.Data.IsValid !=null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,first_name,last_name&access_token={authToken}");
                //burada veri gelmiyor
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


                    return new CreateUserResponse()
                    {
                        IsSucceeded = true,
                        Message = ResponseMessages.UserLoggedInWithFacebookSuccessfully.ToString(),
                        AccessToken = _tokenHandler.CreateAccessToken()
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


                return new CreateUserResponse()
                {
                    IsSucceeded = true,
                    Message = ResponseMessages.UserLoggedInWithGoogleSuccessfully.ToString(),
                    AccessToken = _tokenHandler.CreateAccessToken()
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

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded) //Authentication başarılı!
            {
                //....Yetkileri Belirteceğiz.
                AccessToken token = _tokenHandler.CreateAccessToken();
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

    }
}
