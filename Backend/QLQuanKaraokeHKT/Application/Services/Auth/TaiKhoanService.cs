using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.AuthDTOs;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Customer;
using QLQuanKaraokeHKT.Infrastructure.Data;
using System.ComponentModel;

namespace QLQuanKaraokeHKT.Application.Services.Auth
{
    public class TaiKhoanService : ITaiKhoanService
    {
        private readonly ITaiKhoanRepository _taiKhoanRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly QlkaraokeHktContext _context;
        private readonly IKhachHangService _khachHangService;

        public TaiKhoanService(ITaiKhoanRepository taiKhoanRepository, IMapper mapper, IAuthService authService,IKhachHangService khachHangService ,QlkaraokeHktContext context)
        {
            _taiKhoanRepository = taiKhoanRepository;
            _mapper = mapper;
            _authService = authService;
            _context = context;
            _khachHangService = khachHangService ?? throw new ArgumentNullException(nameof(khachHangService));
        }

   
        public async Task<ServiceResult> CheckPasswordAsync(Guid userId, string password)
        {
            var user = await _taiKhoanRepository.FindByUserIDAsync(userId.ToString());
            if (user == null)
            {
                return ServiceResult.Failure("User not found.");
            }
            var isPasswordValid = await _taiKhoanRepository.CheckPasswordAsync(user, password);
            if (isPasswordValid)
            {
                return ServiceResult.Success("Password is correct.");
            }
            return ServiceResult.Failure("Incorrect password.");
        }

        public async Task<ServiceResult> GetProfileUserAsync(Guid userID)
        {
            var user = await _taiKhoanRepository.FindByUserIDAsync(userID.ToString());
            if (user == null)
            {
                return ServiceResult.Failure("User not found.");
            }

            await _context.Entry(user)
                .Collection(u => u.KhachHangs)
                .LoadAsync();

            var userProfile = _mapper.Map<UserProfileDTO>(user);
            return ServiceResult.Success("User found.", userProfile);
        }

        public async Task<ServiceResult> SignInAsync(SignInDTO signin)
        {
            var user = await _taiKhoanRepository.FindByEmailAsync(signin.Email);
            if (user == null || !await _taiKhoanRepository.CheckPasswordAsync(user, signin.Password))
            {
                return ServiceResult.Failure("Invalid email or password.");
            }
            if(!CheckAccountIsActive(user))
            {
               
                return ServiceResult.Failure("Account is not active. Please activate your account first.",data:false);
            }
            var (accessToken, refreshToken) = await _authService.GenerateTokensAsync(user);
            var LoginResponseDTO = new LoginResponseDTO
            {
                loaiTaiKhoan = user.loaiTaiKhoan.ToString(),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return ServiceResult.Success("Login successful.", LoginResponseDTO);
        }

        // Trong SignUpAsync service
        public async Task<ServiceResult> SignUpAsync(SignUpDTO signup)
        {
            try
            {
                var existingUser = await _taiKhoanRepository.FindByEmailSignUpAsync(signup.Email);
                if (existingUser != null)
                {
                    return ServiceResult.Failure("Email đã được sử dụng bởi tài khoản khác.");
                }

                var applicationUser = _mapper.Map<TaiKhoan>(signup);
                var khachHang = _mapper.Map<KhachHang>(signup);

                // ✅ Sử dụng method transaction-safe
                var (result, createdUser) = await _taiKhoanRepository.CreateCustomerAccountForSignUpAsync(
                    applicationUser,
                    signup.Password,
                    khachHang);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ServiceResult.Failure("Đăng ký thất bại.", errors);
                }

                var userProfile = _mapper.Map<UserProfileDTO>(createdUser);
                return ServiceResult.Success("Đăng ký thành công. Vui lòng kiểm tra email để kích hoạt tài khoản.", userProfile);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi đăng ký: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateUserById(Guid userid, UserProfileDTO user)
        {
            var userApp = await _taiKhoanRepository.FindByUserIDAsync(userid.ToString());
            if (userApp == null)
            {
                return ServiceResult.Failure("User not found.");
            }

            _mapper.Map(user, userApp);

            var result = await _taiKhoanRepository.UpdateUserAsync(userApp);
            await _khachHangService.UpdateKhachHangByTaiKhoanAsync(userApp);
            if (result.Succeeded)
            {
                var updatedUser = await _taiKhoanRepository.FindByUserIDAsync(userid.ToString());
                return ServiceResult.Success("Cập nhật user thành công.", _mapper.Map<UserProfileDTO>(updatedUser));
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return ServiceResult.Failure($"Failed to update user: {string.Join(", ", errors)}");
            }
        }

        private async Task AssignCustomerRoleAsync(TaiKhoan user)
        {
            var roleExists = await _taiKhoanRepository.GetUserRolesAsync(user);
            if (!roleExists.Contains(ApplicationRole.KhacHang))
            {
                await _taiKhoanRepository.AddToRoleAsync(user, ApplicationRole.KhacHang);
            }
        }

        private bool CheckAccountIsActive(TaiKhoan user)
        {
            if (user == null)
            {
                return false;
            }
            return user.daKichHoat && user.EmailConfirmed;
        }
    }
}