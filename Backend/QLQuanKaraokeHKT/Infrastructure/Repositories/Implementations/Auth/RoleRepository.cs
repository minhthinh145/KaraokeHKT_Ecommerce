using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Base;

namespace QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth
{
    public class RoleRepository : GenericRepository<VaiTro,Guid>,IRoleRepository
    {
        private readonly UserManager<TaiKhoan> _userManager;
        private readonly RoleManager<VaiTro> _roleManager;
        public RoleRepository(UserManager<TaiKhoan> userManager, RoleManager<VaiTro> roleManager, QlkaraokeHktContext context) : base(context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        #region GetActions
        public async Task<string> GetRoleDescriptionAsync(string roleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                return role?.moTa ?? roleName;
            }
            catch
            {
                return roleName;
            }
        }

        public async Task<string?> GetRoleCodeFromDescriptionAsync(string roleDescription)
        {
            var role = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.moTa == roleDescription || r.Name == roleDescription);
            if (role != null)
            {
                return role.Name;
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region Create-Update-Delete Actions
        public async Task CreateRoleAsync(string roleName)
        {
            await _roleManager.CreateAsync(new VaiTro { Name = roleName, NormalizedName = roleName.ToUpper() });
        }

        public async Task AddToRoleAsync(TaiKhoan user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> UpdateUserRoleAsync(TaiKhoan user, string newRoleName)
        {
            try
            {
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Xóa tất cả role hiện tại
                if (currentRoles.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                        return false;
                }

                var addResult = await _userManager.AddToRoleAsync(user, newRoleName);
                return addResult.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task RemoveFromRoleAsync(TaiKhoan user, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }

       
        #endregion

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}

