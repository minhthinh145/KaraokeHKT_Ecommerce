using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;

namespace QLQuanKaraokeHKT.Core.Interfaces.Services.Auth
{
    public interface IAuthOrchestrator
    {

        Task<ServiceResult> ExecuteSignInWorkflowAsync(SignInDTO signInDto);

        Task<ServiceResult> ExecuteSignUpWorkflowAsync(SignUpDTO signUpDto);

        Task<ServiceResult> ExecuteAccountVerificationWorkflowAsync(VerifyAccountDTO verifyDto);

        Task<ServiceResult> ExecutePasswordChangeWorkflowAsync(Guid userId, ChangePasswordDTO changePasswordDto);

        Task<ServiceResult> ExecutePasswordChangeConfirmationWorkflowAsync(Guid userId, ConfirmChangePasswordDTO confirmDto);

        Task<ServiceResult> ExecuteSignOutWorkflowAsync(Guid userId, string refreshToken);
    }
}