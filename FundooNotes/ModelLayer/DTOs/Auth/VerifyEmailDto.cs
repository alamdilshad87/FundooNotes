using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Auth
{
    public class VerifyEmailDto
    {
        [Required]
        public string Token { get; set; } = null!;
    }
}