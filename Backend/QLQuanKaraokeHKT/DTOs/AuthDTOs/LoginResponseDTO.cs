namespace QLQuanKaraokeHKT.DTOs.AuthDTOs
{
    public class LoginResponseDTO
    {
        public string loaiTaiKhoan { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
