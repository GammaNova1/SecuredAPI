using Microsoft.IdentityModel.Tokens;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User = EntityLayer.Concrete.User;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; } //appsettings.json'u okumaya yarar. Buradan token bilgileri alınacak
        private TokenOptions _tokenOptions; //Appsetting'e yapılan token ayarlarının alınması için 
        private DateTime _accessTokenExpiration;
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

        }
        public AccessToken CreateToken(User user, List<Role> operationClaims)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration); // Token'in geçerlilik süresi
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey); //Hangi key hangi algoritma kullanılacak 
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        
        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<Role> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<Role> operationClaims) //Bize ait olmayan nesnelere yeni methodlar ekleyebiliyoruz extension ile ama hem method hem class static olmalı
                                                                                              //Burada Claims nesnesine yeni özellikler eklemek için bir extension yazıldı. 
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.Name} {user.Surname}");
            claims.AddRoles(operationClaims?.Select(c => c.Name).Where(name => !string.IsNullOrEmpty(name)).ToArray() ?? Array.Empty<string>());
            if (operationClaims != null && operationClaims.Any())
    {
        claims.CustomAddRole(operationClaims.First().Name); // İlk rolü eklemek için örnek
    }
            return claims;
        }
    }
}
