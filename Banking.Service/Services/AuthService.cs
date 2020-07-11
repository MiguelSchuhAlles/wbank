using Banking.Domain.Entities;
using Banking.Infrastructure;
using Banking.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Banking.Shared.Responses;
using Banking.Shared;

namespace Banking.Service.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly Settings _settings;

        public AuthService(Settings settings, BankingContext context) : base(context)
        {
            _settings = settings;
        }

        public async Task<AuthenticationResponse> Authenticate(string email, string password)
        {
            var user = await Context.Users.Include(u => u.Account).FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null) return null;

            var response = new AuthenticationResponse();

            response.UserId = user.Id;
            response.Name = user.Name;
            response.Email = user.Email;
            response.AccountId = user.Account.Id;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            response.Token = tokenHandler.WriteToken(token);

            return response;
        }
    }
}
