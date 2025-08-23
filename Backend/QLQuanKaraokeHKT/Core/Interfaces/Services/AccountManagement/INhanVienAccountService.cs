using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement
{
    public interface INhanVienAccountService
    {
        Task<ServiceResult> GetAllTaiKhoanNhanVienAsync();
        Task<ServiceResult> GetAllTaiKhoanNhanVienByLoaiTaiKhoanAsync(string loaiTaiKhoan);
        Task<ServiceResult> AddTaiKhoanForNhanVienAsync(AddTaiKhoanForNhanVienDTO addTaiKhoanForNhanVienDto);

        Task<ServiceResult> GetProfileByUserIdAsync(Guid userId);
    }
}