using ModelLayer.DTOs.Auth;

namespace BusinessLayer.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task VerifyEmailAsync(string token);
        Task<string> ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(string token, string newPassword);
    }
}