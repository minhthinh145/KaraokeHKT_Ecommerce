using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Auth
{
    public interface IMaOtpRepository : IGenericRepository<MaOtp, int>
    {
        Task<MaOtp> GetOtpByCodeAsync(string otpCode);

        Task<bool> MarkOtpAsUsedAsync(Guid userId,string otpCode);

        Task<bool> DeleteOtpUsedAsync(Guid userID, string otpCOde);
    }
}
