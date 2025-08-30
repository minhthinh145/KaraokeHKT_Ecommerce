using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Customer;
using QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement;

namespace QLQuanKaraokeHKT.Application.Services.AccountManagement
{
    public class KhachHangAccountService : IKhachHangAccountService
    {
        private readonly IKhachHangRepository _khachHangRepository;
        private readonly IMapper _mapper;

        public KhachHangAccountService(IKhachHangRepository khacHangRepository,IMapper mapper,  IUnitOfWork unitOfWork)
        {
            _khachHangRepository = khacHangRepository;
            _mapper = mapper;
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
