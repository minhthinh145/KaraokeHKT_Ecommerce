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
    public class QLCaLamViecController : ControllerBase
    {
        private readonly IQLCaLamViecService _service;

        public QLCaLamViecController(IQLCaLamViecService service)
        {
            _service = service;
        }

        /// <summary>
        /// Tạo ca làm việc mới (Chỉ quản lý nhân sự)
        /// </summary>
        [HttpPost("create")]
        [Authorize(Roles = ApplicationRole.QuanLyNhanSu)] // ✅ CHỈ QUẢN LÝ NHÂN SỰ
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

        /// <summary>
        /// Lấy danh sách tất cả ca làm việc (Tất cả nhân viên + quản lý)
        /// </summary>
        [HttpGet("getall")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu},{ApplicationRole.QuanLyNhanSu}")] // ✅ TẤT CẢ NHÂN VIÊN + QUẢN LÝ
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

        /// <summary>
        /// Lấy ca làm việc theo ID (Tất cả nhân viên + quản lý)
        /// </summary>
        [HttpGet("getbyid/{maCa}")]
        [Authorize(Roles = $"{ApplicationRole.NhanVienTiepTan},{ApplicationRole.NhanVienKho},{ApplicationRole.NhanVienPhucVu},{ApplicationRole.QuanLyNhanSu}")] // ✅ TẤT CẢ NHÂN VIÊN + QUẢN LÝ
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