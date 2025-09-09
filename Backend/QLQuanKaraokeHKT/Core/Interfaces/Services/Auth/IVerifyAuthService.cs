using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    public interface IVerifyAuthService
    {
        Task<ServiceResult> VerifyAccountByEmail(VerifyAccountDTO verifyAccountDto);
    }
}
