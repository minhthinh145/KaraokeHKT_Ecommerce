using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Customer;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Customer;

namespace QLQuanKaraokeHKT.Application.Services.Customer
{
    public class KhachHangService : IKhachHangService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KhachHangService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResult> UpdateKhachHangByTaiKhoanAsync(TaiKhoan TaiKhoankhachHang)
        {
            if (TaiKhoankhachHang == null)
            {
                return ServiceResult.Failure("Không tìm thấy khách hàng với ID đã cho.");
            }
            var khachHang = await _unitOfWork.KhachHangRepository.GetByAccountIdAsync(TaiKhoankhachHang.Id);
            var khachHangMap = _mapper.Map<TaiKhoan, KhachHang>(TaiKhoankhachHang, khachHang);

            var isUpdated = await _unitOfWork.KhachHangRepository.UpdateAsync(khachHang);
            if (!isUpdated)
            {
                return ServiceResult.Failure("Cập nhật khách hàng không thành công.");
            }
            return ServiceResult.Success("Cập nhật khách hàng thành công.", _mapper.Map<KhachHangDTO>(khachHang));
        }


    }
}
