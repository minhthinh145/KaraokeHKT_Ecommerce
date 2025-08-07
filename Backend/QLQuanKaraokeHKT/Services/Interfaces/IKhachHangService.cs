using QLQuanKaraokeHKT.DTOs;
using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Services.Interfaces
{
    public interface IKhachHangService
    {
        Task<ServiceResult> GetAllKhacHangAsync();
        Task<ServiceResult> GetKhachHangByIdAsync(Guid maKhachHang);
        Task<ServiceResult> CreateKhachHangByDangKyAsync(SignUpDTO signUp);
        Task<ServiceResult> UpdateKhachHangByTaiKhoanAsync(TaiKhoan taiKhoan);

    }
}
