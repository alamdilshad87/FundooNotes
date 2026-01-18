namespace ModelLayer.DTOs.Auth
{
    public class ResendOtpDto
    {
        public string Email { get; set; } = null!;
        public string Purpose { get; set; } = null!;
    }
}