using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;

namespace WorkFlowPro.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration) { 
        
            _configuration = configuration;
        
        }

        public string GenarateToken(User user)
        {

            //getting things from appsetting.json
            var jwtSettings = _configuration.GetSection("jwtSettings");
            var secretKey = jwtSettings["SecretKey"]!;
            var issuer = jwtSettings["Issuer"]!;
            var audience = jwtSettings["Audience"]!;
            var expiryMinutes = int.Parse(jwtSettings["expiryMinutes "]!);

            //creating signing key from secret
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);

            // Define claims(data inside token)

            var claims = new[]
            {
                // Standard JWT claims
                new Claim(JwtRegisteredClaimNames.Sub,
                    user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,
                    user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),

                // Custom claims for our app
                new Claim("userId", user.Id.ToString()),
                new Claim("organizationId",
                    user.OrganizationId.ToString()),
                new Claim(ClaimTypes.Role,
                    user.Role.ToString()),
                new Claim("fullName", user.FullName)
            };

            // STEP 4: Build the token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
              signingCredentials: credentials);

            // STEP 5: Serialize to string
            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new Byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public DateTime GenerateTokenExpiry()
        {
            var everyMinutes = int.Parse(_configuration["JwtSettings:ExpiryMinutes"]!);
            return DateTime.UtcNow.AddMinutes(everyMinutes);
        }
    }
}
