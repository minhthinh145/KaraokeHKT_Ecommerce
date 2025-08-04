using QLQuanKaraokeHKT.Helpers;

namespace QLQuanKaraokeHKT.Services.Interfaces
{
    public interface IPhongHatKaraokeService
    {
        public Task<ServiceResult> GetAllPhongHatKaraokeIsActiveAsync();
    }
}
