using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Booking;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Customer;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Room;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Booking;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Customer;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.HRM;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Inventory;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Room;

namespace QLQuanKaraokeHKT.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QlkaraokeHktContext _context;
        private readonly UserManager<TaiKhoan> _userManager;
        private readonly RoleManager<VaiTro> _roleManager;
        private IDbContextTransaction? _transaction;
        #region Repository Properties
        public IAccountManagementRepository AccountManagementRepository { get; private set; } = null!;
        public IIdentityRepository IdentityRepository { get; private set; } = null!;
        public IRefreshTokenRepository RefreshTokenRepository { get; private set; } = null!;
        public IMaOtpRepository MaOtpRepository { get; private set; } = null!;
        public IRoleRepository RoleRepository { get; private set; } = null!;
        public ITaiKhoanQuanLyRepository TaiKhoanQuanLyRepository { get; private set; } = null!;
        public ISignUpRepository SignUpRepository { get; private set; } = null!;

        public IKhachHangRepository KhachHangRepository { get; private set; } = null!;
        public INhanVienRepository NhanVienRepository { get; private set; } = null!;

        public IThuePhongRepository ThuePhongRepository { get; private set; } = null!;
        public IHoaDonRepository HoaDonRepository { get; private set; } = null!;
        public ILichSuSuDungPhongRepository LichSuSuDungPhongRepository { get; private set; } = null!;
        public IChiTietHoaDonDichVuRepository ChiTietHoaDonDichVuRepository { get; private set; } = null!;

        public IPhongHatKaraokeRepository PhongHatRepository { get; private set; } = null!;
        public ILoaiPhongRepository LoaiPhongRepository { get; private set; } = null!;
        public IPhongHatKaraokeRepository PhongHatKaraokeRepository { get; private set; } = null!;

        public ISanPhamDichVuRepository SanPhamDichVuRepository { get; private set; } = null!;
        public IGiaDichVuRepository GiaDichVuRepository { get; private set; } = null!;
        public IVatLieuRepository VatLieuRepository { get; private set; } = null!;
        public IGiaVatLieuRepository GiaVatLieuRepository { get; private set; } = null!;
        public IMonAnRepository MonAnRepository { get; private set; } = null!;

        public ICaLamViecRepository CaLamViecRepository { get; private set; } = null!;
        public ILichLamViecRepository LichLamViecRepository { get; private set; } = null!;
        public ILuongCaLamViecRepository LuongCaLamViecRepository { get; private set; } = null!;
        public IYeuCauChuyenCaRepository YeuCauChuyenCaRepository { get; private set; } = null!;

        public IPhongHatForCustomerViewRepository PhongHatForCustomerViewRepository { get; private set; } = null!;
        #endregion
        public UnitOfWork(QlkaraokeHktContext context, UserManager<TaiKhoan> userManager, RoleManager<VaiTro> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

            InitializeRepositories();
        }
        private void InitializeRepositories()
        {
            #region Auth & Account Management
            IdentityRepository = new IdentityRepository(_userManager);
            RefreshTokenRepository = new RefreshTokenRepository(_context);
            MaOtpRepository = new MaOtpRepository(_context);
            RoleRepository = new RoleRepository(_userManager, _roleManager);
            AccountManagementRepository = new AccountManagementRepository(_userManager);
            TaiKhoanQuanLyRepository = new TaiKhoanQuanLyRepository(IdentityRepository, _userManager);
            SignUpRepository = new SignUpRepository(_context, _userManager);
            #endregion

            #region Customer & Employee
            KhachHangRepository = new KhachHangRepository(_context);
            NhanVienRepository = new NhanVienRepository(_context);
            #endregion

            #region Room 
            LoaiPhongRepository = new LoaiPhongRepository(_context);
            PhongHatKaraokeRepository = new PhongHatKaraokeRepository(_context);
            PhongHatRepository = new PhongHatKaraokeRepository(_context);
            #endregion

            #region Inventory & Pricing 
            SanPhamDichVuRepository = new SanPhamDichVuRepository(_context);
            VatLieuRepository = new VatLieuRepository(_context);
            GiaVatLieuRepository = new GiaVatLieuRepository(_context);
            GiaDichVuRepository = new GiaDichVuRepository(_context);
            MonAnRepository = new MonAnRepository(_context);
            #endregion

            #region HRM
            CaLamViecRepository = new CaLamViecRepository(_context);
            LuongCaLamViecRepository = new LuongCaLamViecRepository(_context);
            LichLamViecRepository = new LichLamViecRepository(_context);
            YeuCauChuyenCaRepository = new YeuCauChuyenCaRepository(_context);

            #endregion

            #region Booking System 
            HoaDonRepository = new HoaDonRepository(_context);
            ChiTietHoaDonDichVuRepository = new ChiTietHoaDonDichVuRepository(_context);
            LichSuSuDungPhongRepository = new LichSuSuDungPhongRepository(_context);
            ThuePhongRepository = new ThuePhongRepository(_context);
            #endregion

            #region Views
            PhongHatForCustomerViewRepository = new PhongHatForCustomerViewRepository(_context);
            #endregion

        }
        #region Transaction Management
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                throw new InvalidOperationException("Transaction đã được bắt đầu.");

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Không có transaction nào để commit.");

            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Không có transaction nào để rollback.");

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task ExecuteTransactionAsync(Func<Task> action)
        {
            await BeginTransactionAsync();
            try
            {
                await action();
                await CommitTransactionAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> action)
        {
            await BeginTransactionAsync();
            try
            {
                var result = await action();
                await CommitTransactionAsync();
                return result;
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
        #endregion
    }
}

