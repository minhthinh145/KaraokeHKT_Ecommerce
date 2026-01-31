using System.ComponentModel.DataAnnotations.Schema;

namespace QLQuanKaraokeHKT.Application.DTOs.QLHeThongDTOs
{
    public class AddAccountForAdminDTO
    {

        public  string UserName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string loaiTaiKhoan { get; set; } = null!;

    }
}
