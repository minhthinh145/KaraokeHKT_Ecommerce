using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    public interface IChangePasswordService
    {
        /// <summary>
        ///  Checks the password change request and ask User to choice method to receive OTP
        /// </summary>
        /// <param name="changePasswordDTO">Data including UserId , newPassword and confirmPassword</param>
        /// <returns>Notification of error ( if false )</returns>
        Task<ServiceResult> RequestChangePasswordAsync(ChangePasswordDTO changePasswordDTO , string userID);

        /// <summary>
        /// Verify OTP and Change Password if vaild
        /// </summary>
        /// <param name="verifyOtpAndChangePasswordDTO">Data including UserId , OTP code and newPassword </param>
        /// <returns>A confirm message or an error message if applicable </returns>
        Task<ServiceResult> ConfirmChangePasswordAsync(string userId, ConfirmChangePasswordDTO ConfirmChangePasswordDTO);
    }
}
