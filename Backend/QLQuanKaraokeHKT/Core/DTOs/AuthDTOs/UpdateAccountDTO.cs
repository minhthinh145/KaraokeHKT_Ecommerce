namespace QLQuanKaraokeHKT.Core.DTOs.AuthDTOs
{
    public class UpdateAccountDTO
    {
        public Guid maTaiKhoan { get; set; } 
        public string newUserName { get; set; } = string.Empty;
        public string newPassword { get; set; } = string.Empty;
        public string newLoaiTaiKhoan { get; set; } = string.Empty;
    }
}
