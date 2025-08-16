using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.Controllers.Helper;
using QLQuanKaraokeHKT.DTOs.BookingDTOs;
using QLQuanKaraokeHKT.Services.BookingServices;

namespace QLQuanKaraokeHKT.Controllers.BookingController
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangBookingController : ControllerBase
    {
        private readonly IKhachHangDatPhongService _bookingService;
        private readonly IConfiguration _configuration;

        public KhachHangBookingController(
            IKhachHangDatPhongService bookingService,
            IConfiguration configuration)
        {
            _bookingService = bookingService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lấy danh sách phòng hát có thể đặt
        /// </summary>
        [HttpGet("available-rooms")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableRoomsAsync()
        {
            try
            {
                var result = await _bookingService.GetPhongHatAvailableAsync();
                return result.IsSuccess ? Ok(result) : BadRequest(result);
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

        /// <summary>
        /// Lấy danh sách phòng theo loại
        /// </summary>
        [HttpGet("rooms-by-type/{maLoaiPhong}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoomsByTypeAsync(int maLoaiPhong)
        {
            try
            {
                if (maLoaiPhong <= 0)
                    return BadRequest("Mã loại phòng không hợp lệ.");

                var result = await _bookingService.GetPhongHatByLoaiAsync(maLoaiPhong);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
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

        /// <summary>
        /// BƯỚC 1: Tạo hóa đơn đặt phòng (chưa thanh toán)
        /// </summary>
        [HttpPost("create-booking")]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> CreateBookingAsync([FromBody] DatPhongDTO datPhongDto)
        {
            try
            {
                // ✅ VALIDATION AUTHENTICATION & MODEL
                var validationResult = this.ValidateAuthenticationAndModel(out Guid userId);
                if (validationResult != null)
                    return validationResult;

                // ✅ ĐẨY LOGIC VÀO SERVICE
                var result = await _bookingService.TaoHoaDonDatPhongAsync(datPhongDto, userId);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi tạo hóa đơn đặt phòng.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// BƯỚC 2: Xác nhận thanh toán và tạo URL VNPay
        /// </summary>
        [HttpPost("confirm-payment")]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> ConfirmPaymentAsync([FromBody] XacNhanThanhToanDTO xacNhanDto)
        {
            try
            {
                var validationResult = this.ValidateAuthenticationAndModel(out Guid userId);
                if (validationResult != null)
                    return validationResult;

                var result = await _bookingService.XacNhanThanhToanAsync(xacNhanDto, HttpContext);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi xác nhận thanh toán.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Callback từ VNPay sau thanh toán
        /// </summary>
        [HttpGet("vnpay-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> VNPayCallbackAsync()
        {
            try
            {
                var result = await _bookingService.XuLyThanhToanVNPayAsync(Request.Query);

                var frontendUrl = _configuration["AppSettings:FrontendUrl"] ?? "http://localhost:5173";

                if (result.IsSuccess)
                {
                    return Redirect($"{frontendUrl}/customer/booking/success");
                }
                else
                {
                    return Redirect($"{frontendUrl}/customer/booking/failed?message={Uri.EscapeDataString(result.Message)}");
                }
            }
            catch (Exception ex)
            {
                var frontendUrl = _configuration["AppSettings:FrontendUrl"] ?? "http://localhost:5173";
                return Redirect($"{frontendUrl}/booking/failed?message={Uri.EscapeDataString($"Lỗi xử lý thanh toán: {ex.Message}")}");
            }
        }

        /// <summary>
        /// Lấy lịch sử đặt phòng của khách hàng (tự động lấy từ token)
        /// </summary>
        [HttpGet("history")]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> GetBookingHistoryAsync()
        {
            try
            {
                var validationResult = this.ValidateUserAuthentication(out Guid userId);
                if (validationResult != null)
                    return validationResult;

                // ✅ ĐẨY LOGIC VÀO SERVICE
                var result = await _bookingService.GetLichSuDatPhongByUserIdAsync(userId);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy lịch sử đặt phòng.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Hủy đặt phòng (tự động lấy khách hàng từ token)
        /// </summary>
        [HttpPut("cancel/{maThuePhong}")]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> CancelBookingAsync(Guid maThuePhong)
        {
            try
            {
                if (maThuePhong == Guid.Empty)
                    return BadRequest("Mã thuê phòng không hợp lệ.");

                var validationResult = this.ValidateUserAuthentication(out Guid userId);
                if (validationResult != null)
                    return validationResult;

                // ✅ ĐẨY LOGIC VÀO SERVICE
                var result = await _bookingService.HuyDatPhongByUserIdAsync(maThuePhong, userId);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi hủy đặt phòng.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy danh sách đặt phòng chưa thanh toán của khách hàng
        /// </summary>
        [HttpGet("unpaid-bookings")]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> GetUnpaidBookingsAsync()
        {
            try
            {
                var validationResult = this.ValidateAuthenticationAndModel(out Guid userId);
                if (validationResult != null)
                    return validationResult;

                var result = await _bookingService.GetDatPhongChuaThanhToanByUserIdAsync(userId);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách đặt phòng chưa thanh toán.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        //thanh toán lại
        [HttpPut("re-pay/{maThuePhong}")]
        [Authorize]
        public async Task<IActionResult> RePayBookingAsync(Guid maThuePhong)
        {
            try
            {
                if (maThuePhong == Guid.Empty)
                    return BadRequest("Mã thuê phòng không hợp lệ.");
                var validationResult = this.ValidateUserAuthentication(out Guid userId);
                if (validationResult != null)
                    return validationResult;
                var result = await _bookingService.ThanhToanLaiAsync(maThuePhong, userId);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi thanh toán lại.",
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}