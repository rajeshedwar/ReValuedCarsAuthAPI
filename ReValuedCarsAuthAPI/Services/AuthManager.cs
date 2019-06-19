using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ReValuedCarsAuthAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReValuedCarsAuthAPI.Infrastructure;

namespace ReValuedCarsAuthAPI.Services
{
    public class AuthManager : IAuthManager
    {
        private AuthDbContext authdb;
        private IConfiguration config;

        public AuthManager(AuthDbContext dbContext, IConfiguration configuration)
        {
            this.authdb = dbContext;
            this.config = configuration;
        }
        public async Task<dynamic> AddUserAsync(User user)
        {
            await authdb.Users.AddAsync(user);
            await authdb.SaveChangesAsync();
            return new
            {
                user.Id,
                user.Name,
                UserId = user.Email,
                user.PhoneNo
            };
        }

        public string AuthUsers(LoginModel user)
        {
            var result = authdb.Users.SingleOrDefault(c => c.Email == user.email && c.Password == user.password);
            if (result != null)
            {
                string token = GenerateToken(user.email, user.password);
                return token;
            }
            return null;
        }

        private string GenerateToken(string userid, string password)
        {

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userid),
                new Claim(JwtRegisteredClaimNames.Email,userid),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");
            if (userid == "Rajesh@hexaware.com")
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            }

            var sercuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("Jwt:Secret")));
            var credentials = new SigningCredentials(sercuritykey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: config.GetValue<string>("Jwt:Issuer"),
                audience: config.GetValue<string>("Jwt:Audience"),
                claims: claimsIdentity.Claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

