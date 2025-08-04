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
                Expires = DateTime.UtcNow.AddMinutes(55),
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(authKey), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(TaiKhoan user)
        {
            var accessToken = await GenerateAccessTokenForUserAsync(user);
            var refreshToken = Guid.NewGuid().ToString();
            var expires = DateTime.UtcNow.AddDays(7);
            await SaveRefreshTokenAsync(user.Id, refreshToken, expires);
            return (accessToken, refreshToken);
        }

        public async Task<string> RefreshAccessTokenAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.FindByTokenAsync(refreshToken);
            if (storedToken == null || !storedToken.IsActive)
            {
                throw new SecurityTokenException("Invalid refresh token.");
            }
            var user = await _userManager.FindByIdAsync(storedToken.MaTaiKhoan.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {storedToken.MaTaiKhoan} not found.");
            }
            return await GenerateAccessTokenForUserAsync(user);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            await _refreshTokenRepository.RevokeAsync(refreshToken);
        }

        public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expires)
        {
            await _refreshTokenRepository.SaveAsync(userId, refreshToken, DateTime.UtcNow, expires);
        }
    }
}