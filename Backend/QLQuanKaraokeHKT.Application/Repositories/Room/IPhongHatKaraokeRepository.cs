using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Room
{

    public interface IPhongHatKaraokeRepository : IGenericRepository<PhongHatKaraoke, int>
    {
        Task<List<PhongHatKaraoke>> GetAllWithDetailsAsync(bool includePricing = false);
        Task<PhongHatKaraoke?> GetByIdWithDetailsAsync(int maPhong, bool includePricing = false);
        Task<List<PhongHatKaraoke>> GetByLoaiPhongAsync(int maLoaiPhong, bool includePricing = false);

        Task<List<PhongHatKaraoke>> GetAvailableAsync();  
        Task<List<PhongHatKaraoke>> GetOccupiedAsync();   
        Task<List<PhongHatKaraoke>> GetOutOfServiceAsync(); 

        Task<PhongHatKaraoke?> GetByIdForUpdateAsync(int maPhong);
    }
}