using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QLQuanKaraokeHKT.Domain.Entities;
using QLQuanKaraokeHKT.Application.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QLQuanKaraokeHKT.Application.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<TaiKhoan> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        private readonly ILogger<TokenService> _logger;

        public TokenService(UserManager<TaiKhoan> userManager, IConfiguration configuration, 
            IRefreshTokenRepository refreshTokenRepository,
            ILogger<TokenService> logger
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
        }

        public async Task<string> GenerateAccessTokenAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            return await GenerateAccessTokenForUserAsync(user);
        }

        public async Task<string> GenerateAccessTokenForUserAsync(TaiKhoan user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authKey = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddHours(10), 
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(authKey), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(TaiKhoan user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Revoke all existing refresh tokens của user này để tránh conflict
            await RevokeAllUserRefreshTokensAsync(user.Id);

            var accessToken = await GenerateAccessTokenForUserAsync(user);
            var refreshToken = GenerateRefreshToken();
            var expires = DateTime.UtcNow.AddDays(7); 

            await SaveRefreshTokenAsync(user.Id, refreshToken, expires);
            return (accessToken, refreshToken);
        }

        public async Task<string> RefreshAccessTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new SecurityTokenException("Refresh token không được để trống.");
            }

            var storedToken = await _refreshTokenRepository.FindByTokenAsync(refreshToken);
            if (storedToken == null)
            {
                throw new SecurityTokenException("Refresh token không hợp lệ hoặc không tồn tại.");
            }

            if (storedToken.Revoked.HasValue)
            {
                throw new SecurityTokenException("Refresh token đã bị thu hồi. Vui lòng đăng nhập lại.");
            }

            if (storedToken.ThoiGianHetHan < DateTime.UtcNow)
            {
                var revokeSuccess = await _refreshTokenRepository.RevokeTokenAsync(refreshToken);
                if (!revokeSuccess)
                {
                   _logger?.LogWarning($"Failed to revoke expired token: {refreshToken}");
                }
                throw new SecurityTokenException("Refresh token đã hết hạn. Vui lòng đăng nhập lại.");
            }

            var user = await ValidateUserForRefreshAsync(storedToken.MaTaiKhoan, refreshToken);

            return await GenerateAccessTokenForUserAsync(user);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                await _refreshTokenRepository.RevokeTokenAsync(refreshToken);
            }
        }

        public async Task<bool> RevokeAllUserRefreshTokensAsync(Guid userId)
        {
            try
            {
                var revokedCount = await _refreshTokenRepository.RevokeAllByUserIdAsync(userId);
                return revokedCount > 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to revoke all refresh tokens for user {UserId}", userId);
                return false;
            }
        }

        public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expires)
        {
            try
            {
               var newToken = new RefreshToken
                {
                    MaTaiKhoan = userId,
                    ChuoiRefreshToken = refreshToken,
                    ThoiGianTao = DateTime.UtcNow,
                    ThoiGianHetHan = expires,
                    Revoked = null
                };
                await _refreshTokenRepository.CreateAsync(newToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Không thể lưu refresh token: {ex.Message}", ex);
            }
        }

        #region private helper

        private string GenerateRefreshToken()
        {
            // Tạo refresh token an toàn hơn
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()) +
                   Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private async Task<TaiKhoan> ValidateUserForRefreshAsync(Guid userId, string refreshToken)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                await _refreshTokenRepository.RevokeTokenAsync(refreshToken);
                throw new SecurityTokenException("Người dùng không tồn tại. Vui lòng đăng nhập lại.");
            }

            if (user.daBiKhoa)
            {
                await _refreshTokenRepository.RevokeTokenAsync(refreshToken);
                throw new SecurityTokenException("Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.");
            }

            if (!user.daKichHoat)
            {
                await _refreshTokenRepository.RevokeTokenAsync(refreshToken);
                throw new SecurityTokenException("Tài khoản chưa được kích hoạt. Vui lòng kích hoạt tài khoản.");
            }

            return user;
        }

        #endregion

        public async Task<bool> IsRefreshTokenValidAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;

            return await _refreshTokenRepository.IsTokenValidAsync(refreshToken);
        }

        public async Task<int> CleanupExpiredRefreshTokensAsync()
        {
            return await _refreshTokenRepository.CleanupExpiredTokensAsync();
        }
    }
}