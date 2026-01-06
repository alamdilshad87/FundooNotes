using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ModelLayer.Helpers
{
    public class JwtHelper
    {
        public static string GenerateToken(
            int userid, string email, string key, string issuer, string audience, int expiresInMinutes)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userid.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static int ValidateAndGetUserId(string token, string key)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key)
                )
            };

            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                token,
                validationParams,
                out _
            );

            string userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("Invalid token");

            return int.Parse(userId);
        }
    }
}