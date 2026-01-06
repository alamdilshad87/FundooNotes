using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs.Auth;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("secure")]
        public IActionResult Secure()
        {
            return Ok("You are Authenticated");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var verifyToken = await _authService.RegisterAsync(dto);
            return Ok(new
            {
                message = "User registered successfully",
                verifyToken = verifyToken
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            return Ok(new { token });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto dto)
        {
            await _authService.VerifyEmailAsync(dto.Token);
            return Ok("Email verified successfully");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var token = await _authService.ForgotPasswordAsync(dto.Email);
            return Ok(new { resetToken = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            await _authService.ResetPasswordAsync(dto.Token, dto.NewPassword);
            return Ok("Password reset successful");
        }
    }
}