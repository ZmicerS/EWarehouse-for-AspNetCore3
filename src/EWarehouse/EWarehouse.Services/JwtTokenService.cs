using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using EWarehouse.Services.Entities.AccountModels;
using EWarehouse.Services.Entities.TokenModels;
using EWarehouse.Services.Contracts;

namespace EWarehouse.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration, IAccountService accountService)
        {
            _configuration = configuration;
            _accountService = accountService;
        }

        public JwtTokenModel CreateJwtToken(LoginServiceModel model)
        {
            var identity = GetIdentity(model).GetAwaiter().GetResult();
            if (identity == null)
            {
                return null;
            }
            var encodedJwt = GenerateToken(identity);
            var token = new JwtTokenModel
            {
                AccessToken = encodedJwt,
                UserName = identity.Name
            };
            return token;
        }

        private string GenerateToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            double.TryParse(_configuration["Jwt:Lifetime"], out double lifeTime);        
            var jwt = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Issuer"],
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(lifeTime)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private async Task<ClaimsIdentity> GetIdentity(LoginServiceModel model)
        {
            var user = await _accountService.GetUserAsync(model);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName)
                };

                foreach (var roles in user.AssignedRoles)
                {
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, roles.RoleName));
                }
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}
