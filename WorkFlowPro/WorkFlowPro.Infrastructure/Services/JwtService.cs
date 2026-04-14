using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return ;
        }

        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public DateTime GenerateTokenExpiry()
        {
            throw new NotImplementedException();
        }
    }
}
