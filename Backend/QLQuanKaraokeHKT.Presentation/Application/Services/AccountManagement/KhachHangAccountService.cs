using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Application.Repositories.Customer;
using QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement;

namespace QLQuanKaraokeHKT.Application.Services.AccountManagement
{
    public class KhachHangAccountService : IKhachHangAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KhachHangAccountService(IMapper mapper,  IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<ServiceResult> GetAllTaiKhoanKhachHangAsync()
        {
            try
            {   
                var khachHangs = await _unitOfWork.KhachHangRepository.GetAllWithTaiKhoanAsync();
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
