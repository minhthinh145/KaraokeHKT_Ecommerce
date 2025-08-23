using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.HRM
{
    /// <summary>
    /// Service interface for managing LichLamViec (work schedule) operations.
    /// </summary>
    public interface IQLLichLamViecService
    {
        /// <summary>
        /// Creates a new work schedule for an employee.
        /// </summary>
        /// <param name="addLichLamViecDto">The work schedule data to create.</param>
        /// <returns>Service result with created work schedule information.</returns>
        Task<ServiceResult> CreateLichLamViecAsync(AddLichLamViecDTO addLichLamViecDto);

        /// <summary>
        /// Retrieves all work schedules in the system.
        /// </summary>
        /// <returns>Service result with list of all work schedules.</returns>
        Task<ServiceResult> GetAllLichLamViecAsync();

        /// <summary>
        /// Retrieves work schedules for a specific employee.
        /// </summary>
        /// <param name="maNhanVien">The unique ID of the employee.</param>
        /// <returns>Service result with list of work schedules for the employee.</returns>
        Task<ServiceResult> GetLichLamViecByNhanVienAsync(Guid maNhanVien);


        /// <summary>
        /// Rettrives all work schedules in a field of time
        /// </summary>
        /// <param name="start">TimeStart</param>
        /// <param name="end">TimeEnd</param>
        /// <returns>Service result with list of work schedules</returns>
        Task<ServiceResult> GetLichLamViecByRangeAsync(DateOnly start, DateOnly end);

        /// <summary>
        /// Update work shcedules 
        /// </summary>
        /// <param name="lichLamViecDto">DTO</param>
        /// <returns></returns>
        Task<ServiceResult> UpdateLichLamViecAsync(LichLamViecDTO lichLamViecDto);

        /// <summary>
        /// Delete a work schedules by it's id
        /// </summary>
        /// <param name="maLichLamViec">id of work schedules need to delete</param>
        /// <returns></returns>
        Task<ServiceResult> DeleteLichLamViecAsync(int maLichLamViec);

        Task<ServiceResult> SendNotiWorkSchedulesAsync(DateOnly start, DateOnly end);
        Task<ServiceResult> GetLichLamViecByNhanVienAndRangeAsync(Guid maNhanVien, DateOnly start, DateOnly end);
    }
}