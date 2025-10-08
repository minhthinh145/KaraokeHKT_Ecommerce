using QLQuanKaraokeHKT.Application.Repositories.Auth;
using QLQuanKaraokeHKT.Application.Repositories.Booking;
using QLQuanKaraokeHKT.Application.Repositories.Customer;
using QLQuanKaraokeHKT.Application.Repositories.HRM;
using QLQuanKaraokeHKT.Application.Repositories.Inventory;
using QLQuanKaraokeHKT.Application.Repositories.Room;

namespace QLQuanKaraokeHKT.Application
{
    public interface IUnitOfWork : IDisposable
    {
        #region Auth & Account Management
        IAccountManagementRepository AccountManagementRepository { get; }
        IIdentityRepository IdentityRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IMaOtpRepository MaOtpRepository { get; }
        IRoleRepository RoleRepository { get; }
        ITaiKhoanQuanLyRepository TaiKhoanQuanLyRepository { get; }
        ISignUpRepository SignUpRepository { get; }
        #endregion

        #region Custom & Employee Repository base
        IKhachHangRepository KhachHangRepository { get; }
        INhanVienRepository NhanVienRepository { get; }
        #endregion

        #region Booking System
        IThuePhongRepository ThuePhongRepository { get; }
        IHoaDonRepository HoaDonRepository { get; }
        ILichSuSuDungPhongRepository LichSuSuDungPhongRepository { get; }
        IChiTietHoaDonDichVuRepository ChiTietHoaDonDichVuRepository { get; }
        #endregion

        #region Room Management
        IPhongHatKaraokeRepository PhongHatRepository { get; }
        ILoaiPhongRepository LoaiPhongRepository { get; }
        IPhongHatKaraokeRepository PhongHatKaraokeRepository { get; }

        #endregion

        #region Inventory & Pricing
        ISanPhamDichVuRepository SanPhamDichVuRepository { get; }
        IGiaDichVuRepository GiaDichVuRepository { get; }
        IVatLieuRepository VatLieuRepository { get; }
        IGiaVatLieuRepository GiaVatLieuRepository { get; }
        IMonAnRepository MonAnRepository { get; }
        #endregion

        #region HRM
        ICaLamViecRepository CaLamViecRepository { get; }
        ILichLamViecRepository LichLamViecRepository { get; }
        ILuongCaLamViecRepository LuongCaLamViecRepository { get; }
        IYeuCauChuyenCaRepository YeuCauChuyenCaRepository { get; }
        #endregion

        #region ViewModel Repositories
        IPhongHatForCustomerViewRepository PhongHatForCustomerViewRepository { get; }
        #endregion

        #region Transaction Management
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task ExecuteTransactionAsync(Func<Task> action);
        Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> action);
        #endregion

    
    }
}
