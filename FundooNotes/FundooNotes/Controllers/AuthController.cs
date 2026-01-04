using BusinessLayer.Interfaces;
using ModelLayer.DTOs.Auth;

using Microsoft.AspNetCore.Mvc;

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
            await _authService.RegisterAsync(dto);
            return Ok(new { Message = "User Register Successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            await _authService.LoginAsync(dto);
            return Ok(new { Message ="User Login Successful" });
        }
    }
}