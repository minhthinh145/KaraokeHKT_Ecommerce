using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Common;
using QLQuanKaraokeHKT.Shared.Models.Pricing;
namespace QLQuanKaraokeHKT.Application.Services.Common
{
    public class PricingService : IPricingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PricingService> _logger;
        private Dictionary<string, int> _caLamViecCache = new Dictionary<string, int>();

        public PricingService(IUnitOfWork unitOfWork, ILogger<PricingService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ApplyPricingConfigAsync(int maSanPham, IPricingConfig pricingConfig)
        {
            try
            {
                if (pricingConfig == null)
                    throw new ArgumentNullException(nameof(pricingConfig));

                _logger.LogInformation("Áp dụng cấu hình giá cho sản phẩm ID: {MaSanPham}", maSanPham);

                if (pricingConfig.DongGiaAllCa)
                {
                    await ApplyFlatRatePricingAsync(maSanPham, pricingConfig);
                }
                else
                {
                    await ApplyShiftBasedPricingAsync(maSanPham, pricingConfig);
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Tham số null khi áp dụng cấu hình giá cho sản phẩm {MaSanPham}", maSanPham);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi áp dụng cấu hình giá cho sản phẩm {MaSanPham}", maSanPham);
                throw;
            }
        }

        public async Task DisableCurrentPricesAsync(int maSanPham)
        {
            try
            {
                _logger.LogInformation("Vô hiệu hóa giá hiện tại cho sản phẩm ID: {MaSanPham}", maSanPham);
                var updatedCount = await _unitOfWork.GiaDichVuRepository.BulkUpdateStatusByProductAsync(maSanPham, "HetHieuLuc");
                _logger.LogInformation("Đã vô hiệu hóa {Count} giá cho sản phẩm {MaSanPham}", updatedCount, maSanPham);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi vô hiệu hóa giá cho sản phẩm {MaSanPham}", maSanPham);
                throw;
            }
        }

        public async Task<List<GiaDichVu>> GetCurrentPricesAsync(int maSanPham)
        {
            try
            {
                var prices = await _unitOfWork.GiaDichVuRepository.GetPricesByProductAsync(maSanPham);
                return prices ?? new List<GiaDichVu>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy giá hiện tại cho sản phẩm {MaSanPham}", maSanPham);
                return new List<GiaDichVu>();
            }
        }

        public async Task<CustomerPricingInfo> GetCustomerPricingInfoAsync(int maSanPham)
        {
            try
            {
                var currentPrices = await GetCurrentPricesAsync(maSanPham);
                
                if (!currentPrices.Any())
                {
                    return new CustomerPricingInfo { GiaThueHienTai = 0, CoGiaTheoCa = false };
                }

                var pricingInfo = new CustomerPricingInfo();

                // Check if có giá chung (MaCa == null)
                var flatRate = currentPrices.FirstOrDefault(g => g.MaCa == null);
                if (flatRate != null)
                {
                    pricingInfo.GiaThueHienTai = flatRate.DonGia;
                    pricingInfo.CoGiaTheoCa = false;
                    return pricingInfo;
                }

                // Có giá theo ca - lấy theo tên ca
                pricingInfo.CoGiaTheoCa = true;
                
                foreach (var price in currentPrices.Where(p => p.MaCaNavigation != null))
                {
                    switch (price.MaCaNavigation.TenCa)
                    {
                        case "Ca 1":
                            pricingInfo.GiaThueCa1 = price.DonGia;
                            break;
                        case "Ca 2":
                            pricingInfo.GiaThueCa2 = price.DonGia;
                            break;
                        case "Ca 3":
                            pricingInfo.GiaThueCa3 = price.DonGia;
                            break;
                    }
                }

                // Set giá hiện tại theo ca hiện tại
                pricingInfo.GiaThueHienTai = GetCurrentCaPriceFromCaPrices(pricingInfo);

                return pricingInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin giá cho khách hàng {MaSanPham}", maSanPham);
                return new CustomerPricingInfo { GiaThueHienTai = 0, CoGiaTheoCa = false };
            }
        }

        public async Task<decimal> GetCurrentCaPriceAsync(int maSanPham)
        {
            var pricingInfo = await GetCustomerPricingInfoAsync(maSanPham);
            return pricingInfo.GiaThueHienTai;
        }

        private decimal GetCurrentCaPriceFromCaPrices(CustomerPricingInfo pricingInfo)
        {
            if (!pricingInfo.CoGiaTheoCa)
                return pricingInfo.GiaThueHienTai;

            var currentHour = DateTime.Now.Hour;
            
            return currentHour switch
            {
                >= 6 and < 12 => pricingInfo.GiaThueCa1 ?? 0,   // Ca 1: 6h-12h
                >= 12 and < 18 => pricingInfo.GiaThueCa2 ?? 0,  // Ca 2: 12h-18h  
                _ => pricingInfo.GiaThueCa3 ?? 0                 // Ca 3: 18h-6h
            };
        }

        private async Task ApplyFlatRatePricingAsync(int maSanPham, IPricingConfig config)
        {
            try
            {
                if (!config.GiaThueChung.HasValue)
                {
                    _logger.LogWarning("Giá thuê chung không được cung cấp cho sản phẩm {MaSanPham}", maSanPham);
                    return;
                }

                var giaDichVu = new GiaDichVu
                {
                    MaSanPham = maSanPham,
                    MaCa = null,
                    DonGia = config.GiaThueChung.Value,
                    NgayApDung = config.NgayApDungGia,
                    TrangThai = config.TrangThaiGia
                };

                await _unitOfWork.GiaDichVuRepository.CreateAsync(giaDichVu);
                _logger.LogInformation("Tạo giá đồng ca: {Gia} cho sản phẩm {MaSanPham}", config.GiaThueChung.Value, maSanPham);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi áp dụng giá đồng ca cho sản phẩm {MaSanPham}", maSanPham);
                throw;
            }
        }

        private async Task ApplyShiftBasedPricingAsync(int maSanPham, IPricingConfig config)
        {
            try
            {
                var shiftMappings = new[]
                {
                    new { ShiftName = "Ca 1", Price = config.GiaThueCa1 },
                    new { ShiftName = "Ca 2", Price = config.GiaThueCa2 },
                    new { ShiftName = "Ca 3", Price = config.GiaThueCa3 }
                };

                if (_caLamViecCache.Count == 0)
                {
                    await LoadShiftCacheAsync(shiftMappings.Select(s => s.ShiftName).ToList());
                }

                foreach (var shift in shiftMappings)
                {
                    if (shift.Price.HasValue && _caLamViecCache.TryGetValue(shift.ShiftName, out int maCa))
                    {
                        var giaDichVu = new GiaDichVu
                        {
                            MaSanPham = maSanPham,
                            MaCa = maCa,
                            DonGia = shift.Price.Value,
                            NgayApDung = config.NgayApDungGia,
                            TrangThai = config.TrangThaiGia
                        };

                        await _unitOfWork.GiaDichVuRepository.CreateAsync(giaDichVu);
                        _logger.LogInformation("Tạo giá {Ca}: {Gia} cho sản phẩm {MaSanPham}", shift.ShiftName, shift.Price.Value, maSanPham);
                    }
                    else if (shift.Price.HasValue)
                    {
                        _logger.LogWarning("Không thể tạo giá cho ca {ShiftName}: Ca không tồn tại", shift.ShiftName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi áp dụng giá theo ca cho sản phẩm {MaSanPham}", maSanPham);
                throw;
            }
        }

        private async Task LoadShiftCacheAsync(List<string> shiftNames)
        {
            try
            {
                var allShifts = await _unitOfWork.CaLamViecRepository.GetCaLamViecByTenCaAsync(shiftNames);

                _caLamViecCache.Clear();
                foreach (var shift in allShifts)
                {
                    _caLamViecCache[shift.TenCa] = shift.MaCa;
                }

                _logger.LogInformation("Loaded {Count} shifts into cache", _caLamViecCache.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi load shift cache");
                throw;
            }
        }

        #region IPricingConfig Implementation Classes

        private class ExtractedPricingConfig : IPricingConfig
        {
            public bool DongGiaAllCa { get; set; }
            public decimal? GiaThueChung { get; set; }
            public decimal? GiaThueCa1 { get; set; }
            public decimal? GiaThueCa2 { get; set; }
            public decimal? GiaThueCa3 { get; set; }
            public DateOnly NgayApDungGia { get; set; }
            public string TrangThaiGia { get; set; } = "HieuLuc";
        }

        private class EmptyPricingConfig : IPricingConfig
        {
            public bool DongGiaAllCa => false;
            public decimal? GiaThueChung => null;
            public decimal? GiaThueCa1 => null;
            public decimal? GiaThueCa2 => null;
            public decimal? GiaThueCa3 => null;
            public DateOnly NgayApDungGia => DateOnly.FromDateTime(DateTime.Today);
            public string TrangThaiGia => "HieuLuc";
        }

        #endregion
    }
}