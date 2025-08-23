using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Customer;
using QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement;

namespace QLQuanKaraokeHKT.Application.Services.AccountManagement
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
