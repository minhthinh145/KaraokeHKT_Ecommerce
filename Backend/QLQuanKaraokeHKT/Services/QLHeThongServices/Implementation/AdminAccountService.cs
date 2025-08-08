using AutoMapper;
using QLQuanKaraokeHKT.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.TaiKhoanRepo;
using QLQuanKaraokeHKT.Services.QLHeThongServices.Interface;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices.Implementation
{
    public class AdminAccountService : IAdminAccountService
    {
        private readonly ITaiKhoanRepository _taiKhoanRepository;
        private readonly ITaiKhoanQuanLyRepository _taiKhoanQuanLyRepository;
        private readonly IMapper _mapper;
        private readonly IAccountBaseService _accountBaseService;

        public AdminAccountService(ITaiKhoanRepository taiKhoanRepository, IMapper mapper, ITaiKhoanQuanLyRepository taiKhoanQuanLyRepository, IAccountBaseService accountBaseService)
        {
            _taiKhoanRepository = taiKhoanRepository;
            _taiKhoanQuanLyRepository = taiKhoanQuanLyRepository;
            _mapper = mapper;
            _accountBaseService = accountBaseService;
        }

        public async Task<ServiceResult> GetAllAdminAccountAsync()
        {
            {
                var taiKhoanQuanLyList = await _taiKhoanQuanLyRepository.GettAllAdminAccount();
                if (taiKhoanQuanLyList == null || !taiKhoanQuanLyList.Any())
                {
                    return ServiceResult.Failure("Không có tài khoản quản lý nào trong hệ thống.");
                }
                var taiKhoanQuanLyDTOs = _mapper.Map<IEnumerable<TaiKhoanQuanLyDTO>>(taiKhoanQuanLyList);
                return ServiceResult.Success("Lấy danh sách tài khoản quản lý thành công.", taiKhoanQuanLyDTOs);

            }
        }

        public async Task<ServiceResult> AddAdminAccountAsync(AddAccountForAdminDTO adminDTO)
        {
            if (adminDTO == null)
            {
                return ServiceResult.Failure("Thông tin tài khoản quản lý không hợp lệ.");
            }
            var TaiKhoanQuanLy = _mapper.Map<TaiKhoan>(adminDTO);
            // Activate account
            TaiKhoanQuanLy.daKichHoat = true;
            TaiKhoanQuanLy.EmailConfirmed = true;
            TaiKhoanQuanLy.daBiKhoa = false;
            string password = PasswordHelper.GenerateAdminPassword(TaiKhoanQuanLy.loaiTaiKhoan);

            var result = await _taiKhoanRepository.CreateUserAsync(TaiKhoanQuanLy, password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return ServiceResult.Failure("Tạo tài khoản quản lý thất bại.", errors);
            }

            // Gắn role cho tài khoản quản lý
            await _taiKhoanRepository.AddToRoleAsync(TaiKhoanQuanLy, TaiKhoanQuanLy.loaiTaiKhoan);

            return ServiceResult.Success("Tạo tài khoản quản lý thành công.");
        }

     
    }
}
