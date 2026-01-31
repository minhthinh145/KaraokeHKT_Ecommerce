﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLQuanKaraokeHKT.Shared.Constants;
using QLQuanKaraokeHKT.Application.DTOs.QLPhongDTOs;
using QLQuanKaraokeHKT.Application.Services.Interfaces.Room;

namespace QLQuanKaraokeHKT.Presentation.Controllers.Room
{
    [Authorize(Roles = ApplicationRole.QuanLyPhongHat)]
    [Route("api/[controller]")]
    [ApiController]
    public class QLLoaiPhongController : ControllerBase
    {
        private readonly IQLLoaiPhongService _loaiPhongService;

        public QLLoaiPhongController(IQLLoaiPhongService loaiPhongService)
        {
            _loaiPhongService = loaiPhongService ?? throw new ArgumentNullException(nameof(loaiPhongService));
        }

        [HttpGet("GetAllLoaiPhong")]
        public async Task<IActionResult> GetAllLoaiPhong()
        {
            try
            {
                var result = await _loaiPhongService.GetAllLoaiPhongAsync();
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy danh sách loại phòng.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet("GetLoaiPhong/{maLoaiPhong}")]
        public async Task<IActionResult> GetLoaiPhong(int maLoaiPhong)
        {
            try
            {
                var result = await _loaiPhongService.GetLoaiPhongByIdAsync(maLoaiPhong);
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy thông tin loại phòng.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpPost("CreateLoaiPhong")]
        public async Task<IActionResult> CreateLoaiPhong([FromBody] AddLoaiPhongDTO addLoaiPhongDto)
        {
            try
            {
                if (addLoaiPhongDto == null)
                    return BadRequest("Dữ liệu không hợp lệ.");

                var result = await _loaiPhongService.CreateLoaiPhongAsync(addLoaiPhongDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi tạo loại phòng.",
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpPut("UpdateLoaiPhong")]
        public async Task<IActionResult> UpdateLoaiPhong([FromBody] LoaiPhongDTO loaiPhongDto)
        {
            try
            {
                if (loaiPhongDto == null)
                    return BadRequest("Dữ liệu không hợp lệ.");

                var result = await _loaiPhongService.UpdateLoaiPhongAsync(loaiPhongDto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi cập nhật loại phòng.",
                    success = false,
                    error = ex.Message
                });
            }
        }

    }
}