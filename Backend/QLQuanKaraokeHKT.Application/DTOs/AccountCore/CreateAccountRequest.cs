namespace QLQuanKaraokeHKT.Application.DTOs.Core
{
    public class CreateAccountRequest
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Password { get; set; } = string.Empty;
        public string RoleCode { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool RequireEmailVerification { get; set; } = true;
    }
}
