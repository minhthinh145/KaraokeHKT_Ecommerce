using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.Interfaces
{
    public interface IMaOtpService
    {
        Task<ServiceResult> GenerateAndSendOtpAsync(string email);
        Task<ServiceResult> VerifyOtpAsync(string email, string otpCode);
        Task<ServiceResult> MarkOtpAsUsedAsync(string email, string otpCode);
    }
}
