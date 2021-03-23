using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Infrastructure.Auth.Settings;

namespace YetAnotherTodoApp.Infrastructure.Auth.Helpers
{
    public class JwtHelper : IJwtHelper
    {
        private readonly JwtSettings _jwtOptions;

        public JwtHelper(IOptions<JwtSettings> options)
        {
            _jwtOptions = options.Value;
        }

        public JwtDto GenerateJwtToken(Guid userId)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var expires = now.AddMinutes(_jwtOptions.ExpiryTimeInMinutes);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret)),
                                                            SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken
            (
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new JwtDto
            {
                Token = token,
                Expires = expires
            };
        }
    }
}