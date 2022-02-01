using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using TO_DO.ENTİTY.Models;
using TO_DO.SERVİCE.Contracts;
using TO_DO.SERVİCE.Dtos;

namespace TO_DO.API.JWTService
{
    public class TokenService : ITokenService
    {
        private readonly WebApplicationBuilder _builder;
        private readonly IMapper _mapper;
        public TokenService(WebApplicationBuilder builder, IMapper mapper)
        {
            _builder = builder;
            _mapper = mapper;
        }
        public string GetToken(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };
            var token = new JwtSecurityToken(
                issuer: _builder.Configuration["Issuer"],
                audience: _builder.Configuration["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_builder.Configuration["SigningKey"])),
                    SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}