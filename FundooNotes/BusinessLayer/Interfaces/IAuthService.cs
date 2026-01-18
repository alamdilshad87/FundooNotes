using ModelLayer.DTOs.Auth;

namespace BusinessLayer.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<string> VerifyOtpAsync(VerifyOtpDto dto);
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDto dto);
    }
}
