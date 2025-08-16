using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs
{
    public class AddLichLamViecDTO
    {
        [Required(ErrorMessage = "Ngày làm việc không được để trống.")]
        public DateOnly NgayLamViec { get; set; }

        [Required(ErrorMessage = "Mã nhân viên không được để trống.")]
        public Guid MaNhanVien { get; set; }

        [Required(ErrorMessage = "Mã ca làm việc không được để trống.")]
        public int MaCa { get; set; }
    }
}
