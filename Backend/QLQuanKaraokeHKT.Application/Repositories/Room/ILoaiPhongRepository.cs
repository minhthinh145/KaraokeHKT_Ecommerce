using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Room
{
    public interface ILoaiPhongRepository : IGenericRepository<LoaiPhongHatKaraoke,int>
    {

        Task<bool> HasPhongHatKaraokeAsync(int maLoaiPhong);
    }
}
