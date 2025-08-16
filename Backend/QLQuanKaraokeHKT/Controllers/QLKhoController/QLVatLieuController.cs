using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.DTOs.QLKhoDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Services.QLKhoServices;

namespace QLQuanKaraokeHKT.Controllers.QLKhoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class QLVatLieuController : ControllerBase
    {
        private readonly IQLVatLieuService _vatLieuService;

        public QLVatLieuController(IQLVatLieuService vatLieuService)
        {
            _vatLieuService = vatLieuService ?? throw new ArgumentNullException(nameof(vatLieuService));
        }

        /// <summary>
        /// Lấy danh sách tất cả vật liệu (Tất cả nhân viên + quản lý)
        /// </summary>
        [HttpGet("GetAllVatLieu")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu},{ApplicationRole.QuanLyNhanSu},{ApplicationRole.QuanLyKho}")] // ✅ TẤT CẢ NHÂN VIÊN + QUẢN LÝ
        public async Task<IActionResult> GetAllVatLieu()
        {
            try
            {
                var result = await _vatLieuService.GetAllVatLieuWithDetailsAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách vật liệu.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy chi tiết vật liệu (Tất cả nhân viên + quản lý)
        /// </summary>
        [HttpGet("GetVatLieuDetail/{maVatLieu}")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu},{ApplicationRole.QuanLyNhanSu},{ApplicationRole.QuanLyKho}")] // ✅ TẤT CẢ NHÂN VIÊN + QUẢN LÝ
        public async Task<IActionResult> GetVatLieuDetail(int maVatLieu)
        {
            try
            {
                var result = await _vatLieuService.GetVatLieuDetailByIdAsync(maVatLieu);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy thông tin vật liệu.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Tạo vật liệu mới (Chỉ quản lý kho)
        /// </summary>
        [HttpPost("CreateVatLieu")]
        [Authorize(Roles = ApplicationRole.QuanLyKho)]
        public async Task<IActionResult> CreateVatLieu([FromBody] AddVatLieuDTO addVatLieuDto)
        {
            try
            {
                if (addVatLieuDto == null)
                    return BadRequest("Dữ liệu không hợp lệ.");

                var result = await _vatLieuService.CreateVatLieuWithFullDetailsAsync(addVatLieuDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi tạo vật liệu.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Cập nhật vật liệu (Chỉ quản lý kho)
        /// </summary>
        [HttpPut("UpdateVatLieu")]
        [Authorize(Roles = ApplicationRole.QuanLyKho)] 
        public async Task<IActionResult> UpdateVatLieu([FromBody] UpdateVatLieuDTO updateVatLieuDto)
        {
            try
            {
                if (updateVatLieuDto == null)
                    return BadRequest("Dữ liệu không hợp lệ.");

                var result = await _vatLieuService.UpdateVatLieuWithDetailsAsync(updateVatLieuDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi cập nhật vật liệu.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái ngừng cung cấp (Chỉ quản lý kho)
        /// </summary>
        [HttpPut("UpdateNgungCungCap/{maVatLieu}")]
        [Authorize(Roles = ApplicationRole.QuanLyKho)] 
        public async Task<IActionResult> UpdateNgungCungCap(int maVatLieu, [FromQuery] bool ngungCungCap)
        {
            try
            {
                var result = await _vatLieuService.UpdateNgungCungCapAsync(maVatLieu, ngungCungCap);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi cập nhật trạng thái vật liệu.",
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}