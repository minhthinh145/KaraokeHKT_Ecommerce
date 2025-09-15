﻿using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
{
    public interface IMaOtpRepository : IGenericRepository<MaOtp, int>
    {
        Task<MaOtp> GetOtpByCodeAsync(string otpCode);

        Task<bool> MarkOtpAsUsedAsync(Guid userId,string otpCode);

        Task<bool> DeleteOtpUsedAsync(Guid userID, string otpCOde);
    }
}
