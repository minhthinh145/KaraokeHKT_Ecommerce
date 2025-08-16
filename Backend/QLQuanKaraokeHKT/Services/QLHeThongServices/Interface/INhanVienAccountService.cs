using QLQuanKaraokeHKT.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices.Interface
{
    public interface INhanVienAccountService
    {
        Task<ServiceResult> GetAllTaiKhoanNhanVienAsync();
        Task<ServiceResult> GetAllTaiKhoanNhanVienByLoaiTaiKhoanAsync(string loaiTaiKhoan);
        Task<ServiceResult> AddTaiKhoanForNhanVienAsync(AddTaiKhoanForNhanVienDTO addTaiKhoanForNhanVienDto);

        Task<ServiceResult> GetProfileByUserIdAsync(Guid userId);
    }
}