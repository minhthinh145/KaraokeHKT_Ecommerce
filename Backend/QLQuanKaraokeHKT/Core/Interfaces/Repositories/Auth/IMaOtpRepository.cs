using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
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
        /// <summary>
        ///  delete an OTP that has been used.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="otpCOde"></param>
        /// <returns></returns>
        Task<bool> DeleteOtpUsedAsync(Guid userID, string otpCOde);
    }
}
