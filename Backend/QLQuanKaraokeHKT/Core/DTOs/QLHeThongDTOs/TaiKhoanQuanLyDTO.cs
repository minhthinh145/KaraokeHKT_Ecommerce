using System.ComponentModel.DataAnnotations.Schema;

namespace QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs
{
    public class TaiKhoanQuanLyDTO
    {
        public  Guid MaTaiKhoan { get; set; }

        public  string Email { get; set; } = null!;

        public bool daKichHoat { get; set; }

        public bool daBiKhoa { get; set; }

        public string loaiTaiKhoan { get; set; } = null!;

   
    }
}
