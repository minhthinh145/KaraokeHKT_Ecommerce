using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Customer;
using QLQuanKaraokeHKT.Infrastructure.Data;

namespace QLQuanKaraokeHKT.Application.Services.Auth
{
    /// <summary>
    /// User Profile Service - Migrated from TaiKhoanService
    /// Handles ONLY profile-related operations
    /// </summary>
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IKhachHangService _khachHangService;
        private readonly QlkaraokeHktContext _context;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IKhachHangService khachHangService,
            QlkaraokeHktContext context,
            ILogger<UserProfileService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _khachHangService = khachHangService ?? throw new ArgumentNullException(nameof(khachHangService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult> GetUserProfileAsync(Guid userId)
        {
            try
            {
                var user = await _unitOfWork.IdentityRepository.FindByUserIDAsync(userId.ToString());
                if (user == null)
                {
                    return ServiceResult.Failure("User not found.");
                }

                // Load related customer data - exactly like original
                await _context.Entry(user)
                    .Collection(u => u.KhachHangs)
                    .LoadAsync();

                var userProfile = _mapper.Map<UserProfileDTO>(user);
                return ServiceResult.Success("User found.", userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile for user: {UserId}", userId);
                return ServiceResult.Failure("System error while retrieving user profile.");
            }
        }

        public async Task<ServiceResult> UpdateUserProfileAsync(Guid userId, UserProfileDTO userProfileDto)
        {
            return await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                try
                {
                    var userApp = await _unitOfWork.IdentityRepository.FindByUserIDAsync(userId.ToString());
                    if (userApp == null)
                    {
                        return ServiceResult.Failure("User not found.");
                    }

                    _mapper.Map(userProfileDto, userApp);

                    // Update user account
                    var result = await _unitOfWork.IdentityRepository.UpdateUserAsync(userApp);
                    if (!result.Succeeded)
                    {
                        var errors = result.Errors.Select(e => e.Description).ToArray();
                        return ServiceResult.Failure($"Failed to update user: {string.Join(", ", errors)}");
                    }

                    await _khachHangService.UpdateKhachHangByTaiKhoanAsync(userApp);

                    var updatedUser = await _unitOfWork.IdentityRepository.FindByUserIDAsync(userId.ToString());
                    var updatedProfile = _mapper.Map<UserProfileDTO>(updatedUser);

                    return ServiceResult.Success("Cập nhật user thành công.", updatedProfile);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating user profile for user: {UserId}", userId);
                    return ServiceResult.Failure("System error while updating user profile.");
                }
            });
        }
    }
}