using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QLQuanKaraokeHKT.AuthenticationService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<TaiKhoan> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        public AuthService(UserManager<TaiKhoan> userManager, IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
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
                Expires = DateTime.UtcNow.AddMinutes(55), // Access token có thời gian ngắn
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
            var expires = DateTime.UtcNow.AddDays(7); // Refresh token có thời gian dài hơn

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

            // Kiểm tra refresh token có tồn tại không
            if (storedToken == null)
            {
                throw new SecurityTokenException("Refresh token không hợp lệ hoặc không tồn tại.");
            }

            // Kiểm tra refresh token đã bị revoke chưa
            if (storedToken.Revoked.HasValue)
            {
                throw new SecurityTokenException("Refresh token đã bị thu hồi. Vui lòng đăng nhập lại.");
            }

            // Kiểm tra refresh token đã hết hạn chưa
            if (storedToken.ThoiGianHetHan < DateTime.UtcNow)
            {
                // Tự động revoke token hết hạn
                await _refreshTokenRepository.RevokeAsync(refreshToken);
                throw new SecurityTokenException("Refresh token đã hết hạn. Vui lòng đăng nhập lại.");
            }

            // Kiểm tra user có tồn tại không
            var user = await _userManager.FindByIdAsync(storedToken.MaTaiKhoan.ToString());
            if (user == null)
            {
                await _refreshTokenRepository.RevokeAsync(refreshToken);
                throw new SecurityTokenException("Người dùng không tồn tại. Vui lòng đăng nhập lại.");
            }

            // Kiểm tra user có bị khóa không
            if (user.daBiKhoa)
            {
                await _refreshTokenRepository.RevokeAsync(refreshToken);
                throw new SecurityTokenException("Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.");
            }

            // Kiểm tra user có được kích hoạt không
            if (!user.daKichHoat)
            {
                await _refreshTokenRepository.RevokeAsync(refreshToken);
                throw new SecurityTokenException("Tài khoản chưa được kích hoạt. Vui lòng kích hoạt tài khoản.");
            }

            // Tạo access token mới
            return await GenerateAccessTokenForUserAsync(user);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                await _refreshTokenRepository.RevokeAsync(refreshToken);
            }
        }

        public async Task RevokeAllUserRefreshTokensAsync(Guid userId)
        {
            try
            {
                await _refreshTokenRepository.RevokeAllByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                // Log error nhưng không throw để không ảnh hưởng flow chính
                // Có thể log ở đây nếu có logging service
            }
        }

        public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expires)
        {
            try
            {
                await _refreshTokenRepository.SaveAsync(userId, refreshToken, DateTime.UtcNow, expires);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Không thể lưu refresh token: {ex.Message}", ex);
            }
        }

        private string GenerateRefreshToken()
        {
            // Tạo refresh token an toàn hơn
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()) +
                   Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

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