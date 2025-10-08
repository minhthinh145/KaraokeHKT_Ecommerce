using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLPhongDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Room
{
    public interface IRoomOrchestrator
    {
        Task<ServiceResult> CreateRoomWorkflowAsync(AddPhongHatDTO addPhongHatDTO);

        #region Update Workflows
        Task<ServiceResult> UpdateRoomWorkflowAsync(UpdatePhongHatDTO request);
        Task<ServiceResult> ChangeRoomOperationalStatusWorkflowAsync(int maPhong, bool ngungHoatDong);
        Task<ServiceResult> ChangeRoomOccupancyStatusWorkflowAsync(int maPhong, bool dangSuDung);
        #endregion

        #region Query Workflows
        Task<ServiceResult> GetAllRoomsWorkflowAsync();
        Task<ServiceResult> GetRoomDetailWorkflowAsync(int maPhong);
        Task<ServiceResult> GetRoomsByTypeWorkflowAsync(int maLoaiPhong);
        Task<ServiceResult> GetRoomsByStatusWorkflowAsync(RoomStatusFilter statusFilter);
        #endregion
    }
    public enum RoomStatusFilter
    {
        All,
        Available,
        Occupied,
        OutOfService
    }
}
