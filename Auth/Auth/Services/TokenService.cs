using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.Interfaces;
using Auth.Models;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _conf;

        public TokenService(IConfiguration conf)
        {
            _conf = conf;
        }

        public string Generate(User user)
        {
            var claims = CreateClaims(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = CreateToken(claims, creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Claim[] CreateClaims(User user)
        {
            return
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            ];
        }

        private JwtSecurityToken CreateToken(Claim[] claims, SigningCredentials creds)
        {
            return new JwtSecurityToken(
                issuer: _conf["JwtSettings:Issuer"],
                audience: _conf["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
        }
    }
}