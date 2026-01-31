﻿using QLQuanKaraokeHKT.Application.DTOs;
using QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.HRM
{
    public interface IHRMOrchestrator
    {
        Task<ServiceResult> GetAllNhanVienAsync();

        Task<ServiceResult> AddNhanVienWithAccountWorkFlowAsync(AddNhanVienDTO addNhanVienDto);

        Task<ServiceResult> UpdateNhanVienAsyncAndAccountAsync(NhanVienDTO nhanVienDto);

        Task<ServiceResult> UpdateNhanVienDaNghiViecAsync(Guid maNhanVien, bool daNghiViec);
    }
}