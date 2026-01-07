using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Auth
{
    public class VerifyOtpDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Otp { get; set; } = null!;

        [Required]
        public string Purpose { get; set; } = null!;
    }
}