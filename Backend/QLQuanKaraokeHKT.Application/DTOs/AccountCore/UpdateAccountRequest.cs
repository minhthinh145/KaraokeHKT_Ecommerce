namespace QLQuanKaraokeHKT.Application.DTOs.Core
{
    public class UpdateAccountRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? RoleCode { get; set; }
        public DateOnly? NgaySinh { get; set; }
    }
}
