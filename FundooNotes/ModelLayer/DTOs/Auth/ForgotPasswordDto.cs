using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Auth
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
