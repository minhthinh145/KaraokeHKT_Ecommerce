using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Customer
{
    public interface IKhachHangService
    {
        Task<ServiceResult> UpdateKhachHangByTaiKhoanAsync(TaiKhoan taiKhoan);

    }
}
