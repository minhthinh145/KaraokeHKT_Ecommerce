﻿using QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.HRM
{
    public interface IQLCaLamViecService
    {
        Task<ServiceResult> CreateCaLamViecAsync(AddCaLamViecDTO addCaLamViecDto);
        Task<ServiceResult> GetAllCaLamViecsAsync();
        Task<ServiceResult> GetCaLamViecByIdAsync(int maCa);
    }
}
