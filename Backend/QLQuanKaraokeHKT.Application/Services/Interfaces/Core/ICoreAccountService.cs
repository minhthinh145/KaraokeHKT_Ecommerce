using QLQuanKaraokeHKT.Application.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Application.DTOs.Core;
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Core
{
    public interface ICoreAccountService
    {
        Task<ServiceResult<TaiKhoan>> CreateAccountAsync(CreateAccountRequest createAccountRequest);

        Task<ServiceResult<TaiKhoan>> UpdateAccountAsync(UpdateAccountRequest updateAccountRequest);

        Task<ServiceResult> MarkAccountAsLockOrUnlockAsync(Guid accountId, bool isLock);
    }
}
