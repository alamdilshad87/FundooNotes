using ModelLayer.DTOs.Auth;

namespace BusinessLayer.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task VerifyEmailAsync(string token);
        Task<string> ForgotPasswordAsync(string email);

    }
}
