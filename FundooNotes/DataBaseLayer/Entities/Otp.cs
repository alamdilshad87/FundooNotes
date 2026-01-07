namespace DataBaseLayer.Entities
{
    public class Otp
    {
        public int OtpId { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; } = null!;
        public string Purpose { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public User User { get; set; } = null!;
    }
}