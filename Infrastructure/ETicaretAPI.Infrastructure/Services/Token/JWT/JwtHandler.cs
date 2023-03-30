using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Token.JWT
{
    public class JwtHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;
        readonly ITokenFactory _tokenFactory;

        public JwtHandler(IConfiguration configuration, ITokenFactory tokenFactory)
        {
            _configuration = configuration;
            _tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptions>();
            _tokenFactory = tokenFactory;
        }

        private TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;



        public AccessToken CreateAccessToken(AppUser user)
        {
            _accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration);

            //Get the symmetry of the Security Key.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));

            //Create the Encrypted ID.
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwt = CreateJwtSecurityToken(user,_tokenOptions, signinCredentials);

            //Take an instance of the token generator and create the token.
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration,
                RefreshToken= _tokenFactory.GenerateToken()
            };

        }

        public JwtSecurityToken CreateJwtSecurityToken(AppUser user, TokenOptions tokenOptions, SigningCredentials signingCredentials)
        {
            //Fill in the token to be created via tokenOptions.
            var jwt = new JwtSecurityToken(
                issuer:tokenOptions.Issuer,
                audience:tokenOptions.Audience,
                expires:_accessTokenExpiration,
                notBefore:DateTime.UtcNow,
                signingCredentials:signingCredentials,
                claims:new List<Claim> { new(ClaimTypes.Name,user.UserName)}
                );

            return jwt;
        }
    }
}
