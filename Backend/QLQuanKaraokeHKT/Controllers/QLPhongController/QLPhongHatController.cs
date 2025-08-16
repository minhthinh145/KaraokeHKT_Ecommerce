using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Services.QLPhongServices;

namespace QLQuanKaraokeHKT.Controllers.QLPhongController
{
    [Authorize(Roles = ApplicationRole.QuanLyPhongHat)]
    [Route("api/[controller]")]
    [ApiController]
    public class QLPhongHatController : ControllerBase
    {
        private readonly IQLPhongHatService _phongHatService;

        public QLPhongHatController(IQLPhongHatService phongHatService)
        {
            _phongHatService = phongHatService ?? throw new ArgumentNullException(nameof(phongHatService));
        }

        [HttpGet("GetAllPhongHat")]
        public async Task<IActionResult> GetAllPhongHat()
        {
            try
            {
                var result = await _phongHatService.GetAllPhongHatWithDetailsAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách phòng hát.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet("GetPhongHat/{maPhong}")]
        public async Task<IActionResult> GetPhongHat(int maPhong)
        {
            try
            {
                var result = await _phongHatService.GetPhongHatDetailByIdAsync(maPhong);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy thông tin phòng hát.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpPost("CreatePhongHat")]
        public async Task<IActionResult> CreatePhongHat([FromBody] AddPhongHatDTO addPhongHatDto)
        {
            try
            {
                if (addPhongHatDto == null)
                    return BadRequest("Dữ liệu không hợp lệ.");

                var result = await _phongHatService.CreatePhongHatWithFullDetailsAsync(addPhongHatDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi tạo phòng hát.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpPut("UpdatePhongHat")]
        public async Task<IActionResult> UpdatePhongHat([FromBody] UpdatePhongHatDTO updatePhongHatDto)
        {
            try
            {
                if (updatePhongHatDto == null)
                    return BadRequest("Dữ liệu không hợp lệ.");

                var result = await _phongHatService.UpdatePhongHatWithDetailsAsync(updatePhongHatDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi cập nhật phòng hát.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpPatch("UpdateNgungHoatDong")]
        public async Task<IActionResult> UpdateNgungHoatDong([FromQuery] int maPhong, [FromQuery] bool ngungHoatDong)
        {
            try
            {
                var result = await _phongHatService.UpdateNgungHoatDongAsync(maPhong, ngungHoatDong);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi cập nhật trạng thái hoạt động.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpPatch("UpdateDangSuDung")]
        public async Task<IActionResult> UpdateDangSuDung([FromQuery] int maPhong, [FromQuery] bool dangSuDung)
        {
            try
            {
                var result = await _phongHatService.UpdateDangSuDungAsync(maPhong, dangSuDung);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi cập nhật trạng thái sử dụng.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet("GetPhongHatByLoaiPhong/{maLoaiPhong}")]
        public async Task<IActionResult> GetPhongHatByLoaiPhong(int maLoaiPhong)
        {
            try
            {
                var result = await _phongHatService.GetPhongHatByLoaiPhongAsync(maLoaiPhong);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách phòng hát theo loại.",
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}