using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
{
    public interface ITaiKhoanQuanLyRepository
    {
        Task<List<TaiKhoan>> GettAllAdminAccount();

    }
}
