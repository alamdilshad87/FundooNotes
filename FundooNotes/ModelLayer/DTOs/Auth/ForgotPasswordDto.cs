using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Auth
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
