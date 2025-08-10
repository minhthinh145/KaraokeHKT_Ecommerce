using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices.Interface
{
    public interface IKhachHangAccountService
    {
        Task<ServiceResult> GetAllTaiKhoanKhachHangAsync();
    }
}
