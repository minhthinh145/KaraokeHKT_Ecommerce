using AutoMapper;
using QLQuanKaraokeHKT.DTOs;
using QLQuanKaraokeHKT.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.Interfaces;
using QLQuanKaraokeHKT.Services.Interfaces;

namespace QLQuanKaraokeHKT.Services.Implementation
{
    public class KhachHangService : IKhachHangService
    {
        private readonly IKhacHangRepository _repo;
        private readonly IMapper _mapper;

        public KhachHangService(IKhacHangRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
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
            var isCreated = await _repo.CreateKhacHangAsync(khachHang);
            if (isCreated == null)
            {
                return ServiceResult.Failure("Tạo khách hàng không thành công.");
            }
            
            var khachHangDTO = _mapper.Map<KhachHangDTO>(khachHang);
            return ServiceResult.Success("Tạo khách hàng thành công.", khachHangDTO);
        }

        public async Task<ServiceResult> GetAllKhacHangAsync()
        {
           var khachHangs = await _repo.GetAllAsync();
            if (khachHangs == null || !khachHangs.Any())
            {
                return ServiceResult.Failure("Không tìm thấy khách hàng nào.");
            }
            var khachHangDTOs = _mapper.Map<IList<KhachHangDTO>>(khachHangs);
            return ServiceResult.Success("Có danh sách khách hàng",khachHangDTOs);
        }

        public async Task<ServiceResult> GetKhachHangByIdAsync(Guid maKhachHang)
        {
            var khachHang = await _repo.GetByIdAsync(maKhachHang);
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
            var khachHang = await _repo.GetByAccountIdAsync(TaiKhoankhachHang.Id);
            var khachHangMap = _mapper.Map<TaiKhoan, KhachHang>(TaiKhoankhachHang, khachHang);

            var isUpdated = await _repo.UpdateAsync(khachHang);
            if (!isUpdated)
            {
                return ServiceResult.Failure("Cập nhật khách hàng không thành công.");
            }
            return ServiceResult.Success("Cập nhật khách hàng thành công.", _mapper.Map<KhachHangDTO>(khachHang));
        }


    }
}
