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
        private readonly IOtpRepository _otpRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IOtpRepository otpRepository,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _otpRepository = otpRepository;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
                throw new ValidationException("Email already registered");

            PasswordHasher.CreateHash(dto.Password, out var hash, out var salt);

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                IsEmailVerified = false
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();

            var otpSessionId = await GenerateAndSendOtp(user, "verify");
            return otpSessionId;
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email)
                ?? throw new UnauthorizedException("Invalid credentials");

            if (!PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash!, user.PasswordSalt!))
                throw new UnauthorizedException("Invalid credentials");

            if (!user.IsEmailVerified)
                throw new UnauthorizedException("Email not verified");

            var otpSessionId = await GenerateAndSendOtp(user, "login");
            return otpSessionId;
        }

        public async Task<string> VerifyOtpAsync(VerifyOtpDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email)
                ?? throw new NotFoundException("User not found");

            var otp = await _otpRepository.GetValidOtpBySession(
                dto.OtpSessionId,
                dto.Otp,
                dto.Purpose
            );

            otp.IsUsed = true;

            if (dto.Purpose == "verify")
                user.IsEmailVerified = true;

            await _otpRepository.SaveAsync();

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

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new NotFoundException("User not found");

            _ = await GenerateAndSendOtp(user, "reset");
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email)
                ?? throw new NotFoundException("User not found");

            var otp = await _otpRepository.GetValidOtp(
                user.UserId,
                dto.Otp,
                "reset"
            );

            otp.IsUsed = true;

            PasswordHasher.CreateHash(dto.NewPassword, out var hash, out var salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            await _otpRepository.SaveAsync();
        }

        private async Task<string> GenerateAndSendOtp(User user, string purpose)
        {
            var otpCode = OtpGenerator.Generate();
            var otpSessionId = Guid.NewGuid().ToString();

            var otp = new Otp
            {
                UserId = user.UserId,
                OtpSessionId = otpSessionId,
                Code = otpCode,
                Purpose = purpose,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            await _otpRepository.AddAsync(otp);
            await _otpRepository.SaveAsync();

            await _emailService.SendAsync(
                user.Email!,
                "OTP Verification",
                $"Your OTP is {otpCode}. It expires in 10 minutes."
            );

            return otpSessionId;
        }

        private string GetJwtKey() => _configuration["Jwt:Key"]!;
        private string GetJwtIssuer() => _configuration["Jwt:Issuer"]!;
        private string GetJwtAudience() => _configuration["Jwt:Audience"]!;
        private int GetJwtExpiry() => int.Parse(_configuration["Jwt:ExpiresInMinutes"]!);
    }
}
