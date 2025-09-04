using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Room
{
    public interface ILoaiPhongRepository : IGenericRepository<LoaiPhongHatKaraoke,int>
    {

        Task<bool> HasPhongHatKaraokeAsync(int maLoaiPhong);
    }
}
