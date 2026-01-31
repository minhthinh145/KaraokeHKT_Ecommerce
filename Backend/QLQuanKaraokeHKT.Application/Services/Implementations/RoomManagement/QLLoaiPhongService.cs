using AutoMapper;
using QLQuanKaraokeHKT.Application.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Services.Interfaces.Room;

namespace QLQuanKaraokeHKT.Application.Services.Implementations.Room
{
    public class QLLoaiPhongService : IQLLoaiPhongService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<QLLoaiPhongService> _logger;

        public QLLoaiPhongService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<QLLoaiPhongService> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult> GetAllLoaiPhongAsync()
        {
            try
            {
                _logger.LogInformation("Lấy danh sách tất cả loại phòng");

                var loaiPhongs = await _unitOfWork.LoaiPhongRepository.GetAllAsync();
                if (loaiPhongs == null || !loaiPhongs.Any())
                    return ServiceResult.Failure("Không có loại phòng nào trong hệ thống.");

                var loaiPhongDTOs = _mapper.Map<List<LoaiPhongDTO>>(loaiPhongs);
                return ServiceResult.Success("Lấy danh sách loại phòng thành công.", loaiPhongDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách loại phòng");
                return ServiceResult.Failure($"Lỗi khi lấy danh sách loại phòng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetLoaiPhongByIdAsync(int maLoaiPhong)
        {
            try
            {
                _logger.LogInformation("Lấy thông tin loại phòng: {MaLoaiPhong}", maLoaiPhong);

                var loaiPhong = await _unitOfWork.LoaiPhongRepository.GetByIdAsync(maLoaiPhong);
                if (loaiPhong == null)
                    return ServiceResult.Failure("Không tìm thấy loại phòng.");

                var loaiPhongDTO = _mapper.Map<LoaiPhongDTO>(loaiPhong);
                return ServiceResult.Success("Lấy thông tin loại phòng thành công.", loaiPhongDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin loại phòng {MaLoaiPhong}", maLoaiPhong);
                return ServiceResult.Failure($"Lỗi khi lấy thông tin loại phòng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> CreateLoaiPhongAsync(AddLoaiPhongDTO addLoaiPhongDto)
        {
            try
            {
                if (addLoaiPhongDto == null)
                    return ServiceResult.Failure("Thông tin loại phòng không hợp lệ.");

                _logger.LogInformation("Tạo loại phòng mới: {TenLoaiPhong}", addLoaiPhongDto.TenLoaiPhong);

                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    var loaiPhong = _mapper.Map<LoaiPhongHatKaraoke>(addLoaiPhongDto);
                    var createdLoaiPhong = await _unitOfWork.LoaiPhongRepository.CreateAsync(loaiPhong);

                    var result = _mapper.Map<LoaiPhongDTO>(createdLoaiPhong);

                    _logger.LogInformation("Tạo loại phòng thành công: {TenLoaiPhong}, ID: {MaLoaiPhong}",
                        addLoaiPhongDto.TenLoaiPhong, createdLoaiPhong.MaLoaiPhong);

                    return ServiceResult.Success("Tạo loại phòng thành công.", result);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo loại phòng: {TenLoaiPhong}", addLoaiPhongDto?.TenLoaiPhong);
                return ServiceResult.Failure($"Lỗi khi tạo loại phòng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateLoaiPhongAsync(LoaiPhongDTO loaiPhongDto)
        {
            try
            {
                if (loaiPhongDto == null)
                    return ServiceResult.Failure("Thông tin cập nhật không hợp lệ.");

                _logger.LogInformation("Cập nhật loại phòng: {MaLoaiPhong}", loaiPhongDto.MaLoaiPhong);

                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    var existingLoaiPhong = await _unitOfWork.LoaiPhongRepository.GetByIdAsync(loaiPhongDto.MaLoaiPhong);
                    if (existingLoaiPhong == null)
                        return ServiceResult.Failure("Không tìm thấy loại phòng cần cập nhật.");

                    _mapper.Map(loaiPhongDto, existingLoaiPhong);
                    var result = await _unitOfWork.LoaiPhongRepository.UpdateAsync(existingLoaiPhong);

                    if (!result)
                        return ServiceResult.Failure("Cập nhật loại phòng thất bại.");

                    var updatedDto = _mapper.Map<LoaiPhongDTO>(existingLoaiPhong);

                    _logger.LogInformation("Cập nhật loại phòng thành công: {MaLoaiPhong}", loaiPhongDto.MaLoaiPhong);

                    return ServiceResult.Success("Cập nhật loại phòng thành công.", updatedDto);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật loại phòng: {MaLoaiPhong}", loaiPhongDto?.MaLoaiPhong);
                return ServiceResult.Failure($"Lỗi khi cập nhật loại phòng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteLoaiPhongAsync(int maLoaiPhong)
        {
            try
            {
                _logger.LogInformation("Xóa loại phòng: {MaLoaiPhong}", maLoaiPhong);

                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {

                    var hasPhong = await _unitOfWork.LoaiPhongRepository.HasPhongHatKaraokeAsync(maLoaiPhong);
                    if (hasPhong)
                        return ServiceResult.Failure("Không thể xóa loại phòng đang được sử dụng.");

                    var result = await _unitOfWork.LoaiPhongRepository.DeleteAsync(maLoaiPhong);
                    if (!result)
                        return ServiceResult.Failure("Xóa loại phòng thất bại.");

                    _logger.LogInformation("Xóa loại phòng thành công: {MaLoaiPhong}", maLoaiPhong);

                    return ServiceResult.Success("Xóa loại phòng thành công.");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa loại phòng: {MaLoaiPhong}", maLoaiPhong);
                return ServiceResult.Failure($"Lỗi khi xóa loại phòng: {ex.Message}");
            }
        }
    }
}