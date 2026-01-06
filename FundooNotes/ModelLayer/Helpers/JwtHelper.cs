using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Exceptions;

namespace ModelLayer.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(
            int userId,
            string email,
            string key,
            string issuer,
            string audience,
            int expiresInMinutes,
            string purpose)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("purpose", purpose)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static int ValidateAndGetUserId(
            string token,
            string key,
            string expectedPurpose)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key))
            };

            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                token,
                validationParams,
                out _
            );

            var purpose = principal.FindFirst("purpose")?.Value;
            if (purpose != expectedPurpose)
                throw new UnauthorizedException("Invalid token purpose");

            string userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedException("Invalid token");

            return int.Parse(userId);
        }
    }
}
