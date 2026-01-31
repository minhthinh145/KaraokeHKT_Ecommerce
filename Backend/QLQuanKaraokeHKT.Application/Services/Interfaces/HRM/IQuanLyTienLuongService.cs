﻿using QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.HRM
{
    public interface IQuanLyTienLuongService
    {
        Task<ServiceResult> GetAllLuongCaLamViecsAsync();
        Task<ServiceResult> GetLuongCaLamViecByIdAsync(int maLuongCaLamViec);
        Task<ServiceResult> GetLuongCaLamViecByMaCaAsync(int maCa);
        Task<ServiceResult> CreateLuongCaLamViecAsync(AddLuongCaLamViecDTO addLuongCaLamViecDto);
        Task<ServiceResult> UpdateLuongCaLamViecAsync(LuongCaLamViecDTO luongCaLamViecDto);
        Task<ServiceResult> DeleteLuongCaLamViecAsync(int maLuongCaLamViec);    
    }
}
