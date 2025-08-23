using QLQuanKaraokeHKT.Core.Common;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Room
{
    public interface IPhongHatKaraokeService
    {
        public Task<ServiceResult> GetAllPhongHatKaraokeIsActiveAsync();
    }
}
