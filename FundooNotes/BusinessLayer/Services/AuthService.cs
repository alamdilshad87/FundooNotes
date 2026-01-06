using BusinessLayer.Interfaces;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using ModelLayer.DTOs.Auth;
using ModelLayer.Exceptions;
using ModelLayer.Helpers;

using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new ValidationException("User already exists");

            PasswordHasher.CreateHash(dto.Password, out var hash, out var salt);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                IsEmailVerified = false
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();

            return JwtHelper.GenerateToken(
                user.UserId,
                user.Email!,
                GetJwtKey(),
                GetJwtIssuer(),
                GetJwtAudience(),
                30,
                "verify"
            );
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email)
                ?? throw new UnauthorizedException("Invalid email or password");

            if (!PasswordHasher.VerifyPassword(
                    dto.Password,
                    user.PasswordHash!,
                    user.PasswordSalt!
                ))
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            return JwtHelper.GenerateToken(
                user.UserId,
                user.Email!,
                GetJwtKey(),
                GetJwtIssuer(),
                GetJwtAudience(),
                GetJwtExpiry(),
                "login"
            );
        }

        public async Task VerifyEmailAsync(string token)
        {
            int userId = JwtHelper.ValidateAndGetUserId(
                token,
                GetJwtKey(),
                "verify"
            );

            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new UnauthorizedException("Invalid token");

            user.IsEmailVerified = true;
            await _userRepository.SaveAsync();
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new NotFoundException("User not found");

            return JwtHelper.GenerateToken(
                user.UserId,
                user.Email!,
                GetJwtKey(),
                GetJwtIssuer(),
                GetJwtAudience(),
                15,
                "reset"
            );
        }

        public async Task ResetPasswordAsync(string token, string newPassword)
        {
            int userId = JwtHelper.ValidateAndGetUserId(
                token,
                GetJwtKey(),
                "reset"
            );

            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new UnauthorizedException("Invalid token");

            PasswordHasher.CreateHash(newPassword, out var hash, out var salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            await _userRepository.SaveAsync();
        }

        private string GetJwtKey() =>
            _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key missing");

        private string GetJwtIssuer() =>
            _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer missing");

        private string GetJwtAudience() =>
            _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("JWT Audience missing");

        private int GetJwtExpiry() =>
            int.Parse(
                _configuration["Jwt:ExpiresInMinutes"]
                    ?? throw new InvalidOperationException("JWT Expiry missing")
            );
    }
}