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
        public async Task<ServiceResult> CreateKhachHangByDangKyAsync(SignUpDTO signUp)
        {
           var applicationUser = _mapper.Map<TaiKhoan>(signUp);
            if (applicationUser == null)
            {
                return ServiceResult.Failure("Không thể tạo khách hàng từ thông tin đăng ký.");
            }
            var khachHang = _mapper.Map<KhachHang>(applicationUser);
            khachHang.MaKhachHang = Guid.NewGuid(); // Tạo mã khách hàng duy nhất
            var isCreated = await _unitOfWork.KhachHangRepository.CreateKhacHangAsync(khachHang);
            if (isCreated == null)
            {
                return ServiceResult.Failure("Tạo khách hàng không thành công.");
            }
            
            var khachHangDTO = _mapper.Map<KhachHangDTO>(khachHang);
            return ServiceResult.Success("Tạo khách hàng thành công.", khachHangDTO);
        }

        public async Task<ServiceResult> GetAllKhacHangAsync()
        {
           var khachHangs = await _unitOfWork.KhachHangRepository.GetAllAsync();
            if (khachHangs == null || !khachHangs.Any())
            {
                return ServiceResult.Failure("Không tìm thấy khách hàng nào.");
            }
            var khachHangDTOs = _mapper.Map<IList<KhachHangDTO>>(khachHangs);
            return ServiceResult.Success("Có danh sách khách hàng",khachHangDTOs);
        }

        public async Task<ServiceResult> GetKhachHangByIdAsync(Guid maKhachHang)
        {
            var khachHang = await _unitOfWork.KhachHangRepository.GetByIdAsync(maKhachHang);
            if (khachHang == null)
            {
                return ServiceResult.Failure("Không tìm thấy khách hàng với ID đã cho.");
            }
            var khachHangDTO = _mapper.Map<KhachHangDTO>(khachHang);
            return ServiceResult.Success("Thông tin khách hàng", khachHangDTO);
        }

        public async Task<ServiceResult> UpdateKhachHangByTaiKhoanAsync(TaiKhoan TaiKhoankhachHang)
        {
            if (TaiKhoankhachHang == null)
            {
                return ServiceResult.Failure("Không tìm thấy khách hàng với ID đã cho.");
            }
            //map tài khoản vs khách hàng
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
