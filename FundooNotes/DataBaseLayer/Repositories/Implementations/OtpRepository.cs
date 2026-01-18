using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Exceptions;
using System;

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

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Otp> GetValidOtp(
            int userId,
            string otp,
            string purpose
        )
        {
            var otpEntity = await _context.Otps
                .Where(o =>
                    o.UserId == userId &&
                    o.Code == otp &&
                    o.Purpose == purpose &&
                    !o.IsUsed &&
                    o.ExpiresAt > DateTime.UtcNow
                )
                .FirstOrDefaultAsync();

            if (otpEntity == null)
                throw new ValidationException("Invalid or expired OTP");

            return otpEntity;
        }

        public async Task<Otp> GetValidOtpBySession(
            string otpSessionId,
            string otp,
            string purpose
        )
        {
            var otpEntity = await _context.Otps
                .Where(o =>
                    o.OtpSessionId == otpSessionId &&
                    o.Code == otp &&
                    o.Purpose == purpose &&
                    !o.IsUsed &&
                    o.ExpiresAt > DateTime.UtcNow
                )
                .FirstOrDefaultAsync();

            if (otpEntity == null)
                throw new ValidationException("Invalid or expired OTP");

            return otpEntity;
        }
    }
}