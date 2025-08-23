using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth
{
    public class TaiKhoanQuanLyRepository : ITaiKhoanQuanLyRepository
    {
        private readonly ITaiKhoanRepository _taiKhoanRepository;
        private readonly UserManager<TaiKhoan> _userManager;

        public TaiKhoanQuanLyRepository(ITaiKhoanRepository taiKhoanRepository, UserManager<TaiKhoan> userManager)
        {
            _taiKhoanRepository = taiKhoanRepository ?? throw new ArgumentNullException(nameof(taiKhoanRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<List<TaiKhoan>> GettAllAdminAccount()
        {
          var managerRoles = GetAllManagerRoles();
            var allUsers = await _userManager.Users.ToListAsync();
            var managerUsers = new List<TaiKhoan>();
            foreach (var user in allUsers)
            {
                var roles = await _taiKhoanRepository.GetUserRolesAsync(user);
                if (roles.Any(role => managerRoles.Contains(role)))
                {
                    managerUsers.Add(user);
                }
            }
            return managerUsers;

        }

        private List<string> GetAllManagerRoles()
        {
            var managerRoles = new List<string>
                {
                    ApplicationRole.QuanTriHeThong,
                    ApplicationRole.QuanLyKho,
                    ApplicationRole.QuanLyNhanSu,
                    ApplicationRole.QuanLyPhongHat
                };
            return managerRoles;
        }
    }
}
