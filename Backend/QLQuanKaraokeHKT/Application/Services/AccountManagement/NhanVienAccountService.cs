using AutoMapper;
using QLQuanKaraokeHKT.Application.Helpers;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs;
using QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM;
using QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement;

namespace QLQuanKaraokeHKT.Application.Services.AccountManagement
{
    public class NhanVienAccountService : INhanVienAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountBaseService _accountBaseService;

        public NhanVienAccountService(
            IUnitOfWork unitOfWork,
            IAccountBaseService accountBaseService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountBaseService = accountBaseService ?? throw new ArgumentNullException(nameof(accountBaseService));
        }

        public async Task<ServiceResult> GetAllTaiKhoanNhanVienAsync()
        {
            try
            {
                var nhanViens = await _unitOfWork.NhanVienRepository.GetAllNhanVienWithTaiKhoanAsync(); 
                if (nhanViens == null || !nhanViens.Any())
                {
                    return ServiceResult.Failure("Không có nhân viên nào trong hệ thống.");
                }

                var nhanVienTaiKhoanDTOs = _mapper.Map<IEnumerable<NhanVienTaiKhoanDTO>>(nhanViens);
                return ServiceResult.Success("Lấy danh sách tài khoản nhân viên thành công.", nhanVienTaiKhoanDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi lấy danh sách tài khoản nhân viên: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetAllTaiKhoanNhanVienByLoaiTaiKhoanAsync(string loaiTaiKhoan)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loaiTaiKhoan))
                {
                    return ServiceResult.Failure("Loại tài khoản không được để trống.");
                }

                var nhanViens = await _unitOfWork.NhanVienRepository.GetAllByLoaiTaiKhoanWithTaiKhoanAsync(loaiTaiKhoan); 
                if (nhanViens == null || !nhanViens.Any())
                {
                    return ServiceResult.Failure($"Không có nhân viên nào với loại tài khoản '{loaiTaiKhoan}'.");
                }

                var nhanVienTaiKhoanDTOs = _mapper.Map<IEnumerable<NhanVienTaiKhoanDTO>>(nhanViens);
                return ServiceResult.Success($"Lấy danh sách nhân viên loại '{loaiTaiKhoan}' thành công.", nhanVienTaiKhoanDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi lấy danh sách nhân viên theo loại tài khoản: {ex.Message}");
            }
        }

        public async Task<ServiceResult> AddTaiKhoanForNhanVienAsync(AddTaiKhoanForNhanVienDTO request)
        {
            try
            {
                // 1. Validate input
                var validationResult = ValidationHelper.ValidateAddTaiKhoanForNhanVien(request);
                if (!validationResult.IsSuccess)
                    return validationResult;

                // 2. Lấy thông tin nhân viên
                var existingNhanVien = await _unitOfWork.NhanVienRepository.GetNhanVienByIdAsync(request.MaNhanVien);
                if (existingNhanVien == null)
                {
                    return ServiceResult.Failure("Không tìm thấy nhân viên.");
                }

                // 3. Lấy tài khoản cũ và role của nó
                TaiKhoan? oldAccount = null;
                string roleCode = ApplicationRole.NhanVienPhucVu; // Default role

                if (existingNhanVien.MaTaiKhoan != Guid.Empty)
                {
                    oldAccount = await _unitOfWork.IdentityRepository.FindByUserIDAsync(existingNhanVien.MaTaiKhoan.ToString());
                    if (oldAccount != null)
                    {
                        // Lấy role từ tài khoản cũ
                        roleCode = oldAccount.loaiTaiKhoan ?? ApplicationRole.NhanVienPhucVu;
                    }
                }

                // 4. Kiểm tra email mới không trùng
                var emailCheckResult = await _accountBaseService.CheckEmailExistsAsync(request.Email);
                if (!emailCheckResult.IsSuccess)
                    return emailCheckResult;

                // 5. Tạo tài khoản mới với role từ tài khoản cũ
                var newAccountResult = await CreateTaiKhoanForExistingNhanVienAsync(request, roleCode);
                if (!newAccountResult.IsSuccess)
                    return newAccountResult;

                var newAccount = (TaiKhoan)newAccountResult.Data;

                // 6. Link tài khoản mới với nhân viên
                var linkResult = await LinkTaiKhoanToNhanVienAsync(existingNhanVien, newAccount, request);
                if (!linkResult.IsSuccess)
                {
                    // Nếu link thất bại, xóa tài khoản mới đã tạo
                    await _unitOfWork.AccountManagementRepository.DeleteUserAsync(newAccount);
                    return linkResult;
                }

                // 7. Xóa tài khoản cũ (sau khi link thành công)
                if (oldAccount != null)
                {
                    var deleteOldResult = await _unitOfWork.AccountManagementRepository.DeleteUserWithRelatedDataAsync(oldAccount);
                    if (!deleteOldResult)
                    {
                        // Log warning nhưng không fail vì tài khoản mới đã tạo thành công
                    }
                }

                // 8. Gửi email thông báo
                var formattedPassword = PasswordHelper.FormatPassword(request.Password);
                var updatedNhanVienDTO = _mapper.Map<NhanVienDTO>(existingNhanVien);
                await _accountBaseService.SendWelcomeEmailAsync(request.Email, updatedNhanVienDTO.HoTen, formattedPassword);

                // 9. Trả về kết quả
                return ServiceResult.Success("Di dời email tài khoản cho nhân viên thành công.", updatedNhanVienDTO);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi di dời email tài khoản: {ex.Message}");
            }
        }

