using DataBaseLayer.Entities;

namespace DataBaseLayer.Repositories.Interfaces
{
    public interface IOtpRepository
    {
        Task AddAsync(Otp otp);
        Task SaveAsync();

        Task<Otp> GetValidOtp(int userId, string otp, string purpose );

        Task<Otp> GetValidOtpBySession(string otpSessionId, string otp, string purpose );
    }
}