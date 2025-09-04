using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;
using System.Runtime.CompilerServices;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth
{
    public class MaOtpRepository : GenericRepository<MaOtp, int>, IMaOtpRepository
    {

        public MaOtpRepository(QlkaraokeHktContext context) : base(context)
        {

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
                    return false; 
                }

                _context.MaOtps.Remove(otp);

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

            otp.DaSuDung = true;
            return true;
        }
    }
}
