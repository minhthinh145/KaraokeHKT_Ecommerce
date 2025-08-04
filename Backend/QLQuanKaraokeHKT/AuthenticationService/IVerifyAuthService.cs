using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.AuthenticationService
{
    public interface IVerifyAuthService
    {
        /// <summary>
        /// Verifies account activation by email using OTP
        /// </summary>
        /// <param name="verifyAccountDto">Contains user ID and OTP code</param>
        /// <returns>ServiceResult indicating success or failure</returns>
        Task<ServiceResult> VerifyAccountByEmail(VerifyAccountDTO verifyAccountDtok, Guid userId);
    }
}
