using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CashFlow.Domain.Security
{
    public class JwtTokenGenerator : IAccessTokenGenerator
    {
        private readonly uint _expirationTimeInMinutes;
        private readonly string _signingKey;

        public JwtTokenGenerator(uint expirationTimeInMinutes, string signingKey)
        {
            _expirationTimeInMinutes = expirationTimeInMinutes;
            _signingKey = signingKey;
        }

        public string Generate(User user)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Sid, user.UserIdentifier.ToString()),
                new(ClaimTypes.Role, user.Role),
            };

            var tokenDescription = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes),
                SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(securityToken);
        }

        private SymmetricSecurityKey SecurityKey()
        {
            var key = Encoding.UTF8.GetBytes(_signingKey);
            return new SymmetricSecurityKey(key);
        }
    }
}
