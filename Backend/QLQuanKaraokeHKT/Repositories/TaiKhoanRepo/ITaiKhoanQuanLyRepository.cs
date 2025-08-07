using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Repositories.TaiKhoanRepo
{
    public interface ITaiKhoanQuanLyRepository
    {
        Task<List<TaiKhoan>> GettAllAdminAccount();

    }
}
