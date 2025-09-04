using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Room;
using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Presentation.Controllers.Room
{
    [Authorize(Roles = ApplicationRole.QuanLyPhongHat)]
    [Route("api/[controller]")]
    [ApiController]
    public class QLPhongHatController : ControllerBase
    {
        private readonly IRoomOrchestrator _roomOrchestrator;
        private readonly ILogger<QLPhongHatController> _logger;

        public QLPhongHatController(IRoomOrchestrator roomOrchestrator, ILogger<QLPhongHatController> logger)
        {
            _roomOrchestrator = roomOrchestrator ?? throw new ArgumentNullException(nameof(roomOrchestrator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("GetAllPhongHat")]
        public async Task<IActionResult> GetAllPhongHat()
        {
            try
            {
                var result = await _roomOrchestrator.GetAllRoomsWorkflowAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống khi lấy danh sách phòng hát");
                return StatusCode(500, ServiceResult.Failure("Lỗi hệ thống khi lấy danh sách phòng hát."));
            }
        }

        [HttpGet("GetPhongHat/{maPhong}")]
        public async Task<IActionResult> GetPhongHat([Required] int maPhong)
        {
            try
            {
                if (maPhong <= 0)
                    return BadRequest(ServiceResult.Failure("Mã phòng không hợp lệ."));

                var result = await _roomOrchestrator.GetRoomDetailWorkflowAsync(maPhong);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống khi lấy thông tin phòng hát {MaPhong}", maPhong);
                return StatusCode(500, ServiceResult.Failure("Lỗi hệ thống khi lấy thông tin phòng hát."));
            }
        }


        [HttpPost("CreatePhongHat")]
        public async Task<IActionResult> CreatePhongHat([FromBody] AddPhongHatDTO addPhongHatDto)
        {
            try
            {
                if (addPhongHatDto == null)
                    return BadRequest(ServiceResult.Failure("Dữ liệu không hợp lệ."));

                if (!ModelState.IsValid)
                    return BadRequest(ServiceResult.Failure("Dữ liệu không hợp lệ.", ModelState));

                var result = await _roomOrchestrator.CreateRoomWorkflowAsync(addPhongHatDto);
                return result.IsSuccess
                    ? CreatedAtAction(nameof(GetPhongHat), new { maPhong = ((PhongHatDetailDTO)result.Data!).MaPhong }, result)
                    : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống khi tạo phòng hát");
                return StatusCode(500, ServiceResult.Failure("Lỗi hệ thống khi tạo phòng hát."));
            }
        }


        [HttpPut("UpdatePhongHat")]
        public async Task<IActionResult> UpdatePhongHat([FromBody] UpdatePhongHatDTO updatePhongHatDto)
        {
            try
            {
                if (updatePhongHatDto == null)
                    return BadRequest(ServiceResult.Failure("Dữ liệu không hợp lệ."));

                if (!ModelState.IsValid)
                    return BadRequest(ServiceResult.Failure("Dữ liệu không hợp lệ.", ModelState));

                var result = await _roomOrchestrator.UpdateRoomWorkflowAsync(updatePhongHatDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống khi cập nhật phòng hát {MaPhong}", updatePhongHatDto?.MaPhong);
                return StatusCode(500, ServiceResult.Failure("Lỗi hệ thống khi cập nhật phòng hát."));
            }
        }


        [HttpPatch("UpdateNgungHoatDong")]
        public async Task<IActionResult> UpdateNgungHoatDong([FromQuery] int maPhong, [FromQuery] bool ngungHoatDong)
        {
            try
            {
                if (maPhong <= 0)
                    return BadRequest(ServiceResult.Failure("Mã phòng không hợp lệ."));

                var result = await _roomOrchestrator.ChangeRoomOperationalStatusWorkflowAsync(maPhong, ngungHoatDong);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống khi cập nhật trạng thái hoạt động phòng {MaPhong}", maPhong);
                return StatusCode(500, ServiceResult.Failure("Lỗi hệ thống khi cập nhật trạng thái hoạt động."));
            }
        }


        [HttpPatch("UpdateDangSuDung")]
        public async Task<IActionResult> UpdateDangSuDung([FromQuery] int maPhong, [FromQuery] bool dangSuDung)
        {
            try
            {
                if (maPhong <= 0)
                    return BadRequest(ServiceResult.Failure("Mã phòng không hợp lệ."));

                var result = await _roomOrchestrator.ChangeRoomOccupancyStatusWorkflowAsync(maPhong, dangSuDung);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống khi cập nhật trạng thái sử dụng phòng {MaPhong}", maPhong);
                return StatusCode(500, ServiceResult.Failure("Lỗi hệ thống khi cập nhật trạng thái sử dụng."));
            }
        }


        [HttpGet("GetPhongHatByLoaiPhong/{maLoaiPhong}")]
        public async Task<IActionResult> GetPhongHatByLoaiPhong([Required] int maLoaiPhong)
        {
            try
            {
                if (maLoaiPhong <= 0)
                    return BadRequest(ServiceResult.Failure("Mã loại phòng không hợp lệ."));

                // ✅ FIX: Sử dụng RoomOrchestrator
                var result = await _roomOrchestrator.GetRoomsByTypeWorkflowAsync(maLoaiPhong);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống khi lấy danh sách phòng hát theo loại {MaLoaiPhong}", maLoaiPhong);
                return StatusCode(500, ServiceResult.Failure("Lỗi hệ thống khi lấy danh sách phòng hát theo loại."));
            }
        }


        [HttpGet("GetPhongHatByStatus")]
        public async Task<IActionResult> GetPhongHatByStatus([FromQuery] RoomStatusFilter statusFilter = RoomStatusFilter.All)
        {
            try
            {
                var result = await _roomOrchestrator.GetRoomsByStatusWorkflowAsync(statusFilter);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống khi lấy danh sách phòng hát theo trạng thái {StatusFilter}", statusFilter);
                return StatusCode(500, ServiceResult.Failure("Lỗi hệ thống khi lấy danh sách phòng hát theo trạng thái."));
            }
        }
    }
}