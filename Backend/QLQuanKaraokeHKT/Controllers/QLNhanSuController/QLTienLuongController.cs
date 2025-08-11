using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Services.QLNhanSuServices.QLTienLuongServices;

namespace QLQuanKaraokeHKT.Controllers.QLNhanSuController
{
    [Route("api/[controller]")]
    [ApiController]
    public class QLTienLuongController : ControllerBase
    {
        private readonly IQuanLyTienLuongService _service;

        public QLTienLuongController(IQuanLyTienLuongService service)
        {
            _service = service;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllLuongCaLamViecsAsync()
        {
            try
            {
                var result = await _service.GetAllLuongCaLamViecsAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách lương ca làm việc.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet("getbyid/{maLuongCaLamViec}")]
        public async Task<IActionResult> GetLuongCaLamViecByIdAsync(int maLuongCaLamViec)
        {
            try
            {
                var result = await _service.GetLuongCaLamViecByIdAsync(maLuongCaLamViec);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy lương ca làm việc.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLuongCaLamViecAsync([FromBody] AddLuongCaLamViecDTO addLuongCaLamViecDto)
        {
            try
            {
                if (addLuongCaLamViecDto == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }
                var result = await _service.CreateLuongCaLamViecAsync(addLuongCaLamViecDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi tạo lương ca làm việc.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpDelete("delete/{maLuongCaLamViec}")]
        public async Task<IActionResult> DeleteLuongCaLamViecAsync(int maLuongCaLamViec)
        {
            try
            {
                var result = await _service.DeleteLuongCaLamViecAsync(maLuongCaLamViec);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi xóa lương ca làm việc.",
                    success = false,
                    error = ex.Message
                });
            }

        }
    }
}
