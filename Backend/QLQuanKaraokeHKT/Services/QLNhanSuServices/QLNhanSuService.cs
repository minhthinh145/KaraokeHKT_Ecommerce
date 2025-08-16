using AutoMapper;
using QLQuanKaraokeHKT.DTOs;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.ExternalService.Interfaces;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.Interfaces;
using QLQuanKaraokeHKT.Repositories.TaiKhoanRepo;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices
{
    public class QLNhanSuService : IQLNhanSuService
    {
        private readonly INhanVienRepository _nhanVienRepository;
        private readonly ITaiKhoanRepository _taiKhoanRepository;
        private readonly IMapper _mapper;
        private readonly ISendEmailService _emailService;

        public QLNhanSuService(INhanVienRepository nhanVienRepository, ITaiKhoanRepository taiKhoanRepository, IMapper mapper, ISendEmailService emailService)
        {
            _nhanVienRepository = nhanVienRepository ?? throw new ArgumentNullException(nameof(nhanVienRepository));
            _taiKhoanRepository = taiKhoanRepository ?? throw new ArgumentNullException(nameof(taiKhoanRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<ServiceResult> GetAllNhanVienAsync()
        {
            try
            {
                var nhanViens = await _nhanVienRepository.GetAllNhanVienAsync();
                if (nhanViens == null || !nhanViens.Any())
                {
                    return ServiceResult.Failure("Không có nhân viên nào trong hệ thống.");
                }

                var nhanVienDTOs = _mapper.Map<IEnumerable<NhanVienDTO>>(nhanViens);
                return ServiceResult.Success("Lấy danh sách nhân viên thành công.", nhanVienDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi lấy danh sách nhân viên: {ex.Message}");
            }
        }

        public async Task<ServiceResult> AddNhanVienAndAccountAsync(AddNhanVienDTO addNhanVienDto, string password)
        {
            try
            {
                // Validate input
                var validationResult = ValidateAddNhanVienInput(addNhanVienDto, password);
                if (!validationResult.IsSuccess)
                    return validationResult;

                // Format password
                password = FormatPassword(password);

                // Check email exists
                var emailCheckResult = await CheckEmailExistsAsync(addNhanVienDto.Email);
                if (!emailCheckResult.IsSuccess)
                    return emailCheckResult;

                // Create account
                var accountResult = await CreateTaiKhoanAsync(addNhanVienDto, password);
                if (!accountResult.IsSuccess)
                    return accountResult;

                var taiKhoan = (TaiKhoan)accountResult.Data;

                // Create employee
                var employeeResult = await CreateNhanVienAsync(addNhanVienDto, taiKhoan);
                if (!employeeResult.IsSuccess)
                    return employeeResult;

                var nhanVienDTO = (NhanVienDTO)employeeResult.Data;

                // Send welcome email
                await SendWelcomeEmailAsync(addNhanVienDto.Email, nhanVienDTO.HoTen, password);

                return ServiceResult.Success("Tạo nhân viên và tài khoản thành công.", nhanVienDTO);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi tạo nhân viên: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateNhanVienAsyncAndAccountAsync(NhanVienDTO nhanVienDto)
        {
            try
            {
                // Validate input
                if (nhanVienDto == null)
                    return ServiceResult.Failure("Thông tin cập nhật không hợp lệ.");

                // Get existing data
                var existingDataResult = await GetExistingNhanVienAndTaiKhoanAsync(nhanVienDto.MaNv);
                if (!existingDataResult.IsSuccess)
                    return existingDataResult;

                var (existingNhanVien, taiKhoan) = ((NhanVien, TaiKhoan))existingDataResult.Data;

                // Check email conflict
                var emailConflictResult = await CheckEmailConflictAsync(nhanVienDto.Email, existingNhanVien.Email, taiKhoan.Id);
                if (!emailConflictResult.IsSuccess)
                    return emailConflictResult;

                // Get role code
                var roleCodeResult = GetRoleCodeFromDescription(nhanVienDto.LoaiNhanVien);
                if (!roleCodeResult.IsSuccess)
                    return roleCodeResult;

                var newRoleCode = (string)roleCodeResult.Data;

                // Update account
                var updateAccountResult = await UpdateTaiKhoanAsync(taiKhoan, nhanVienDto, newRoleCode);
                if (!updateAccountResult.IsSuccess)
                    return updateAccountResult;

                // Update employee
                var updateEmployeeResult = await UpdateNhanVienAsync(existingNhanVien, nhanVienDto);
                if (!updateEmployeeResult.IsSuccess)
                    return updateEmployeeResult;

                var updatedNhanVienDTO = _mapper.Map<NhanVienDTO>(existingNhanVien);
                return ServiceResult.Success("Cập nhật nhân viên và tài khoản thành công.", updatedNhanVienDTO);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi cập nhật nhân viên: {ex.Message}");
            }
        }
        public async Task<ServiceResult> UpdateNhanVienDaNghiViecAsync(Guid maNhanVien, bool daNghiViec)
        {
            try
            {
                var existingNhanVien = await _nhanVienRepository.GetNhanVienByIdAsync(maNhanVien);
                if (existingNhanVien == null)
                    return ServiceResult.Failure("Không tìm thấy nhân viên.");

                var updateResult = await _nhanVienRepository.UpdateNhanVienDaNghiViecAsync(maNhanVien, daNghiViec);
                if (!updateResult)
                    return ServiceResult.Failure("Cập nhật trạng thái nhân viên thất bại.");

                if (daNghiViec)
                {
                    await _taiKhoanRepository.LockAccountAsync(existingNhanVien.MaTaiKhoan);
                    return ServiceResult.Success("Nhân viên đã được đánh dấu là đã nghỉ việc và tài khoản đã bị khóa.");
                }
                else
                {
                    await _taiKhoanRepository.UnlockAccountAsync(existingNhanVien.MaTaiKhoan);
                    return ServiceResult.Success("Nhân viên đã được đánh dấu là đang làm việc và tài khoản đã được mở khóa.");
                }
            } catch(Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi cập nhật trạng thái nhân viên: {ex.Message}");
            }
        }

        #region Private Helper Methods

        private static ServiceResult ValidateAddNhanVienInput(AddNhanVienDTO addNhanVienDto, string password)
        {
            if (addNhanVienDto == null)
                return ServiceResult.Failure("Thông tin nhân viên không hợp lệ.");

            if (string.IsNullOrWhiteSpace(password))
                return ServiceResult.Failure("Mật khẩu không được để trống.");

            return ServiceResult.Success();
        }

        private static string FormatPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return password;

            int index = 0;
            while (index < password.Length && !char.IsLetter(password[index]))
            {
                index++;
            }

            if (index < password.Length)
            {
                return password.Substring(0, index)
                     + char.ToUpper(password[index])
                     + password[(index + 1)..];
            }

            return password;
        }

        private async Task<ServiceResult> CheckEmailExistsAsync(string email)
        {
            var existingUser = await _taiKhoanRepository.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return ServiceResult.Failure("Email đã được sử dụng bởi tài khoản khác.");
            }
            return ServiceResult.Success();
        }

        private async Task<ServiceResult> CreateTaiKhoanAsync(AddNhanVienDTO addNhanVienDto, string password)
        {
            var taiKhoan = _mapper.Map<TaiKhoan>(addNhanVienDto);
            taiKhoan.daKichHoat = true;
            taiKhoan.EmailConfirmed = true;
            taiKhoan.daBiKhoa = false;

            var createUserResult = await _taiKhoanRepository.CreateUserAsync(taiKhoan, password);
            if (!createUserResult.Succeeded)
            {
                var errors = createUserResult.Errors.Select(e => e.Description).ToList();
                return ServiceResult.Failure("Tạo tài khoản thất bại.", errors);
            }

            await _taiKhoanRepository.AddToRoleAsync(taiKhoan, addNhanVienDto.LoaiTaiKhoan);
            return ServiceResult.Success("Tạo tài khoản thành công.", taiKhoan);
        }

        private async Task<ServiceResult> CreateNhanVienAsync(AddNhanVienDTO addNhanVienDto, TaiKhoan taiKhoan)
        {
            var roleDescription = await _taiKhoanRepository.GetRoleDescriptionAsync(addNhanVienDto.LoaiTaiKhoan);

            var nhanVien = _mapper.Map<NhanVien>(addNhanVienDto);
            nhanVien.MaNv = Guid.NewGuid();
            nhanVien.MaTaiKhoan = taiKhoan.Id;
            nhanVien.LoaiNhanVien = roleDescription;

            var createNhanVienResult = await _nhanVienRepository.CreateNhanVienAsync(nhanVien);
            if (createNhanVienResult == null)
            {
                return ServiceResult.Failure("Tạo nhân viên thất bại.");
            }

            var nhanVienDTO = _mapper.Map<NhanVienDTO>(nhanVien);
            return ServiceResult.Success("Tạo nhân viên thành công.", nhanVienDTO);
        }

        private async Task SendWelcomeEmailAsync(string email, string hoTen, string password)
        {
            var emailContent = $"Chào nhân viên {hoTen},\n\n" +
                               "Tài khoản của bạn đã được tạo thành công. Vui lòng đăng nhập với thông tin sau:\n" +
                               $"Email: {email}\n" +
                               $"Mật khẩu: {password}";

            await _emailService.SendEmailByContentAsync(email, "Tạo tài khoản nhân viên", emailContent);
        }

        private async Task<ServiceResult> GetExistingNhanVienAndTaiKhoanAsync(Guid maNhanVien)
        {
            var existingNhanVien = await _nhanVienRepository.GetNhanVienByIdAsync(maNhanVien);
            if (existingNhanVien == null)
            {
                return ServiceResult.Failure("Không tìm thấy nhân viên.");
            }

            var taiKhoan = await _taiKhoanRepository.FindByUserIDAsync(existingNhanVien.MaTaiKhoan.ToString());
            if (taiKhoan == null)
            {
                return ServiceResult.Failure("Không tìm thấy tài khoản liên kết.");
            }

            return ServiceResult.Success("Lấy thông tin thành công.", (existingNhanVien, taiKhoan));
        }

        private async Task<ServiceResult> CheckEmailConflictAsync(string newEmail, string currentEmail, Guid currentTaiKhoanId)
        {
            if (newEmail != currentEmail)
            {
                var existingEmailUser = await _taiKhoanRepository.FindByEmailAsync(newEmail);
                if (existingEmailUser != null && existingEmailUser.Id != currentTaiKhoanId)
                {
                    return ServiceResult.Failure("Email đã được sử dụng bởi tài khoản khác.");
                }
            }
            return ServiceResult.Success();
        }

        private static ServiceResult GetRoleCodeFromDescription(string? loaiNhanVien)
        {
            var newRoleCode = RoleHelper.GetRoleCode(loaiNhanVien ?? "");
            if (string.IsNullOrEmpty(newRoleCode))
            {
                return ServiceResult.Failure("Loại nhân viên không hợp lệ.");
            }
            return ServiceResult.Success("Role code hợp lệ.", newRoleCode);
        }

        private async Task<ServiceResult> UpdateTaiKhoanAsync(TaiKhoan taiKhoan, NhanVienDTO nhanVienDto, string newRoleCode)
        {
            taiKhoan.FullName = nhanVienDto.HoTen;
            taiKhoan.Email = nhanVienDto.Email;
            taiKhoan.UserName = nhanVienDto.Email;
            taiKhoan.PhoneNumber = nhanVienDto.SoDienThoai;
            taiKhoan.loaiTaiKhoan = newRoleCode;

            var updateUserResult = await _taiKhoanRepository.UpdateUserAsync(taiKhoan);
            if (!updateUserResult.Succeeded)
            {
                var errors = updateUserResult.Errors.Select(e => e.Description).ToArray();
                return ServiceResult.Failure($"Cập nhật tài khoản thất bại: {string.Join(", ", errors)}");
            }

            var updateRoleResult = await _taiKhoanRepository.UpdateUserRoleAsync(taiKhoan, newRoleCode);
            if (!updateRoleResult)
            {
                return ServiceResult.Failure("Cập nhật vai trò thất bại.");
            }

            return ServiceResult.Success("Cập nhật tài khoản thành công.");
        }

        private async Task<ServiceResult> UpdateNhanVienAsync(NhanVien existingNhanVien, NhanVienDTO nhanVienDto)
        {
            existingNhanVien.HoTen = nhanVienDto.HoTen;
            existingNhanVien.Email = nhanVienDto.Email;
            existingNhanVien.NgaySinh = nhanVienDto.NgaySinh;
            existingNhanVien.SoDienThoai = nhanVienDto.SoDienThoai;
            existingNhanVien.LoaiNhanVien = nhanVienDto.LoaiNhanVien;

            var updateNhanVienResult = await _nhanVienRepository.UpdateNhanVienAsync(existingNhanVien);
            if (!updateNhanVienResult)
            {
                return ServiceResult.Failure("Cập nhật nhân viên thất bại.");
            }

            return ServiceResult.Success("Cập nhật nhân viên thành công.");
        }

    

        #endregion
    }
}