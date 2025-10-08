using QLQuanKaraokeHKT.Core.Common;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.External
{
    public interface IMaOtpService
    {
        Task<ServiceResult> GenerateAndSendOtpAsync(string email);
        Task<ServiceResult> VerifyOtpAsync(string email, string otpCode);
        Task<ServiceResult> MarkOtpAsUsedAsync(string email, string otpCode);
    }
}
