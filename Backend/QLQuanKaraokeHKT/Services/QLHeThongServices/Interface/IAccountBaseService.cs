using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices.Interface
{
    public interface IAccountBaseService
    {
        Task<ServiceResult> CheckEmailExistsAsync(string email);
        Task<ServiceResult> SendWelcomeEmailAsync(string email, string hoTen, string password);


    }
}