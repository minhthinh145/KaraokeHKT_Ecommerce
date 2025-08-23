using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement;
using QLQuanKaraokeHKT.Core.Interfaces.Services.HRM;
using QLQuanKaraokeHKT.Presentation.Extensions;
using System.Security.Claims;

namespace QLQuanKaraokeHKT.Presentation.Controllers.HRM
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Yêu cầu đăng nhập
    public class NhanVienAllController : ControllerBase
    {
        private readonly IQLYeuCauChuyenCaService _yeuCauChuyenCaService;
        private readonly IQLLichLamViecService _lichLamViecService;
        private readonly INhanVienAccountService _nhanVienAccountService;

        public NhanVienAllController(
            IQLYeuCauChuyenCaService yeuCauChuyenCaService,
            IQLLichLamViecService lichLamViecService,
            INhanVienAccountService nhanVienAccountService)
        {
            _yeuCauChuyenCaService = yeuCauChuyenCaService ?? throw new ArgumentNullException(nameof(yeuCauChuyenCaService));
            _lichLamViecService = lichLamViecService ?? throw new ArgumentNullException(nameof(lichLamViecService));
            _nhanVienAccountService = nhanVienAccountService ?? throw new ArgumentNullException(nameof(nhanVienAccountService));
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var validateResult = this.ValidateUserAuthentication(out Guid userId);
                if (validateResult != null)
                    return validateResult;

                var result = await _nhanVienAccountService.GetProfileByUserIdAsync(userId);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy thông tin profile.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        #region Lịch làm việc

        /// <summary>
        /// Xem lịch làm việc của nhân viên
        /// </summary>
        [HttpGet("lich-lam-viec/{maNhanVien}")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu},{ApplicationRole.QuanLyNhanSu}")]
        public async Task<IActionResult> GetLichLamViecByNhanVienAsync(Guid maNhanVien)
        {
            try
            {
                var result = await _lichLamViecService.GetLichLamViecByNhanVienAsync(maNhanVien);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy lịch làm việc của nhân viên.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        #endregion

        #region Yêu cầu chuyển ca (Nhân viên)

        /// <summary>
        /// Tạo yêu cầu chuyển ca mới (Nhân viên)
        /// </summary>
        [HttpPost("yeu-cau-chuyen-ca/create")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu}")]
        public async Task<IActionResult> CreateYeuCauChuyenCaAsync([FromBody] AddYeuCauChuyenCaDTO addYeuCauChuyenCaDto)
        {
            try
            {
                if (addYeuCauChuyenCaDto == null)
                    return BadRequest("Dữ liệu không hợp lệ.");

                var result = await _yeuCauChuyenCaService.CreateYeuCauChuyenCaAsync(addYeuCauChuyenCaDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi tạo yêu cầu chuyển ca.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy danh sách yêu cầu chuyển ca của nhân viên
        /// </summary>
        [HttpGet("yeu-cau-chuyen-ca/my-requests/{maNhanVien}")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu}")]
        public async Task<IActionResult> GetMyYeuCauChuyenCaAsync(Guid maNhanVien)
        {
            try
            {
                var result = await _yeuCauChuyenCaService.GetYeuCauChuyenCaByNhanVienAsync(maNhanVien);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách yêu cầu chuyển ca.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Xóa yêu cầu chuyển ca (chỉ chưa phê duyệt)
        /// </summary>
        [HttpDelete("yeu-cau-chuyen-ca/delete/{maYeuCau}")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu}")]
        public async Task<IActionResult> DeleteYeuCauChuyenCaAsync(int maYeuCau)
        {
            try
            {
                var result = await _yeuCauChuyenCaService.DeleteYeuCauChuyenCaAsync(maYeuCau);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi xóa yêu cầu chuyển ca.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        #endregion

        #region Yêu cầu chuyển ca (Quản lý phê duyệt)

        /// <summary>
        /// Lấy danh sách tất cả yêu cầu chuyển ca (Quản lý)
        /// </summary>
        [HttpGet("yeu-cau-chuyen-ca/all")]
        [Authorize(Roles = ApplicationRole.QuanLyNhanSu)]
        public async Task<IActionResult> GetAllYeuCauChuyenCaAsync()
        {
            try
            {
                var result = await _yeuCauChuyenCaService.GetAllYeuCauChuyenCaAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách yêu cầu chuyển ca.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy danh sách yêu cầu chuyển ca chưa phê duyệt (Quản lý)
        /// </summary>
        [HttpGet("yeu-cau-chuyen-ca/pending")]
        [Authorize(Roles = ApplicationRole.QuanLyNhanSu)]
        public async Task<IActionResult> GetYeuCauChuyenCaChuaPheDuyetAsync()
        {
            try
            {
                var result = await _yeuCauChuyenCaService.GetYeuCauChuyenCaChuaPheDuyetAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy yêu cầu chuyển ca cần phê duyệt.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy danh sách yêu cầu chuyển ca đã phê duyệt (Quản lý)
        /// </summary>
        [HttpGet("yeu-cau-chuyen-ca/approved")]
        [Authorize(Roles = ApplicationRole.QuanLyNhanSu)]
        public async Task<IActionResult> GetYeuCauChuyenCaDaPheDuyetAsync()
        {
            try
            {
                var result = await _yeuCauChuyenCaService.GetYeuCauChuyenCaDaPheDuyetAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy yêu cầu chuyển ca đã phê duyệt.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Phê duyệt yêu cầu chuyển ca (Quản lý)
        /// </summary>
        [HttpPut("yeu-cau-chuyen-ca/approve")]
        [Authorize(Roles = ApplicationRole.QuanLyNhanSu)]
        public async Task<IActionResult> PheDuyetYeuCauChuyenCaAsync([FromBody] PheDuyetYeuCauChuyenCaDTO pheDuyetDto)
        {
            try
            {
                if (pheDuyetDto == null)
                    return BadRequest("Dữ liệu phê duyệt không hợp lệ.");

                var result = await _yeuCauChuyenCaService.PheDuyetYeuCauChuyenCaAsync(pheDuyetDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi phê duyệt yêu cầu chuyển ca.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy chi tiết yêu cầu chuyển ca theo ID
        /// </summary>
        [HttpGet("yeu-cau-chuyen-ca/{maYeuCau}")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu},{ApplicationRole.QuanLyNhanSu}")]
        public async Task<IActionResult> GetYeuCauChuyenCaByIdAsync(int maYeuCau)
        {
            try
            {
                var result = await _yeuCauChuyenCaService.GetYeuCauChuyenCaByIdAsync(maYeuCau);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy thông tin yêu cầu chuyển ca.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        #endregion
    }
}