        private async Task<ServiceResult> CreateTaiKhoanForExistingNhanVienAsync(AddTaiKhoanForNhanVienDTO request, string roleCode)
        {
            var password = PasswordHelper.FormatPassword(request.Password);

            var taiKhoan = new TaiKhoan
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = "", // Sẽ cập nhật từ nhân viên
                PhoneNumber = "", // Sẽ cập nhật từ nhân viên
                loaiTaiKhoan = roleCode, // Sử dụng roleCode từ tài khoản cũ
                daKichHoat = true,
                EmailConfirmed = true,
                daBiKhoa = false
            };

            var createResult = await _unitOfWork.IdentityRepository.CreateUserAsync(taiKhoan, password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description).ToList();
                return ServiceResult.Failure("Tạo tài khoản thất bại.", errors);
            }

            // Gán role giống tài khoản cũ
            await _unitOfWork.RoleRepository.AddToRoleAsync(taiKhoan, roleCode);

            return ServiceResult.Success("Tạo tài khoản thành công.", taiKhoan);
        }

        private async Task<ServiceResult> LinkTaiKhoanToNhanVienAsync(NhanVien nhanVien, TaiKhoan taiKhoan, AddTaiKhoanForNhanVienDTO request)
        {
            // Cập nhật thông tin tài khoản từ nhân viên
            taiKhoan.FullName = nhanVien.HoTen;
            taiKhoan.PhoneNumber = nhanVien.SoDienThoai;

            var updateAccountResult = await _unitOfWork.IdentityRepository.UpdateUserAsync(taiKhoan);
            if (!updateAccountResult.Succeeded)
            {
                return ServiceResult.Failure("Cập nhật thông tin tài khoản thất bại.");
            }

            // Cập nhật nhân viên với tài khoản mới
            nhanVien.MaTaiKhoan = taiKhoan.Id;
            nhanVien.Email = request.Email; // Chỉ cập nhật email
            // LoaiNhanVien và các field khác giữ nguyên

            var updateEmployeeResult = await _unitOfWork.NhanVienRepository.UpdateNhanVienAsync(nhanVien);
            if (!updateEmployeeResult)
            {
                return ServiceResult.Failure("Cập nhật thông tin nhân viên thất bại.");
            }

            return ServiceResult.Success("Liên kết tài khoản thành công.");
        }

        public async Task<ServiceResult> GetProfileByUserIdAsync(Guid userId)
        {
            var taiKhoan = await _unitOfWork.IdentityRepository.FindByUserIDAsync(userId.ToString());
            if (taiKhoan == null) return ServiceResult.Failure("Không tìm thấy tài khoản.");
            var nhanVien = await _unitOfWork.NhanVienRepository.GetNhanVienByTaiKhoanIdAsync(taiKhoan.Id);
            if (nhanVien == null) return ServiceResult.Failure("Không tìm thấy nhân viên.");
            var dto = _mapper.Map<NhanVienTaiKhoanDTO>(nhanVien);
            return ServiceResult.Success("Lấy profile thành công.", dto);
        }
    }
}
