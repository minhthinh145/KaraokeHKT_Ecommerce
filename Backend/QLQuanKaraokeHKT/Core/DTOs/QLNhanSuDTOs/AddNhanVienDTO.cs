using System.ComponentModel.DataAnnotations;

namespace QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs
{
    public class AddNhanVienDTO
    {
        [Required]
        /// <summary>
        /// Gets or sets the full name of the employee.
        /// </summary>
        public string HoTen { get; set; } = string.Empty;
        [Required]

        /// <summary>
        /// Gets or sets the phone number of the employee.
        /// </summary>
        public string SoDienThoai { get; set; } = string.Empty;
        [Required]

        /// <summary>
        /// Gets or sets the email address of the employee.
        /// </summary>
        public string Email { get; set; } = string.Empty;
       
        /// <summary>
        /// Gets or sets the date of birth of the employee.
        /// </summary>
        [Required]

        public DateTime NgaySinh { get; set; }
        [Required]
        public string LoaiTaiKhoan { get; set; } = string.Empty;

    }
}