using System.ComponentModel.DataAnnotations;

namespace DataBaseLayer.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; } = null;

        [Required]
        public string? PasswordHash { get; set; } = null;

        [Required]
        public string? PasswordSalt { get; set; } = null;

        public bool IsEmailVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
