using AutoMapper;
using QLQuanKaraokeHKT.Application.Services.Auth;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.DTOs.Core;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Core;

namespace QLQuanKaraokeHKT.Application.Services.Core
{
    public class CoreAccountService : ICoreAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CoreAccountService> _logger;
        private readonly IUserAuthenticationService _userAuthenticationService;


        public CoreAccountService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserAuthenticationService userAuthenticationService,
            ILogger<CoreAccountService> logger
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger;
            _userAuthenticationService = userAuthenticationService ?? throw new ArgumentNullException(nameof(userAuthenticationService));

        }

        public async Task<ServiceResult<TaiKhoan>> CreateAccountAsync(CreateAccountRequest createAccountRequest)
        {
            try
            {
                var emailCheck = await _userAuthenticationService.IsEmailAvailableAsync(createAccountRequest.Email);
                if (!emailCheck.IsSuccess)
                {
                    return ServiceResult<TaiKhoan>.Failure(emailCheck.Message);
                }

                _logger.LogInformation("Starting account creation for email: {Email}", createAccountRequest.Email);

                var taiKhoan = _mapper.Map<TaiKhoan>(createAccountRequest);

                var createdResult = await _unitOfWork.IdentityRepository.CreateUserAsync(taiKhoan, createAccountRequest.Password);

                if (!createdResult.Succeeded)
                {
                    return ServiceResult<TaiKhoan>.Failure("Tạo tài khoản thất bại", createdResult.Errors.Select(e => e.Description).ToArray());
                }

                await _unitOfWork.RoleRepository.AddToRoleAsync(taiKhoan, createAccountRequest.RoleCode);

                _logger.LogInformation("Account created successfully for email: {Email}", createAccountRequest.Email);

                return ServiceResult<TaiKhoan>.Success("Tạo tài khoản thành công.", taiKhoan);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account for email: {Email}", createAccountRequest.Email);
                return ServiceResult<TaiKhoan>.Failure("Lỗi hệ thống khi tạo tài khoản.");
            }
        }

        public async Task<ServiceResult> MarkAccountAsLockOrUnlockAsync(Guid accountId, bool isLock)
        {
            try
            {
                if (isLock)
                {
                    return await _unitOfWork.AccountManagementRepository.LockAccountAsync(accountId)
                        ? ServiceResult.Success("Khóa tài khoản thành công.")
                        : ServiceResult.Failure("Khóa tài khoản thất bại.");
                }
                else
                {
                    return await _unitOfWork.AccountManagementRepository.UnlockAccountAsync(accountId)
                        ? ServiceResult.Success("Mở khóa tài khoản thành công.")
                        : ServiceResult.Failure("Mở khóa tài khoản thất bại.");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi {(isLock ? "khóa" : "mở khóa")} tài khoản: {ex.Message}");
            }
        }

        public async Task<ServiceResult<TaiKhoan>> UpdateAccountAsync(UpdateAccountRequest updateAccountRequest)
        {
            try
            {
                _logger.LogInformation("Starting account update for ID: {UserId}", updateAccountRequest.Id);

                var taiKhoan = await _unitOfWork.IdentityRepository.FindByUserIDAsync(updateAccountRequest.Id.ToString());

                if (taiKhoan == null)
                {
                    return ServiceResult<TaiKhoan>.Failure("Tài khoản không tồn tại.");
                }

                _mapper.Map(updateAccountRequest, taiKhoan);

                var updateResult = await _unitOfWork.IdentityRepository.UpdateUserAsync(taiKhoan);

                if (!updateResult.Succeeded)
                {
                    return ServiceResult<TaiKhoan>.Failure("Cập nhật tài khoản thất bại", updateResult.Errors.Select(e => e.Description).ToArray());
                }

                if (updateAccountRequest.RoleCode != null)
                {
                    var updateRole = await _unitOfWork.RoleRepository.UpdateUserRoleAsync(taiKhoan, updateAccountRequest.RoleCode);

                    if (!updateRole)
                    {
                        return ServiceResult<TaiKhoan>.Failure("Cập nhật vai trò tài khoản thất bại");
                    }
                }

                _logger.LogInformation("Account updated successfully for Email: {Email}", updateAccountRequest.Email);

                return ServiceResult<TaiKhoan>.Success("Cập nhật tài khoản thành công.", taiKhoan);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating account for Email: {Email}", updateAccountRequest.Email);
                return ServiceResult<TaiKhoan>.Failure("Lỗi hệ thống khi cập nhật tài khoản.");
            }
        }
    }
}
