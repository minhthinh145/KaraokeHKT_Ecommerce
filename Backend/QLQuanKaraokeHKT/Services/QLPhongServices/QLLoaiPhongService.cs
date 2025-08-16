using AutoMapper;
using QLQuanKaraokeHKT.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLPhong.Interfaces;

namespace QLQuanKaraokeHKT.Services.QLPhongServices
{
    public class QLLoaiPhongService : IQLLoaiPhongService
    {
        private readonly ILoaiPhongRepository _loaiPhongRepository;
        private readonly IMapper _mapper;

        public QLLoaiPhongService(ILoaiPhongRepository loaiPhongRepository, IMapper mapper)
        {
            _loaiPhongRepository = loaiPhongRepository ?? throw new ArgumentNullException(nameof(loaiPhongRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResult> GetAllLoaiPhongAsync()
        {
            try
            {
                var loaiPhongs = await _loaiPhongRepository.GetAllLoaiPhongAsync();
                if (loaiPhongs == null || !loaiPhongs.Any())
                    return ServiceResult.Failure("Không có loại phòng nào trong hệ thống.");

                var loaiPhongDTOs = _mapper.Map<List<LoaiPhongDTO>>(loaiPhongs);
                return ServiceResult.Success("Lấy danh sách loại phòng thành công.", loaiPhongDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy danh sách loại phòng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetLoaiPhongByIdAsync(int maLoaiPhong)
        {
            try
            {
                var loaiPhong = await _loaiPhongRepository.GetLoaiPhongByIdAsync(maLoaiPhong);
                if (loaiPhong == null)
                    return ServiceResult.Failure("Không tìm thấy loại phòng.");

                var loaiPhongDTO = _mapper.Map<LoaiPhongDTO>(loaiPhong);
                return ServiceResult.Success("Lấy thông tin loại phòng thành công.", loaiPhongDTO);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi lấy thông tin loại phòng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> CreateLoaiPhongAsync(AddLoaiPhongDTO addLoaiPhongDto)
        {
            try
            {
                if (addLoaiPhongDto == null)
                    return ServiceResult.Failure("Thông tin loại phòng không hợp lệ.");

                var loaiPhong = _mapper.Map<LoaiPhongHatKaraoke>(addLoaiPhongDto);
                var createdLoaiPhong = await _loaiPhongRepository.CreateLoaiPhongAsync(loaiPhong);

                var result = _mapper.Map<LoaiPhongDTO>(createdLoaiPhong);
                return ServiceResult.Success("Tạo loại phòng thành công.", result);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi tạo loại phòng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateLoaiPhongAsync(LoaiPhongDTO loaiPhongDto)
        {
            try
            {
                if (loaiPhongDto == null)
                    return ServiceResult.Failure("Thông tin cập nhật không hợp lệ.");

                var loaiPhong = _mapper.Map<LoaiPhongHatKaraoke>(loaiPhongDto);
                var result = await _loaiPhongRepository.UpdateLoaiPhongAsync(loaiPhong);

                if (!result)
                    return ServiceResult.Failure("Cập nhật loại phòng thất bại.");

                return ServiceResult.Success("Cập nhật loại phòng thành công.", loaiPhong);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi cập nhật loại phòng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteLoaiPhongAsync(int maLoaiPhong)
        {
            try
            {
                // Kiểm tra xem có phòng nào đang sử dụng loại phòng này không
                var hasPhong = await _loaiPhongRepository.HasPhongHatKaraokeAsync(maLoaiPhong);
                if (hasPhong)
                    return ServiceResult.Failure("Không thể xóa loại phòng đang được sử dụng.");

                var result = await _loaiPhongRepository.DeleteLoaiPhongAsync(maLoaiPhong);
                if (!result)
                    return ServiceResult.Failure("Xóa loại phòng thất bại.");

                return ServiceResult.Success("Xóa loại phòng thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi khi xóa loại phòng: {ex.Message}");
            }
        }
    }
}