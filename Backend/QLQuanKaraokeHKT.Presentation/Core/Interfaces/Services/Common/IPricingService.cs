using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Shared.Models.Pricing;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Common
{
    public interface IPricingService
    {
        Task ApplyPricingConfigAsync(int maSanPham, IPricingConfig pricingConfig);
        Task DisableCurrentPricesAsync(int maSanPham);
        Task<List<GiaDichVu>> GetCurrentPricesAsync(int maSanPham);
        Task<CustomerPricingInfo> GetCustomerPricingInfoAsync(int maSanPham);
        Task<decimal> GetCurrentCaPriceAsync(int maSanPham);
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

