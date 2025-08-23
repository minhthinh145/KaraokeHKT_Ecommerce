using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    public interface IVerifyAuthService
    {
        /// <summary>
        /// Verifies account activation by email using OTP
        /// </summary>
        /// <param name="verifyAccountDto">Contains user ID and OTP code</param>
        /// <returns>ServiceResult indicating success or failure</returns>
        Task<ServiceResult> VerifyAccountByEmail(VerifyAccountDTO verifyAccountDto);
    }
}
