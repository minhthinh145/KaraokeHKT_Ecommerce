﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.Shared.Constants;
using QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Application.Services.Interfaces.HRM;

namespace QLQuanKaraokeHKT.Presentation.Controllers.HRM
{
    [Route("api/[controller]")]
    [ApiController]
    public class QLLichLamViecController : ControllerBase
    {
        private readonly IQLLichLamViecService _service;

        public QLLichLamViecController(IQLLichLamViecService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lấy tất cả lịch làm việc (Tất cả nhân viên + quản lý)
        /// </summary>
        [HttpGet("GetAllLichLamViec")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu},{ApplicationRole.QuanLyNhanSu}")] // ✅ TẤT CẢ NHÂN VIÊN + QUẢN LÝ
        public async Task<IActionResult> GetAllLichLamViec()
        {
            try
            {
                var result = await _service.GetAllLichLamViecAsync();
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
                    message = "Lỗi hệ thống khi lấy danh sách lịch làm việc.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy lịch làm việc theo nhân viên (Tất cả nhân viên + quản lý)
        /// </summary>
        [HttpGet("GetLichLamViecByNhanVien/{maNhanVien}")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu},{ApplicationRole.QuanLyNhanSu}")]
        public async Task<IActionResult> GetLichLamViecByNhanVien(Guid maNhanVien)
        {
            try
            {
                var result = await _service.GetLichLamViecByNhanVienAsync(maNhanVien);
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
                    message = "Lỗi hệ thống khi lấy lịch làm việc của nhân viên.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy lịch làm việc theo khoảng thời gian (Tất cả nhân viên + quản lý)
        /// </summary>
        [HttpGet("GetLichLamViecByRange")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu},{ApplicationRole.QuanLyNhanSu}")] // ✅ TẤT CẢ NHÂN VIÊN + QUẢN LÝ
        public async Task<IActionResult> GetLichLamViecByRange([FromQuery] DateOnly start, [FromQuery] DateOnly end)
        {
            try
            {
                var result = await _service.GetLichLamViecByRangeAsync(start, end);
                if (result.IsSuccess)
                    return Ok(result);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy lịch làm việc theo khoảng ngày.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Tạo lịch làm việc (Chỉ quản lý nhân sự)
        /// </summary>
        [HttpPost("CreateLichLamViec")]
        [Authorize(Roles = ApplicationRole.QuanLyNhanSu)] // ✅ CHỈ QUẢN LÝ NHÂN SỰ
        public async Task<IActionResult> CreateLichLamViecAsync([FromBody] AddLichLamViecDTO addLichLamViecDto)
        {
            try
            {
                if (addLichLamViecDto == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }
                var result = await _service.CreateLichLamViecAsync(addLichLamViecDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi tạo lịch làm việc.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Cập nhật lịch làm việc (Chỉ quản lý nhân sự)
        /// </summary>
        [HttpPut("UpdateLichLamViec")]
        [Authorize(Roles = ApplicationRole.QuanLyNhanSu)] 
        public async Task<IActionResult> UpdateLichLamViecAsync([FromBody] LichLamViecDTO lichLamViecDto)
        {
            try
            {
                if (lichLamViecDto == null)
                    return BadRequest("Dữ liệu không hợp lệ.");

                var result = await _service.UpdateLichLamViecAsync(lichLamViecDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi cập nhật lịch làm việc.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Xóa lịch làm việc (Chỉ quản lý nhân sự)
        /// </summary>
        [HttpDelete("DeleteLichLamViec/{maLichLamViec}")]
        [Authorize(Roles = ApplicationRole.QuanLyNhanSu)] // ✅ CHỈ QUẢN LÝ NHÂN SỰ
        public async Task<IActionResult> DeleteLichLamViecAsync(int maLichLamViec)
        {
            try
            {
                var result = await _service.DeleteLichLamViecAsync(maLichLamViec);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi xóa lịch làm việc.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Gửi thông báo lịch làm việc (Chỉ quản lý nhân sự)
        /// </summary>
        [HttpPost("SendNotiWorkSchedules")]
        [Authorize(Roles = ApplicationRole.QuanLyNhanSu)] //
        public async Task<IActionResult> SendNotiWorkSchedules([FromQuery] DateOnly start, [FromQuery] DateOnly end)
        {
            try
            {
                var result = await _service.SendNotiWorkSchedulesAsync(start, end);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi gửi thông báo lịch làm việc.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy lịch làm việc của nhân viên theo khoảng ngày
        /// </summary>
        [HttpGet("GetLichLamViecByNhanVienAndRange")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu},{ApplicationRole.QuanLyNhanSu}")]
        public async Task<IActionResult> GetLichLamViecByNhanVienAndRange([FromQuery] Guid maNhanVien, [FromQuery] DateOnly start, [FromQuery] DateOnly end)
        {
            try
            {
                var result = await _service.GetLichLamViecByNhanVienAndRangeAsync(maNhanVien, start, end);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy lịch làm việc theo khoảng ngày.",
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}