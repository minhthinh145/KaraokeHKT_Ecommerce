namespace QLQuanKaraokeHKT.Core.DTOs
{
    public class CreateUserOtpDTO
    {
        public Guid MaTaiKhoan { get; set; }
        public string maOTP { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
