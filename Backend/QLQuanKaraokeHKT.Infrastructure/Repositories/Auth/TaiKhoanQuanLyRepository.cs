using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.Auth;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth
{
    public class TaiKhoanQuanLyRepository : ITaiKhoanQuanLyRepository
    {
        private readonly UserManager<TaiKhoan> _userManager;
        private readonly IIdentityRepository _identityRepository;

        public TaiKhoanQuanLyRepository(IIdentityRepository identityRepository, UserManager<TaiKhoan> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository)); 
        }

        public async Task<List<TaiKhoan>> GettAllAdminAccount()
        {
          var managerRoles = GetAllManagerRoles();
            var allUsers = await _userManager.Users.ToListAsync();
            var managerUsers = new List<TaiKhoan>();
            foreach (var user in allUsers)
            {
                var roles = await _identityRepository.GetUserRolesAsync(user);
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
