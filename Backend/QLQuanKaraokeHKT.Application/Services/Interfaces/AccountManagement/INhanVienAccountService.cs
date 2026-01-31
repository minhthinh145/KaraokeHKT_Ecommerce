
using QLQuanKaraokeHKT.Application.DTOs.QLHeThongDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.AccountManagement
{
    public interface INhanVienAccountService
    {
        Task<ServiceResult> GetAllTaiKhoanNhanVienAsync();

        Task<ServiceResult> GetAllTaiKhoanNhanVienByLoaiTaiKhoanAsync(string loaiTaiKhoan);

        Task<ServiceResult> ChangeTaiKhoanForNhanVienAsync(AddTaiKhoanForNhanVienDTO addTaiKhoanForNhanVienDto);

        Task<ServiceResult> GetProfileByUserIdAsync(Guid userId);
    }
}