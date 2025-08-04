using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.Implementation
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

        public async Task<MaOtp> GetOtpByCodeAsync(string otpCode)
        {
          return await _context.MaOtps
                .FirstOrDefaultAsync(otp => otp.maOTP == otpCode && !otp.DaSuDung);
        }

        public async Task<bool> MarkOtpAsUsedAsync(Guid userId, string otpCode)
        {
            var otp = await _context.MaOtps
                .FirstOrDefaultAsync(o => o.maOTP == otpCode && o.MaTaiKhoan == userId && !o.DaSuDung);
            if (otp == null)
            {
                otp.DaSuDung = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
