using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.Interfaces;

namespace QLQuanKaraokeHKT.Repositories.Implementation
{
    public class PhongHatKaraokeRepository : IPhongHatKaraokeRepository
    {
        private readonly QlkaraokeHktContext _context;

        public PhongHatKaraokeRepository(QlkaraokeHktContext context)
        {
            _context = context;
        }

        public async Task<bool> DeletePhongHatKaraokeAsync(string KaraokeRoom_ID)
        {
           return await _context.PhongHatKaraokes
                .Where(x => x.MaPhong.ToString() == KaraokeRoom_ID)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<PhongHatKaraoke> FindPhongHatKaraokeByIdAsync(string KaraokeRoom_ID)
        {
            var result = await _context.PhongHatKaraokes.FirstOrDefaultAsync(x => x.MaPhong.ToString() == KaraokeRoom_ID);
            if (result == null)
            {
                throw new KeyNotFoundException($"Karaoke Room with ID {KaraokeRoom_ID} not found.");
            }
            return result;
        }

        public async Task<List<PhongHatKaraoke>> GetAllPhongHatKarokeAsync()
        {
            return await _context.PhongHatKaraokes
                .Where(x => x.DangSuDung == false)
                .ToListAsync();
        }

        public async Task<bool> UpdatePhongHatKaraokeAsync(PhongHatKaraoke phongHatKaraoke)
        {
            var result =  _context.PhongHatKaraokes.Update(phongHatKaraoke);
            if (result.State == EntityState.Modified)
            {
                return await _context.SaveChangesAsync() > 0;
            }
            else
            {
                throw new InvalidOperationException("Cập nhật phòng thất bại");
            }
        }
    }
}
