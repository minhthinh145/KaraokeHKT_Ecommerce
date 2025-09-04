using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Room;

namespace QLQuanKaraokeHKT.Application.Services.Room
{
    public class RoomPricingService : IRoomPricingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoomPricingService> _logger;
        private Dictionary<string, int> _caLamViecCache = new Dictionary<string, int>();

        public RoomPricingService(IUnitOfWork unitOfWork, ILogger<RoomPricingService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // ✅ EXISTING METHODS (unchanged)
        public async Task ApplyPricingConfigAsync(int maSanPham, IPricingConfig pricingConfig)
        {
            try
            {
                if (pricingConfig == null)
                    throw new ArgumentNullException(nameof(pricingConfig));

                _logger.LogInformation("Áp dụng cấu hình giá mới cho sản phẩm ID: {MaSanPham}", maSanPham);

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

        // ✅ NEW METHOD: Extract Pricing Config from GiaDichVu list
        public IPricingConfig ExtractPricingConfig(List<GiaDichVu> prices)
        {
            if (!prices?.Any() == true)
            {
                _logger.LogDebug("Không có giá nào để extract, trả về empty config");
                return new EmptyPricingConfig();
            }

            try
            {
                _logger.LogDebug("Extracting pricing config từ {Count} giá", prices.Count);

                var flatRate = prices.FirstOrDefault(p => p.MaCa == null);
                var hasShiftPricing = prices.Any(p => p.MaCa != null);

                var config = new ExtractedPricingConfig
                {
                    DongGiaAllCa = flatRate != null,
                    GiaThueChung = flatRate?.DonGia,
                    GiaThueCa1 = GetPriceByCaName(prices, "Ca 1"),
                    GiaThueCa2 = GetPriceByCaName(prices, "Ca 2"),
                    GiaThueCa3 = GetPriceByCaName(prices, "Ca 3"),
                    NgayApDungGia = GetLatestApplyDate(prices),
                    TrangThaiGia = "HieuLuc"
                };

                _logger.LogDebug("Extracted pricing - Flat: {HasFlat}, Ca1: {Ca1}, Ca2: {Ca2}, Ca3: {Ca3}",
                    flatRate != null, config.GiaThueCa1.HasValue, config.GiaThueCa2.HasValue, config.GiaThueCa3.HasValue);

                return config;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi extract pricing config");
                return new EmptyPricingConfig();
            }
        }

        private decimal? GetPriceByCaName(List<GiaDichVu> prices, string caName)
        {
            try
            {
                return prices.FirstOrDefault(p => p.MaCaNavigation?.TenCa == caName)?.DonGia;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Lỗi khi lấy giá cho ca {CaName}", caName);
                return null;
            }
        }

        private DateOnly GetLatestApplyDate(List<GiaDichVu> prices)
        {
            try
            {
                return prices.Any() ? prices.Max(p => p.NgayApDung) : DateOnly.FromDateTime(DateTime.Today);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Lỗi khi lấy ngày áp dụng mới nhất");
                return DateOnly.FromDateTime(DateTime.Today);
            }
        }

        // ✅ EXISTING PRIVATE METHODS (unchanged)
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

        // ✅ IMPLEMENTATION CLASS FOR EXTRACTED PRICING
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