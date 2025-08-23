using QLQuanKaraokeHKT.Core.Common;


namespace QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement
{
    public interface IAccountBaseService
    {
        Task<ServiceResult> CheckEmailExistsAsync(string email);
        Task<ServiceResult> SendWelcomeEmailAsync(string email, string hoTen, string password);

    }
}