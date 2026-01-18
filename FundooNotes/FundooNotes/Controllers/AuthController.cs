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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var otpSessionId = await _authService.RegisterAsync(dto);

            return Ok(new
            {
                otpSessionId,
                message = "Registration successful. OTP sent to email."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var otpSessionId = await _authService.LoginAsync(dto);

            return Ok(new
            {
                otpSessionId,
                message = "OTP sent to email for login verification."
            });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var token = await _authService.VerifyOtpAsync(dto);

            return Ok(new
            {
                message = "OTP verified successfully",
                token
            });
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpDto dto)
        {
            await _authService.ResendOtpAsync(dto.Email, dto.Purpose);

            var otpSessionId = await _authService.ResendOtpAsync(dto.Email, dto.Purpose);

            return Ok(new
            {
                otpSessionId,
                message = "OTP resent successfully"
            });

        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            await _authService.ForgotPasswordAsync(dto.Email);

            return Ok(new
            {
                message = "OTP sent to email for password reset."
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _authService.ResetPasswordAsync(dto);

            return Ok(new
            {
                message = "Password reset successful."
            });
        }

        [Authorize]
        [HttpGet("secure")]
        public IActionResult Secure()
        {
            return Ok("You are authenticated");
        }
    }
}