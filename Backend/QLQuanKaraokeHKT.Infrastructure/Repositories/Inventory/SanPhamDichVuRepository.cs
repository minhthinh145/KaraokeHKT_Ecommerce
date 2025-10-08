using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.Inventory;
using QLQuanKaraokeHKT.Infrastructure.Data;


namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Inventory
{
    public class SanPhamDichVuRepository : GenericRepository<SanPhamDichVu,int>,ISanPhamDichVuRepository
    {

        public SanPhamDichVuRepository(QlkaraokeHktContext context) : base(context)
        {
        }
    }
}