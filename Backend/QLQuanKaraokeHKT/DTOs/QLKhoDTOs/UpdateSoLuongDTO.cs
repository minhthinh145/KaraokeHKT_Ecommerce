using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.DTOs.QLKhoDTOs
{
    public class UpdateSoLuongDTO
    {
        [Required]
        public int MaVatLieu { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải >= 0")]
        public int SoLuongMoi { get; set; }
    }
}
