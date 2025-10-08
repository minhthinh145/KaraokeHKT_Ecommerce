using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.HRM
{
    public interface INhanVienRepository : IGenericRepository<NhanVien, Guid>
    {
        Task<NhanVien?> GetNhanVienByTaiKhoanIdAsync(Guid maTaiKhoan);
        Task<NhanVien> GetByIdWithTaiKhoanAsync(Guid maNhanVien);
        Task<List<NhanVien>> GetAllByLoaiTaiKhoanWithTaiKhoanAsync(string loaiTaiKhoan);
        Task<List<NhanVien>> GetAllNhanVienWithTaiKhoanAsync();
        Task<bool> UpdateNhanVienDaNghiViecAsync(Guid maNhanVien, bool daNghiViec);
    }
}
