
namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Room
{
    public interface IRoomQueryService
    {
        Task<ServiceResult> GetAllRoomsAsync(int pageIndex = 1, int pageSize = 10);
        Task<ServiceResult> GetRoomByIdAsync(int roomId);
    }
}