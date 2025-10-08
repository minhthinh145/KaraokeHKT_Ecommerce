using QLQuanKaraokeHKT.Core.Common;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement
{
    public interface IKhachHangAccountService
    {
        Task<ServiceResult> GetAllTaiKhoanKhachHangAsync();
    }
}
