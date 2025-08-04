namespace QLQuanKaraokeHKT.DTOs.AuthDTOs
{
    public class UserProfileDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string LoaiTaiKhoan { get; set; } 
        public string BirthDateFormatted => BirthDate?.ToString("dd/MM/yyyy");
    }
}
