using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Common;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Room;

namespace QLQuanKaraokeHKT.Application.Services.Room
{
    public class RoomOrchestrator : IRoomOrchestrator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPricingService _pricingService;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomOrchestrator> _logger;

        public RoomOrchestrator(
            IUnitOfWork unitOfWork,
            IPricingService pricingService,
            IMapper mapper,
            ILogger<RoomOrchestrator> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult> CreateRoomWorkflowAsync(AddPhongHatDTO request)
        {
            try
            {
                if (request == null)
                    return ServiceResult.Failure("Thông tin phòng hát không hợp lệ.");

                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    _logger.LogInformation("Bắt đầu tạo phòng hát mới: {TenPhong}", request.TenPhong);

                    var sanPham = _mapper.Map<SanPhamDichVu>(request);
                    var createdSanPham = await _unitOfWork.SanPhamDichVuRepository.CreateAsync(sanPham);
                    
                    var phongHat = _mapper.Map<PhongHatKaraoke>(request);
                    phongHat.MaSanPham = createdSanPham.MaSanPham;
                    phongHat.DangSuDung = false;
                    phongHat.NgungHoatDong = false;
                    var createdPhongHat = await _unitOfWork.PhongHatKaraokeRepository.CreateAsync(phongHat);

                    await _pricingService.ApplyPricingConfigAsync(createdSanPham.MaSanPham, request.AsPricingConfig());

                    var roomWithDetails = await _unitOfWork.PhongHatKaraokeRepository.GetByIdWithDetailsAsync(createdPhongHat.MaPhong, true);
                    var result = _mapper.Map<PhongHatDetailDTO>(roomWithDetails);

                    _logger.LogInformation("Tạo phòng hát thành công: {TenPhong}, MaPhong: {MaPhong}", 
                        request.TenPhong, createdPhongHat.MaPhong);

                    return ServiceResult.Success("Tạo phòng hát thành công với đầy đủ thông tin.", result);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo phòng hát: {Message}", ex.Message);
                return ServiceResult.Failure($"Lỗi khi tạo phòng hát: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateRoomWorkflowAsync(UpdatePhongHatDTO request)
        {
            try
            {
                if (request == null)
                    return ServiceResult.Failure("Thông tin cập nhật phòng hát không hợp lệ.");

                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    _logger.LogInformation("Bắt đầu cập nhật phòng hát: MaPhong {MaPhong}", request.MaPhong);

                    var phongHat = await _unitOfWork.PhongHatKaraokeRepository.GetByIdForUpdateAsync(request.MaPhong);
                    if (phongHat == null)
                        return ServiceResult.Failure("Không tìm thấy phòng hát cần cập nhật.");

                    _mapper.Map(request, phongHat);
                    await _unitOfWork.PhongHatKaraokeRepository.UpdateAsync(phongHat);

                    if (HasSanPhamChanges(request))
                    {
                        var sanPham = await _unitOfWork.SanPhamDichVuRepository.GetByIdAsync(phongHat.MaSanPham);
                        if (sanPham != null)
                        {
                            _mapper.Map(request, sanPham);
                            await _unitOfWork.SanPhamDichVuRepository.UpdateAsync(sanPham);
                        }
                    }

                    if (request.CapNhatGiaThue && request.DongGiaAllCa.HasValue)
                    {
                        await _pricingService.DisableCurrentPricesAsync(phongHat.MaSanPham);
                        await _pricingService.ApplyPricingConfigAsync(phongHat.MaSanPham, request.AsPricingConfig());
                    }

                    var updatedRoom = await _unitOfWork.PhongHatKaraokeRepository.GetByIdWithDetailsAsync(request.MaPhong, true);
                    var result = _mapper.Map<PhongHatDetailDTO>(updatedRoom);

                    _logger.LogInformation("Cập nhật phòng hát thành công: MaPhong {MaPhong}", request.MaPhong);
                    return ServiceResult.Success("Cập nhật phòng hát thành công với đầy đủ thông tin.", result);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật phòng hát: {Message}", ex.Message);
                return ServiceResult.Failure($"Lỗi khi cập nhật phòng hát: {ex.Message}");
            }
        }

        public async Task<ServiceResult> ChangeRoomOperationalStatusWorkflowAsync(int maPhong, bool ngungHoatDong)
        {
            try
            {
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    _logger.LogInformation("Cập nhật trạng thái hoạt động phòng: MaPhong {MaPhong}, NgungHoatDong: {NgungHoatDong}", 
                        maPhong, ngungHoatDong);

                    var phongHat = await _unitOfWork.PhongHatKaraokeRepository.GetByIdForUpdateAsync(maPhong);
                    if (phongHat == null)
                        return ServiceResult.Failure("Không tìm thấy phòng hát.");

                    if (ngungHoatDong && phongHat.DangSuDung)
                    {
                        return ServiceResult.Failure("Không thể ngừng hoạt động phòng đang có khách sử dụng.");
                    }

                    phongHat.NgungHoatDong = ngungHoatDong;
                    if (ngungHoatDong)
                    {
                        phongHat.DangSuDung = false;
                    }

                    await _unitOfWork.PhongHatKaraokeRepository.UpdateAsync(phongHat);

                    var message = ngungHoatDong
                        ? "Phòng hát đã được đánh dấu ngừng hoạt động."
                        : "Phòng hát đã được đánh dấu hoạt động trở lại.";

                    return ServiceResult.Success(message);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái phòng hát: {Message}", ex.Message);
                return ServiceResult.Failure($"Lỗi khi cập nhật trạng thái phòng hát: {ex.Message}");
            }
        }

        public async Task<ServiceResult> ChangeRoomOccupancyStatusWorkflowAsync(int maPhong, bool dangSuDung)
        {
            try
            {
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    _logger.LogInformation("Cập nhật trạng thái sử dụng phòng: MaPhong {MaPhong}, DangSuDung: {DangSuDung}", 
                        maPhong, dangSuDung);

                    var phongHat = await _unitOfWork.PhongHatKaraokeRepository.GetByIdForUpdateAsync(maPhong);
                    if (phongHat == null)
                        return ServiceResult.Failure("Không tìm thấy phòng hát.");

                    if (dangSuDung && phongHat.NgungHoatDong)
                    {
                        return ServiceResult.Failure("Không thể đánh dấu sử dụng phòng đang ngừng hoạt động.");
                    }

                    phongHat.DangSuDung = dangSuDung;
                    await _unitOfWork.PhongHatKaraokeRepository.UpdateAsync(phongHat);

                    var message = dangSuDung
                        ? "Phòng đã được đánh dấu đang sử dụng."
                        : "Phòng đã được đánh dấu không sử dụng.";

                    return ServiceResult.Success(message);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái sử dụng: {Message}", ex.Message);
                return ServiceResult.Failure($"Lỗi khi cập nhật trạng thái sử dụng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetAllRoomsWorkflowAsync()
        {
            try
            {
                _logger.LogInformation("Lấy danh sách tất cả phòng hát");

                var phongHats = await _unitOfWork.PhongHatKaraokeRepository.GetAllWithDetailsAsync(includePricing: true);

                if (!phongHats.Any())
                    return ServiceResult.Failure("Không có phòng hát nào trong hệ thống.");

                var phongHatDTOs = _mapper.Map<List<PhongHatDetailDTO>>(phongHats);

                _logger.LogInformation("Hoàn thành mapping cho {Count} phòng hát với pricing real-time", phongHatDTOs.Count);
                return ServiceResult.Success("Lấy danh sách phòng hát thành công.", phongHatDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phòng hát: {Message}", ex.Message);
                return ServiceResult.Failure($"Lỗi khi lấy danh sách phòng hát: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetRoomDetailWorkflowAsync(int maPhong)
        {
            try
            {
                _logger.LogInformation("Lấy chi tiết phòng hát: MaPhong {MaPhong}", maPhong);
                var phongHat = await _unitOfWork.PhongHatKaraokeRepository.GetByIdWithDetailsAsync(maPhong, true);
                
                if (phongHat == null)
                    return ServiceResult.Failure("Không tìm thấy phòng hát.");

                var phongHatDTO = _mapper.Map<PhongHatDetailDTO>(phongHat);
                return ServiceResult.Success("Lấy thông tin phòng hát thành công.", phongHatDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin phòng hát: {Message}", ex.Message);
                return ServiceResult.Failure($"Lỗi khi lấy thông tin phòng hát: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetRoomsByTypeWorkflowAsync(int maLoaiPhong)
        {
            try
            {
                _logger.LogInformation("Lấy danh sách phòng hát theo loại: MaLoaiPhong {MaLoaiPhong}", maLoaiPhong);
                var phongHats = await _unitOfWork.PhongHatKaraokeRepository.GetByLoaiPhongAsync(maLoaiPhong, true);
                
                if (!phongHats.Any())
                    return ServiceResult.Failure("Không có phòng hát nào thuộc loại phòng này.");

                var phongHatDTOs = _mapper.Map<List<PhongHatDetailDTO>>(phongHats);
                return ServiceResult.Success("Lấy danh sách phòng hát theo loại thành công.", phongHatDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phòng hát theo loại: {Message}", ex.Message);
                return ServiceResult.Failure($"Lỗi khi lấy danh sách phòng hát theo loại: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetRoomsByStatusWorkflowAsync(RoomStatusFilter statusFilter)
        {
            try
            {
                _logger.LogInformation("Lấy danh sách phòng hát theo trạng thái: {Status}", statusFilter);
                
                List<PhongHatKaraoke> phongHats = statusFilter switch
                {
                    RoomStatusFilter.Available => await _unitOfWork.PhongHatKaraokeRepository.GetAvailableAsync(),
                    RoomStatusFilter.Occupied => await _unitOfWork.PhongHatKaraokeRepository.GetOccupiedAsync(),
                    RoomStatusFilter.OutOfService => await _unitOfWork.PhongHatKaraokeRepository.GetOutOfServiceAsync(),
                    _ => await _unitOfWork.PhongHatKaraokeRepository.GetAllWithDetailsAsync()
                };

                if (!phongHats.Any())
                {
                    var statusName = GetStatusDisplayName(statusFilter);
                    return ServiceResult.Failure($"Không tìm thấy phòng hát nào{statusName}.");
                }

                var phongHatDTOs = _mapper.Map<List<PhongHatDetailDTO>>(phongHats);
                return ServiceResult.Success("Lấy danh sách phòng hát thành công.", phongHatDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phòng hát theo trạng thái: {Message}", ex.Message);
                return ServiceResult.Failure($"Lỗi khi lấy danh sách phòng hát theo trạng thái: {ex.Message}");
            }
        }

        #region Helper Methods

        private bool HasSanPhamChanges(UpdatePhongHatDTO dto)
        {
            return !string.IsNullOrEmpty(dto.TenPhong) || !string.IsNullOrEmpty(dto.HinhAnhPhong);
        }

        private string GetStatusDisplayName(RoomStatusFilter filter)
        {
            return filter switch
            {
                RoomStatusFilter.Available => " sẵn sàng đặt phòng",
                RoomStatusFilter.Occupied => " đang có khách",
                RoomStatusFilter.OutOfService => " ngừng hoạt động",
                _ => ""
            };
        }
        #endregion
    }
}
