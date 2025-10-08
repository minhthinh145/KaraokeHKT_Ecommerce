using QLQuanKaraokeHKT.Domain.Entities;

namespace QLQuanKaraokeHKT.Application.Repositories.Auth
{
    public interface ITaiKhoanQuanLyRepository
    {
        Task<List<TaiKhoan>> GettAllAdminAccount();

    }
}
