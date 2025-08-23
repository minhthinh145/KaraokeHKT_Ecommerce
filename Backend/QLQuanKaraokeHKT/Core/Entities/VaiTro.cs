using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLQuanKaraokeHKT.Core.Entities
{
    [Table("VaiTro")]
    public class VaiTro : IdentityRole<Guid>
    {
        [Column("moTa")]
        public string? moTa { get; set; }
    }
}