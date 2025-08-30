using AutoMapper;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Infrastructure;

namespace QLQuanKaraokeHKT.Application.Services.Auth
{
    /// <summary>
    /// User Authentication Service: Handle user authentication logic
    /// </summary>
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserAuthenticationService> _logger;
        public UserAuthenticationService(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IMapper mapper,
            ILogger<UserAuthenticationService> logger
            )
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TaiKhoan?> ValidateUserCredentialsAsync(string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    _logger.LogWarning("Empty email or password provided");
                    return null;
                }

                var user = await _unitOfWork.IdentityRepository.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("User not found with email: {Email}", email);
                    return null;
                }

                var isPasswordValid = await _unitOfWork.IdentityRepository.CheckPasswordAsync(user, password);
                if (!isPasswordValid)
                {
                    _logger.LogWarning("Invalid password for user with email: {Email}", email);
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user credentials for email: {Email}", email);
                return null;
            }
        }

        public bool IsAccountActive(TaiKhoan user)
        {
            if (user == null)
            {
                return false;
            }

            return user.daKichHoat && user.EmailConfirmed && !user.daBiKhoa;
        }

        public async Task<bool> CheckPasswordAsync(Guid userId, string password)
        {
            try
            {
                var user = await _unitOfWork.IdentityRepository.FindByUserIDAsync(userId.ToString());
                if (user == null)
                {
                    _logger.LogWarning("User not found for password check: {UserId}", userId);
                    return false;
                }

                return await _unitOfWork.IdentityRepository.CheckPasswordAsync(user, password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking password for user: {UserId}", userId);
                return false;
            }
        }

        public async Task<LoginResponseDTO> GenerateLoginResponseAsync(TaiKhoan user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                var (accessToken, refreshToken) = await _tokenService.GenerateTokensAsync(user);

                return new LoginResponseDTO
                {
                    loaiTaiKhoan = user.loaiTaiKhoan.ToString(),
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating login response for user: {UserId}", user?.Id);
                throw;
            }
        }
    }
}
