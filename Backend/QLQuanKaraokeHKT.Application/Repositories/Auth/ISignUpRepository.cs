using Microsoft.AspNetCore.Identity;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories;

namespace QLQuanKaraokeHKT.Application.Repositories.Auth
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
