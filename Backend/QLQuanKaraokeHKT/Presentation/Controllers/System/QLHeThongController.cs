using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement;
using QLQuanKaraokeHKT.Core.Interfaces.Services.HRM;
using QLQuanKaraokeHKT.Presentation.Extensions;

namespace QLQuanKaraokeHKT.Presentation.Controllers.System
{
    [Authorize(Roles = ApplicationRole.QuanTriHeThong)]
    [Route("api/[controller]")]
    [ApiController]
    
    public class QLHeThongController : ControllerBase
    {
        private readonly IAdminAccountService _adminAccountService;
        private readonly INhanVienAccountService _nhanVienAccountService;
        private readonly IKhachHangAccountService _khachHangAccountService;
        private readonly IAccountManagementService _accountManagementService;

        public QLHeThongController(
            IAdminAccountService adminAccountService, 
            INhanVienAccountService nhanVienAccountService, 
            IKhachHangAccountService khachHangAccountService, 
            IAccountManagementService accountManagementService
            )
        {
            _adminAccountService = adminAccountService ?? throw new ArgumentNullException(nameof(adminAccountService));
            _nhanVienAccountService = nhanVienAccountService ?? throw new ArgumentNullException(nameof(nhanVienAccountService));
            _khachHangAccountService = khachHangAccountService ?? throw new ArgumentNullException(nameof(khachHangAccountService));
            _accountManagementService = accountManagementService ?? throw new ArgumentNullException(nameof(accountManagementService));

        }


        /// <summary>
        /// Lấy danh sách tất cả nhân viên
        /// </summary>
        /// <returns></returns>
        [HttpGet("nhanvienAll")]
        public async Task<IActionResult> GetAllNhanVien()
        {
            try
            {
                var result = await _nhanVienAccountService.GetAllTaiKhoanNhanVienAsync();
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


        [HttpGet("adminAll")]
        public async Task<IActionResult> GetAllQuanLy()
        {
            try
            {
                var result = await _adminAccountService.GetAllAdminAccountAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách quản lý.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả tài khoản khách hàng
        /// </summary>
        /// <returns>Danh sách tài khoản khách hàng</returns>
        [HttpGet("taikhoan/khachhang")]
        public async Task<IActionResult> GetAllTaiKhoanKhachHang()
        {
            try
            {
                var result = await _khachHangAccountService.GetAllTaiKhoanKhachHangAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách tài khoản khách hàng.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả tài khoản nhân viên
        /// </summary>
        /// <returns>Danh sách tài khoản nhân viên</returns>
        [HttpGet("taikhoan/nhanvien")]
        public async Task<IActionResult> GetAllTaiKhoanNhanVien()
        {
            try
            {
                var result = await _nhanVienAccountService.GetAllTaiKhoanNhanVienAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách tài khoản nhân viên.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy danh sách tài khoản nhân viên theo loại tài khoản
        /// </summary>
        /// <param name="loaiTaiKhoan">Loại tài khoản (VD: NhanVienPhucVu, QuanLyKho...)</param>
        /// <returns>Danh sách tài khoản nhân viên theo loại</returns>
        [HttpGet("taikhoan/nhanvien/loai/{loaiTaiKhoan}")]
        public async Task<IActionResult> GetAllTaiKhoanNhanVienByLoaiTaiKhoan(string loaiTaiKhoan)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loaiTaiKhoan))
                {
                    return BadRequest(new
                    {
                        message = "Loại tài khoản không được để trống.",
                        success = false
                    });
                }

                var result = await _nhanVienAccountService.GetAllTaiKhoanNhanVienByLoaiTaiKhoanAsync(loaiTaiKhoan);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách nhân viên theo loại tài khoản.",
                    success = false,
                    error = ex.Message
                });
            }
        }


        /// <summary>
        /// Tạo tài khoản cho nhân viên đã có sẵn
        /// </summary>
        /// <param name="request">Thông tin mã nhân viên và email</param>
        /// <returns>Kết quả tạo tài khoản</returns>
        [HttpPost("taikhoan/nhanvien/gan-tai-khoan")]
        public async Task<IActionResult> AddTaiKhoanForNhanVien([FromBody] AddTaiKhoanForNhanVienDTO request)
        {
            try
            {
                var modelValidation = this.ValidateModelState();
                if (modelValidation != null)
                    return modelValidation;

                var result = await _nhanVienAccountService.ChangeTaiKhoanForNhanVienAsync(request);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi tạo tài khoản cho nhân viên.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Tạo tài khoản quản lý mới
        /// </summary>
        /// <returns></returns>
        [HttpPost("taikhoan/admin/tao-tai-khoan")]
        public async Task<IActionResult> CreateTaiKhoanQuanLy([FromBody] AddAccountForAdminDTO request)
        {
            try
            {
                var modelValidation = this.ValidateModelState();
                if (modelValidation != null)
                    return modelValidation;
                var result = await _adminAccountService.AddAdminAccountAsync(request);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi tạo tài khoản quản lý.",
                    success = false,
                    error = ex.Message
                });
            }
        }
        /// <summary>
        /// Lấy danh sách các loại tài khoản có sẵn
        /// </summary>
        /// <returns>Danh sách loại tài khoản</returns>
        [HttpGet("loai-tai-khoan")]
        public async Task<IActionResult> GetLoaiTaiKhoan()
        {
            try
            {
                var result = await _accountManagementService.GetAllLoaiTaiKhoanAsync();
                return result.IsSuccess
                    ? Ok(result)
                    : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách loại tài khoản.",
                    success = false,
                    error = ex.Message
                });
            }
        }
        /// <summary>
        /// Khóa tài khoản theo mã tài khoản
        /// </summary>
        /// <param name="maTaiKhoan">Mã tài khoản cần khóa</param>
        /// <returns>Kết quả khóa tài khoản</returns>
        [HttpPut("taikhoan/{maTaiKhoan}/lock")]
        public async Task<IActionResult> LockAccount(Guid maTaiKhoan)
        {
            try
            {
                if (maTaiKhoan == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        message = "Mã tài khoản không hợp lệ.",
                        success = false
                    });
                }

                var result = await _accountManagementService.LockAccountByMaTaiKhoanAsync(maTaiKhoan);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi khóa tài khoản.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Mở khóa tài khoản theo mã tài khoản
        /// </summary>
        /// <param name="maTaiKhoan">Mã tài khoản cần mở khóa</param>
        /// <returns>Kết quả mở khóa tài khoản</returns>
        [HttpPut("taikhoan/{maTaiKhoan}/unlock")]
        public async Task<IActionResult> UnlockAccount(Guid maTaiKhoan)
        {
            try
            {
                if (maTaiKhoan == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        message = "Mã tài khoản không hợp lệ.",
                        success = false
                    });
                }

                var result = await _accountManagementService.UnlockAccountByMaTaiKhoanAsync(maTaiKhoan);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi mở khóa tài khoản.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpDelete("taikhoan/{maTaiKhoan}/delete")]
        public async Task<IActionResult> DeleteAccount(Guid maTaiKhoan)
        {
            try
            {
                if (maTaiKhoan == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        message = "Mã tài khoản không hợp lệ.",
                        success = false
                    });
                }
                var result = await _accountManagementService.DeleteAccountAsync(maTaiKhoan);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi xóa tài khoản.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpPut("taikhoan/update")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDTO updateAccountDTO)
        {
            try
            {
                var modelValidation = this.ValidateModelState();
                if (modelValidation != null)
                    return modelValidation;
                var result = await _accountManagementService.UpdateAccountAsync(updateAccountDTO);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi cập nhật tài khoản.",
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}