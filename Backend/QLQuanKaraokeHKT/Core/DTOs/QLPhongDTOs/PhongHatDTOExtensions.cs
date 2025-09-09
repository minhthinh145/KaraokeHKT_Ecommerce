using QLQuanKaraokeHKT.Core.Interfaces.Services.Common;

namespace QLQuanKaraokeHKT.Core.DTOs.QLPhongDTOs
{

    public static class PhongHatDTOExtensions
    {
  
        public static IPricingConfig AsPricingConfig(this AddPhongHatDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
                
            return new AddPhongHatPricingConfig(dto);
        }

  
        public static IPricingConfig AsPricingConfig(this UpdatePhongHatDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
                
            return new UpdatePhongHatPricingConfig(dto);
        }

        #region Private Implementation Classes

        private class AddPhongHatPricingConfig : IPricingConfig
        {
            private readonly AddPhongHatDTO _dto;

            public AddPhongHatPricingConfig(AddPhongHatDTO dto)
            {
                _dto = dto ?? throw new ArgumentNullException(nameof(dto));
            }

            public bool DongGiaAllCa => _dto.DongGiaAllCa;
            public decimal? GiaThueChung => _dto.GiaThueChung;
            public decimal? GiaThueCa1 => _dto.GiaThueCa1;
            public decimal? GiaThueCa2 => _dto.GiaThueCa2;
            public decimal? GiaThueCa3 => _dto.GiaThueCa3;
            public DateOnly NgayApDungGia => _dto.NgayApDungGia;
            public string TrangThaiGia => _dto.TrangThaiGia;
        }

        private class UpdatePhongHatPricingConfig : IPricingConfig
        {
            private readonly UpdatePhongHatDTO _dto;

            public UpdatePhongHatPricingConfig(UpdatePhongHatDTO dto)
            {
                _dto = dto ?? throw new ArgumentNullException(nameof(dto));
            }

            public bool DongGiaAllCa => _dto.DongGiaAllCa ?? false;
            public decimal? GiaThueChung => _dto.GiaThueChung;
            public decimal? GiaThueCa1 => _dto.GiaThueCa1;
            public decimal? GiaThueCa2 => _dto.GiaThueCa2;
            public decimal? GiaThueCa3 => _dto.GiaThueCa3;
            public DateOnly NgayApDungGia => _dto.NgayApDungGia ?? DateOnly.FromDateTime(DateTime.Today);
            public string TrangThaiGia => _dto.TrangThaiGia ?? "HieuLuc";
        }

        #endregion
    }
}