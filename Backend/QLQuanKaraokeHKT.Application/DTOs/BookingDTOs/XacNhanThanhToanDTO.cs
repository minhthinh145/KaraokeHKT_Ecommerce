using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Application.DTOs.BookingDTOs
{
    public class XacNhanThanhToanDTO
    {
        [Required(ErrorMessage = "Mã hóa đơn không được để trống")]
        public Guid MaHoaDon { get; set; }

        [Required(ErrorMessage = "Mã thuê phòng không được để trống")]
        public Guid MaThuePhong { get; set; }
    }
}