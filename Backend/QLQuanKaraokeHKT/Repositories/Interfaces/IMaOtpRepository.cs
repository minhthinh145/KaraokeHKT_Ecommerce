using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.Interfaces
{
    public interface IMaOtpRepository
    {
        /// <summary>
        /// Create a new OTP (One-Time Password) record in the database.
        /// </summary>
        /// <param name="maOtp"></param>
        /// <returns></returns>
        Task<MaOtp> CreateOTPAsync(MaOtp maOtp);
        /// <summary>
        /// Get an OTP by its code.
        /// </summary>
        /// <param name="otpCode"></param>
        /// <returns></returns>
        Task<MaOtp> GetOtpByCodeAsync(string otpCode);
        /// <summary>
        /// Mark an OTP as used by updating its status in the database.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otpCode"></param>
        /// <returns></returns>
        Task<bool> MarkOtpAsUsedAsync(Guid userId,string otpCode);
    }
}
