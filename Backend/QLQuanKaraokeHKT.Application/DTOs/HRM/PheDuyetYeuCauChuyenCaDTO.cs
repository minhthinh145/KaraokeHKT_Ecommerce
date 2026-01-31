using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Application.DTOs.QLNhanSuDTOs
{
    public class PheDuyetYeuCauChuyenCaDTO
    {
        [Required]
        public int MaYeuCau { get; set; }

        [Required(ErrorMessage = "Kết quả phê duyệt không được để trống")]
        public bool KetQuaPheDuyet { get; set; } // true: chấp nhận, false: từ chối

        [MaxLength(500, ErrorMessage = "Ghi chú phê duyệt không quá 500 ký tự")]
        public string? GhiChuPheDuyet { get; set; }
    }
}