using JWT.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IConfiguration _config;

        public AuthenticationService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJWTToken(UserModel model)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim("DateOfBirth", model.DateOfBirth.ToString("yyyy-MM-dd")),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:ValidAudience"],
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_config["JwtSettings:MinutesToExpire"])),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public interface IAuthenticationService
    {
        string GenerateJWTToken(UserModel model);
    }
}