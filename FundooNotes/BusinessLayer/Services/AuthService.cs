using BusinessLayer.Interfaces;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using Microsoft.Identity.Client;
using ModelLayer.DTOs.Auth;
using ModelLayer.Helpers;

namespace BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

        public async Task<bool> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid Email or Password");

            bool isValid = PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt);

            if (!isValid)
                throw new Exception("Invalid Email or Password");

            return true;
        }
    }
}