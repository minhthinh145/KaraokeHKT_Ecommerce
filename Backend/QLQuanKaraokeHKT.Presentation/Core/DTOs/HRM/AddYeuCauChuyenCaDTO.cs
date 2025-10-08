using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs
{
    public class AddYeuCauChuyenCaDTO
    {
        [Required(ErrorMessage = "Mã lịch làm việc gốc không được để trống")]
        public int MaLichLamViecGoc { get; set; }

        [Required(ErrorMessage = "Ngày làm việc mới không được để trống")]
        public DateOnly NgayLamViecMoi { get; set; }

        [Required(ErrorMessage = "Ca mới không được để trống")]
        public int MaCaMoi { get; set; }

        [MaxLength(500, ErrorMessage = "Lý do chuyển ca không quá 500 ký tự")]
        public string? LyDoChuyenCa { get; set; }
    }
}
