using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.DTOs.QLPhongDTOs
{
    public class AddLoaiPhongDTO
    {
        [Required(ErrorMessage = "Tên loại phòng không được để trống")]
        [MaxLength(50, ErrorMessage = "Tên loại phòng không quá 50 ký tự")]
        public string TenLoaiPhong { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Sức chứa phải lớn hơn 0")]
        public int SucChua { get; set; }
    }
}
