using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Inventory
{
    public class SanPhamDichVuRepository : GenericRepository<SanPhamDichVu,int>,ISanPhamDichVuRepository
    {

        public SanPhamDichVuRepository(QlkaraokeHktContext context) : base(context)
        {
        }
    }
}