using DataBaseLayer.Entities;

namespace DataBaseLayer.Repositories.Interfaces
{
    public interface IOtpRepository
    {
        Task AddAsync(Otp otp);
        Task<Otp> GetValidOtp(int userId, string code, string purpose);
        Task SaveAsync();
    }
}