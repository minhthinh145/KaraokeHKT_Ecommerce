using AutoMapper;
using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Repositories.Interfaces;
using QLQuanKaraokeHKT.Repositories.TaiKhoanRepo;
using QLQuanKaraokeHKT.Services.QLHeThongServices.Interface;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices.Implementation
{
    public class KhachHangAccountService : IKhachHangAccountService
    {
        private readonly IKhacHangRepository _khachHangRepository;
        private readonly IMapper _mapper;
        private readonly ITaiKhoanRepository _taiKhoanRepository;

        public KhachHangAccountService(IKhacHangRepository khacHangRepository,IMapper mapper, ITaiKhoanRepository taiKhoanRepository)
        {
            _khachHangRepository = khacHangRepository;
            _mapper = mapper;
            _taiKhoanRepository = taiKhoanRepository ?? throw new ArgumentNullException(nameof(taiKhoanRepository));
        }


        public async Task<ServiceResult> GetAllTaiKhoanKhachHangAsync()
        {
            try
            {   
                var khachHangs = await _khachHangRepository.GetAllWithTaiKhoanAsync();
                if (khachHangs == null || !khachHangs.Any())
                {
                    return ServiceResult.Failure("Không có khách hàng nào trong hệ thống.");
                }

                var khachHangTaiKhoanDTOs = _mapper.Map<IEnumerable<KhachHangTaiKhoanDTO>>(khachHangs);
                return ServiceResult.Success("Lấy danh sách tài khoản khách hàng thành công.", khachHangTaiKhoanDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi lấy danh sách tài khoản khách hàng: {ex.Message}");
            }
        }

    }
}
