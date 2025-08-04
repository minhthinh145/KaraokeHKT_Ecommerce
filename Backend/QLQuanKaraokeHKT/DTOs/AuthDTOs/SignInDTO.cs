using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.DTOs.AuthDTOs
{
    public class SignInDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
