using AutoMapper;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.BookingDTOs;
using QLQuanKaraokeHKT.Core.Entities.Views;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Common;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Room;
using QLQuanKaraokeHKT.Shared.Models;

namespace QLQuanKaraokeHKT.Application.Services.Room
{
    public class RoomQueryService : IRoomQueryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPricingService _pricingService;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomQueryService> _logger;

        public RoomQueryService(
            IUnitOfWork unitOfWork,
            IPricingService pricingService,
            IMapper mapper,
            ILogger<RoomQueryService> logger)
        {
            _unitOfWork = unitOfWork;
            _pricingService = pricingService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResult> GetAllRoomsAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var roomViews = await _unitOfWork.PhongHatForCustomerViewRepository.GetCustomerRoomsAsync(pageIndex, pageSize);

                var customerDTOs = await TransformToCustomerDTOsAsync(roomViews.Items);

                var result = new Pagination<PhongHatForCustomerDTO>(
                    customerDTOs, 
                    roomViews.TotalCount, 
                    pageIndex, 
                    pageSize
                );

                return ServiceResult.Success("Lấy danh sách phòng hát thành công", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phòng hát");
                return ServiceResult.Failure("Không thể lấy danh sách phòng hát");
            }
        }

        public async Task<ServiceResult> GetRoomByIdAsync(int roomId)
        {
            try
            {
                var roomView = await _unitOfWork.PhongHatForCustomerViewRepository.GetCustomerRoomByIdAsync(roomId);

                if (roomView == null)
                    return ServiceResult.Failure("Không tìm thấy phòng hát");

                var customerDTOs = await TransformToCustomerDTOsAsync(new List<PhongHatForCustomerView> { roomView });
                
                return ServiceResult.Success("Lấy thông tin phòng hát thành công", customerDTOs.FirstOrDefault());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin phòng hát {RoomId}", roomId);
                return ServiceResult.Failure("Không thể lấy thông tin phòng hát");
            }
        }

        #region Private Helper Methods

        private async Task<List<PhongHatForCustomerDTO>> TransformToCustomerDTOsAsync(
            List<PhongHatForCustomerView> roomViews)
        {
            var customerDTOs = new List<PhongHatForCustomerDTO>();

            foreach (var roomView in roomViews)
            {
                var dto = _mapper.Map<PhongHatForCustomerDTO>(roomView);

                if (!dto.CoGiaTheoCa)
                {
                    var pricingInfo = await _pricingService.GetCustomerPricingInfoAsync(roomView.MaSanPham);
                    dto.GiaThueHienTai = pricingInfo.GiaThueHienTai;
                }

                customerDTOs.Add(dto);
            }

            return customerDTOs;
        }

        #endregion
    }
}