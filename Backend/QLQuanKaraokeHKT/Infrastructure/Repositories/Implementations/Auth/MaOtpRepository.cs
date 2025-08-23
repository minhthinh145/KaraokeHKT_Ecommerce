using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Infrastructure.Data;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth
{
    public class MaOtpRepository : IMaOtpRepository
    {
        private readonly QlkaraokeHktContext _context;

        public MaOtpRepository(QlkaraokeHktContext context)
        {
            _context = context;
        }

        public async Task<MaOtp> CreateOTPAsync(MaOtp maOtp)
        {
            await _context.MaOtps.AddAsync(maOtp);
            await _context.SaveChangesAsync();
            return maOtp;
        }

        public async Task<bool> DeleteOtpUsedAsync(Guid userID, string otpCode)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(otpCode))
                {
                    return false;
                }

                // Find OTP that is used by specific user
                var otp = await _context.MaOtps
                    .FirstOrDefaultAsync(o => o.MaTaiKhoan == userID
                                           && o.maOTP == otpCode
                                           && o.DaSuDung);

                if (otp == null)
                {
                    return false; // OTP không tồn tại hoặc chưa được sử dụng
                }

                // Remove the OTP record
                _context.MaOtps.Remove(otp);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<MaOtp> GetOtpByCodeAsync(string otpCode)
        {
            if (string.IsNullOrWhiteSpace(otpCode))
            {
                return null;
            }
            Console.WriteLine($"UTCNOW:{DateTime.UtcNow}");
            return await _context.MaOtps
                .FirstOrDefaultAsync(otp => otp.maOTP == otpCode 
                                         && !otp.DaSuDung 
                                         && otp.NgayHetHan > DateTime.UtcNow);
        }

        public async Task<bool> MarkOtpAsUsedAsync(Guid userId, string otpCode)
        {
            var otp = await _context.MaOtps
                .FirstOrDefaultAsync(o => o.maOTP == otpCode && o.MaTaiKhoan == userId && !o.DaSuDung);

            if (otp == null)
            {
                return false;
            }

            if (otp.NgayHetHan < DateTime.UtcNow)
            {
                return false; 
            }

            // Mark OTP as used
            otp.DaSuDung = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
