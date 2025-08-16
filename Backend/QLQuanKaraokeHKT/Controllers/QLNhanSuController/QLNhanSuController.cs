using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.Controllers.Helper;
using QLQuanKaraokeHKT.DTOs;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Services.QLHeThongServices;

namespace QLQuanKaraokeHKT.Controllers.QLNhanSuController
{
    [Authorize(Roles = ApplicationRole.QuanLyNhanSu)]
    [Route("api/[controller]")]
    [ApiController]
    public class QLNhanSuController : ControllerBase
    {
        private readonly IQLNhanSuService _qlHeThongService;

        public QLNhanSuController(IQLNhanSuService qlHeThongService)
        {
            _qlHeThongService = qlHeThongService ?? throw new ArgumentNullException(nameof(qlHeThongService));
        }

        /// <summary>
        /// Lấy danh sách tất cả nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        [HttpGet("nhanvien")]
        public async Task<IActionResult> GetAllNhanVien()
        {
            try
            {
                var result = await _qlHeThongService.GetAllNhanVienAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách nhân viên.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Tạo mới nhân viên và tài khoản
        /// </summary>
        /// <param name="request">Thông tin nhân viên và mật khẩu</param>
        /// <returns>Kết quả tạo nhân viên</returns>
        [HttpPost("nhanvien")]
        public async Task<IActionResult> AddNhanVienAndAccount([FromBody] AddNhanVienDTO request)
        {
            try
            {
                var modelValidation = this.ValidateModelState();
                if (modelValidation != null)
                    return modelValidation;

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new
                    {
                        message = "Mật khẩu không được để trống.",
                        success = false
                    });
                }

                var result = await _qlHeThongService.AddNhanVienAndAccountAsync(request, request.Email);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi tạo nhân viên.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Cập nhật thông tin nhân viên và tài khoản
        /// </summary>
        /// <param name="nhanVienDto">Thông tin nhân viên cần cập nhật</param>
        /// <returns>Kết quả cập nhật</returns>
        [HttpPut("nhanvien")]
        public async Task<IActionResult> UpdateNhanVienAndAccount([FromBody] NhanVienDTO nhanVienDto)
        {
            try
            {
                var modelValidation = this.ValidateModelState();
                if (modelValidation != null)
                    return modelValidation;

                if (nhanVienDto.MaNv == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        message = "Mã nhân viên không hợp lệ.",
                        success = false
                    });
                }

                var result = await _qlHeThongService.UpdateNhanVienAsyncAndAccountAsync(nhanVienDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi cập nhật nhân viên.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Đánh dấu hoặc bỏ đánh dấu nhân viên đã nghỉ việc (và tự động khóa/mở khóa tài khoản)
        /// </summary>
        /// <param name="maNhanVien">Mã nhân viên</param>
        /// <param name="daNghiViec">true: nghỉ việc, false: đang làm</param>
        /// <returns>Kết quả cập nhật trạng thái</returns>
        [HttpPatch("nhanvien/danghiviec")]
        public async Task<IActionResult> UpdateNhanVienDaNghiViec([FromQuery] Guid maNhanVien, [FromQuery] bool daNghiViec)
        {
            try
            {
                if (maNhanVien == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        message = "Mã nhân viên không hợp lệ.",
                        success = false
                    });
                }

                var result = await _qlHeThongService.UpdateNhanVienDaNghiViecAsync(maNhanVien, daNghiViec);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi cập nhật trạng thái nghỉ việc của nhân viên.",
                    success = false,
                    error = ex.Message
                });
            }
        }
    }

  
}