using QLQuanKaraokeHKT.Core.Interfaces.Services.Common;

namespace QLQuanKaraokeHKT.Core.DTOs.QLKhoDTOs
{
    public static class VatLieuDTOExtensions
    {
        public static IPricingConfig AsPricingConfig(this AddVatLieuDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
                
            return new VatLieuPricingConfig(dto);
        }

        public static IPricingConfig AsPricingConfig(this UpdateVatLieuDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
                
            return new UpdateVatLieuPricingConfig(dto);
        }

        private class VatLieuPricingConfig : IPricingConfig
        {
            private readonly AddVatLieuDTO _dto;

            public VatLieuPricingConfig(AddVatLieuDTO dto)
            {
                _dto = dto ?? throw new ArgumentNullException(nameof(dto));
            }

            public bool DongGiaAllCa => _dto.DongGiaAllCa;
            public decimal? GiaThueChung => _dto.GiaBanChung;
            public decimal? GiaThueCa1 => _dto.GiaBanCa1;
            public decimal? GiaThueCa2 => _dto.GiaBanCa2;
            public decimal? GiaThueCa3 => _dto.GiaBanCa3;
            public DateOnly NgayApDungGia => _dto.NgayApDungGiaBan;
            public string TrangThaiGia => _dto.TrangThaiGiaBan;
        }

        private class UpdateVatLieuPricingConfig : IPricingConfig
        {
            private readonly UpdateVatLieuDTO _dto;

            public UpdateVatLieuPricingConfig(UpdateVatLieuDTO dto)
            {
                _dto = dto ?? throw new ArgumentNullException(nameof(dto));
            }

            public bool DongGiaAllCa => _dto.DongGiaAllCa ?? false;
            public decimal? GiaThueChung => _dto.GiaBanChung;
            public decimal? GiaThueCa1 => _dto.GiaBanCa1;
            public decimal? GiaThueCa2 => _dto.GiaBanCa2;
            public decimal? GiaThueCa3 => _dto.GiaBanCa3;
            public DateOnly NgayApDungGia => _dto.NgayApDungGiaBan ?? DateOnly.FromDateTime(DateTime.Today);
            public string TrangThaiGia => _dto.TrangThaiGiaBan ?? "HieuLuc";
        }
    }
}