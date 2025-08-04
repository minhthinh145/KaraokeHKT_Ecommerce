using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.Interfaces
{
    public interface IMaOtpService
    {
        /// <summary>
        /// Generates an OTP and sends it to the user's email/phone.
        /// </summary>
        /// <param name="userId">The user ID for which the OTP will be generated</param>
        /// <returns>A Task containing the result of the OTP generation process.</returns>
        Task<ServiceResult> GenerateAndSendOtpAsync(Guid userId);

        /// <summary>
        /// Verifies the OTP entered by the user.
        /// </summary>
        /// <param name="otpCode">The OTP code entered by the user</param>
        /// <returns>true if OTP is valid, false otherwise.</returns>
        Task<ServiceResult> VerifyOtpAsync(Guid userId, string otpCode);

        /// <summary>
        /// Marks the OTP as used after verification.
        /// </summary>
        /// <param name="otpCode">The OTP code to be marked as used.</param>
        /// <returns>A Task indicating whether the OTP was successfully marked as used.</returns>
        Task<ServiceResult> MarkOtpAsUsedAsync(Guid userId, string otpCode);
    }
}
