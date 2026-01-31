﻿using QLQuanKaraokeHKT.Application.DTOs.QLPhongDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Room
{
    public interface IQLLoaiPhongService
    {
        Task<ServiceResult> GetAllLoaiPhongAsync();
        Task<ServiceResult> GetLoaiPhongByIdAsync(int maLoaiPhong);
        Task<ServiceResult> CreateLoaiPhongAsync(AddLoaiPhongDTO addLoaiPhongDto);
        Task<ServiceResult> UpdateLoaiPhongAsync(LoaiPhongDTO loaiPhongDto);
        Task<ServiceResult> DeleteLoaiPhongAsync(int maLoaiPhong);
    }
}