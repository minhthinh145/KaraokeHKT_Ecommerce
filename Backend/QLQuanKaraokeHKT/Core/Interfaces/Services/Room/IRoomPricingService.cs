using QLQuanKaraokeHKT.Core.Entities;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Room
{
    public interface IRoomPricingService
    {
        Task ApplyPricingConfigAsync(int maSanPham, IPricingConfig pricingConfig);
        Task DisableCurrentPricesAsync(int maSanPham);
        Task<List<GiaDichVu>> GetCurrentPricesAsync(int maSanPham);

        IPricingConfig ExtractPricingConfig(List<GiaDichVu> prices);

    }
    public interface IPricingConfig
    {
        bool DongGiaAllCa { get; }
        decimal? GiaThueChung { get; }
        decimal? GiaThueCa1 { get; }
        decimal? GiaThueCa2 { get; }
        decimal? GiaThueCa3 { get; }
        DateOnly NgayApDungGia { get; }
        string TrangThaiGia { get; }
    }
}
