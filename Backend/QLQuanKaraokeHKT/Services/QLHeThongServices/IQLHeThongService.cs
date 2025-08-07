using QLQuanKaraokeHKT.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices
{
    public interface IQLHeThongService
    {
        Task<ServiceResult> GetAllTaiKhoanKhachHangAsync();
        Task<ServiceResult> GetAllTaiKhoanNhanVienAsync();
        Task<ServiceResult> GetAllTaiKhoanNhanVienByLoaiTaiKhoanAsync(string loaiTaiKhoan);
        Task<ServiceResult> AddTaiKhoanForNhanVienAsync(AddTaiKhoanForNhanVienDTO addTaiKhoanForNhanVienDto);
        Task<ServiceResult> GetAllNhanVienAsync();
        Task<ServiceResult> LockAccountByMaTaiKhoanAsync(Guid maTaiKhoan);
        Task<ServiceResult> UnlockAccountByMaTaiKhoanAsync(Guid maTaiKhoan);
        Task<ServiceResult> GettAllAdminAccount();
    }
}
