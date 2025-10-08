using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.DTOs.Core;
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Core
{
    public interface ICoreAccountService
    {
        Task<ServiceResult<TaiKhoan>> CreateAccountAsync(CreateAccountRequest createAccountRequest);

        Task<ServiceResult<TaiKhoan>> UpdateAccountAsync(UpdateAccountRequest updateAccountRequest);

        Task<ServiceResult> MarkAccountAsLockOrUnlockAsync(Guid accountId, bool isLock);
    }
}
