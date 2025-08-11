using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Services.QLNhanSuServices.QLCaLamViecServices;

namespace QLQuanKaraokeHKT.Controllers.QLNhanSuController
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class QLCaLamViecController : ControllerBase
    {
        private readonly IQLCaLamViecService _service;

        public QLCaLamViecController(IQLCaLamViecService service)
        {
            _service = service;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateCaLamViecAsync([FromBody] AddCaLamViecDTO addCaLamViecDto)
        {
            try
            {
                if (addCaLamViecDto == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }
                var result = await _service.CreateCaLamViecAsync(addCaLamViecDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi tạo ca làm việc.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllCaLamViecsAsync()
        {
            try
            {
                var result = await _service.GetAllCaLamViecsAsync();
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách ca làm việc.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet("getbyid/{maCa}")]
        public async Task<IActionResult> GetCaLamViecByIdAsync(int maCa)
        {
            try
            {
                var result = await _service.GetCaLamViecByIdAsync(maCa);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy ca làm việc.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        
    }
}
