using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Base;

namespace QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth
{
    public interface ISignUpRepository : IGenericRepository<TaiKhoan, Guid>
    {
        Task<TaiKhoan?> FindByEmailSignUpAsync(string email);

        Task<bool> IsEmailExistsForSignUpAsync(string email);

        Task<(IdentityResult Result, TaiKhoan? User)> CreateCustomerAccountForSignUpAsync(
            TaiKhoan user,
            string password,
            KhachHang khachHang);
    }
}
