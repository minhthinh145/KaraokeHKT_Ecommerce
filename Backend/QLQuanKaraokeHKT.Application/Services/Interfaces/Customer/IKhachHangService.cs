
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Customer
{
    public interface IKhachHangService
    {
        Task<ServiceResult> UpdateKhachHangByTaiKhoanAsync(TaiKhoan taiKhoan);

    }
}
