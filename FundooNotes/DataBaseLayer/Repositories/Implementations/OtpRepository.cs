using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Exceptions;

namespace DataBaseLayer.Repositories.Implementations
{
    public class OtpRepository : IOtpRepository
    {
        private readonly FundooNotesDbContext _context;

        public OtpRepository(FundooNotesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Otp otp)
        {
            await _context.Otps.AddAsync(otp);
        }

        public async Task<Otp> GetValidOtp(int userId, string code, string purpose)
        {
            return await _context.Otps.FirstOrDefaultAsync(o =>
                o.UserId == userId &&
                o.Code == code &&
                o.Purpose == purpose &&
                !o.IsUsed &&
                o.ExpiresAt > DateTime.UtcNow
            ) ?? throw new UnauthorizedException("Invalid or expired OTP");
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}