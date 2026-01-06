using BusinessLayer.Interfaces;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using ModelLayer.DTOs.Auth;
using ModelLayer.Helpers;

using Microsoft.Extensions.Configuration;
using System.Formats.Asn1;

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
        public async Task RegisterAsync(RegisterDto dto)
        {
            var exisiting = await _userRepository.GetByEmailAsync(dto.Email);
            if (exisiting != null)
                throw new Exception("User Already Exists");

            PasswordHasher.CreateHash(dto.Password, out var hash, out var salt);
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid Email or Password");
            if (user.PasswordHash == null || user.PasswordSalt == null)
                throw new Exception("Invalid email or password");

            bool isValid = PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt);

            if (!isValid)
                throw new Exception("Invalid Email or Password");

            string key = _configuration["Jwt:Key"] ?? throw new Exception("JWT Key missing");
            string issuer = _configuration["Jwt:Issuer"] ?? throw new Exception("JWT Issuer missing");
            string audience = _configuration["Jwt:Audience"] ?? throw new Exception("JWT Audience missing");
            int expiresInMinutes = int.Parse( _configuration["Jwt:ExpiresInMinutes"] ?? throw new Exception("JWT Expiry missing"));

            return JwtHelper.GenerateToken(user.UserId, user.Email!, key, issuer, audience, expiresInMinutes);
        }

        public async Task VerifyEmailAsync(string token)
        {
            int userId = JwtHelper.ValidateAndGetUserId(
        token,
        _configuration["Jwt:Key"]!
    );

            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new Exception("Invalid token");

            user.IsEmailVerified = true;
            await _userRepository.SaveAsync();
        }
    }
}