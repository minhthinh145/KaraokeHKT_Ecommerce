using AutoMapper;
using QLQuanKaraokeHKT.DTOs;
using QLQuanKaraokeHKT.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.ExternalService.Interfaces;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.Interfaces;
using QLQuanKaraokeHKT.Repositories.TaiKhoanRepo;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices
{
    public class QLHeThongService : IQLHeThongService
    {
        private readonly INhanVienRepository _nhanVienRepository;
        private readonly IKhacHangRepository _khachHangRepository;
        private readonly ITaiKhoanRepository _taiKhoanRepository;
        private readonly IMapper _mapper;
        private readonly ISendEmailService _emailService;
        private readonly ITaiKhoanQuanLyRepository _taiKhoanQuanLyRepository;

        public QLHeThongService(
            INhanVienRepository nhanVienRepository,
            IKhacHangRepository khachHangRepository,
            ITaiKhoanRepository taiKhoanRepository,
            IMapper mapper,
            ISendEmailService emailService,
            ITaiKhoanQuanLyRepository taiKhoanQuanLyRepository 
            )
        {
            _nhanVienRepository = nhanVienRepository ?? throw new ArgumentNullException(nameof(nhanVienRepository));
            _khachHangRepository = khachHangRepository ?? throw new ArgumentNullException(nameof(khachHangRepository));
            _taiKhoanRepository = taiKhoanRepository ?? throw new ArgumentNullException(nameof(taiKhoanRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _taiKhoanQuanLyRepository = taiKhoanQuanLyRepository ?? throw new ArgumentNullException(nameof(taiKhoanQuanLyRepository));
        }

        public async Task<ServiceResult> GetAllTaiKhoanKhachHangAsync()
        {
            try
            {
                var khachHangs = await _khachHangRepository.GetAllWithTaiKhoanAsync(); 
                if (khachHangs == null || !khachHangs.Any())
                {
                    return ServiceResult.Failure("Không có khách hàng nào trong hệ thống.");
                }

                var khachHangTaiKhoanDTOs = _mapper.Map<IEnumerable<KhachHangTaiKhoanDTO>>(khachHangs);
                return ServiceResult.Success("Lấy danh sách tài khoản khách hàng thành công.", khachHangTaiKhoanDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi lấy danh sách tài khoản khách hàng: {ex.Message}");
            }
        }

        public async Task<ServiceResult> GetAllTaiKhoanNhanVienAsync()
        {
            try
            {
                var nhanViens = await _nhanVienRepository.GetAllNhanVienWithTaiKhoanAsync(); // Cần implement method này
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

                var nhanViens = await _nhanVienRepository.GetAllByLoaiTaiKhoanWithTaiKhoanAsync(loaiTaiKhoan); // Cần implement method này
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
                var validationResult = ValidateAddTaiKhoanInput(request);
                if (!validationResult.IsSuccess)
                    return validationResult;

                // 2. Lấy thông tin nhân viên
                var existingNhanVien = await _nhanVienRepository.GetNhanVienByIdAsync(request.MaNhanVien);
                if (existingNhanVien == null)
                {
                    return ServiceResult.Failure("Không tìm thấy nhân viên.");
                }

                // 3. Lấy tài khoản cũ và role của nó
                TaiKhoan? oldAccount = null;
                string roleCode = ApplicationRole.NhanVienPhucVu; // Default role

                if (existingNhanVien.MaTaiKhoan != Guid.Empty)
                {
                    oldAccount = await _taiKhoanRepository.FindByUserIDAsync(existingNhanVien.MaTaiKhoan.ToString());
                    if (oldAccount != null)
                    {
                        // Lấy role từ tài khoản cũ
                        roleCode = oldAccount.loaiTaiKhoan ?? ApplicationRole.NhanVienPhucVu;
                    }
                }

                // 4. Kiểm tra email mới không trùng
                var emailCheckResult = await CheckEmailExistsAsync(request.Email);
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
                    await _taiKhoanRepository.DeleteUserAsync(newAccount);
                    return linkResult;
                }

                // 7. Xóa tài khoản cũ (sau khi link thành công)
                if (oldAccount != null)
                {
                    var deleteOldResult = await _taiKhoanRepository.DeleteUserWithRelatedDataAsync(oldAccount);
                    if (!deleteOldResult)
                    {
                        // Log warning nhưng không fail vì tài khoản mới đã tạo thành công
                    }
                }

                // 8. Gửi email thông báo
                var formattedPassword = FormatPassword(request.Password);
                var updatedNhanVienDTO = _mapper.Map<NhanVienDTO>(existingNhanVien);
                await SendWelcomeEmailAsync(request.Email, updatedNhanVienDTO.HoTen, formattedPassword);

                // 9. Trả về kết quả
                return ServiceResult.Success("Di dời email tài khoản cho nhân viên thành công.", updatedNhanVienDTO);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi di dời email tài khoản: {ex.Message}");
            }
        }
        public async Task<ServiceResult> LockAccountByMaTaiKhoanAsync(Guid maTaiKhoan)
        {
            var result = await _taiKhoanRepository.LockAccountAsync(maTaiKhoan);
            if (result)
            {
                return ServiceResult.Success("Tài khoản đã bị khóa thành công.");
            }
            return ServiceResult.Failure("Không thể khóa tài khoản. Vui lòng kiểm tra lại mã tài khoản hoặc trạng thái tài khoản.");

        }

        public async Task<ServiceResult> UnlockAccountByMaTaiKhoanAsync(Guid maTaiKhoan)
        {
            var result = await _taiKhoanRepository.UnlockAccountAsync(maTaiKhoan);
            if (result)
            {
                return ServiceResult.Success("Tài khoản đã được mở khóa thành công.");
            }
            return ServiceResult.Failure("Không thể mở khóa tài khoản. Vui lòng kiểm tra lại mã tài khoản hoặc trạng thái tài khoản.");
        }

        public async Task<ServiceResult> GettAllAdminAccount()
        {
            var taiKhoanQuanLyList = await _taiKhoanQuanLyRepository.GettAllAdminAccount();
            if (taiKhoanQuanLyList == null || !taiKhoanQuanLyList.Any())
            {
                return ServiceResult.Failure("Không có tài khoản quản lý nào trong hệ thống.");
            }
            var taiKhoanQuanLyDTOs = _mapper.Map<IEnumerable<TaiKhoanQuanLyDTO>>(taiKhoanQuanLyList);
            return ServiceResult.Success("Lấy danh sách tài khoản quản lý thành công.", taiKhoanQuanLyDTOs);

        }
        #region Helper Methods

        private async Task SendWelcomeEmailAsync(string email, string hoTen, string password)
        {
            var emailContent = $"Chào nhân viên {hoTen},\n\n" +
                               "Tài khoản của bạn đã được tạo thành công. Vui lòng đăng nhập với thông tin sau:\n" +
                               $"Email: {email}\n" +
                               $"Mật khẩu: {password}";

            await _emailService.SendEmailByContentAsync(email, "Tạo tài khoản nhân viên", emailContent);
        }

        private static ServiceResult ValidateAddTaiKhoanInput(AddTaiKhoanForNhanVienDTO request)
        {
            if (request == null)
                return ServiceResult.Failure("Thông tin yêu cầu không hợp lệ.");

            if (request.MaNhanVien == Guid.Empty)
                return ServiceResult.Failure("Mã nhân viên không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.Email))
                return ServiceResult.Failure("Email không được để trống.");

          

            return ServiceResult.Success();
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

        private async Task<ServiceResult> CreateTaiKhoanForExistingNhanVienAsync(AddTaiKhoanForNhanVienDTO request, string roleCode)
        {
            var password = FormatPassword(request.Password);

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

            var createResult = await _taiKhoanRepository.CreateUserAsync(taiKhoan, password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description).ToList();
                return ServiceResult.Failure("Tạo tài khoản thất bại.", errors);
            }

            // Gán role giống tài khoản cũ
            await _taiKhoanRepository.AddToRoleAsync(taiKhoan, roleCode);

            return ServiceResult.Success("Tạo tài khoản thành công.", taiKhoan);
        }

        private async Task<ServiceResult> LinkTaiKhoanToNhanVienAsync(NhanVien nhanVien, TaiKhoan taiKhoan, AddTaiKhoanForNhanVienDTO request)
        {
            // Cập nhật thông tin tài khoản từ nhân viên
            taiKhoan.FullName = nhanVien.HoTen;
            taiKhoan.PhoneNumber = nhanVien.SoDienThoai;

            var updateAccountResult = await _taiKhoanRepository.UpdateUserAsync(taiKhoan);
            if (!updateAccountResult.Succeeded)
            {
                return ServiceResult.Failure("Cập nhật thông tin tài khoản thất bại.");
            }

            // Cập nhật nhân viên với tài khoản mới
            nhanVien.MaTaiKhoan = taiKhoan.Id;
            nhanVien.Email = request.Email; // Chỉ cập nhật email
            // LoaiNhanVien và các field khác giữ nguyên

            var updateEmployeeResult = await _nhanVienRepository.UpdateNhanVienAsync(nhanVien);
            if (!updateEmployeeResult)
            {
                return ServiceResult.Failure("Cập nhật thông tin nhân viên thất bại.");
            }

            return ServiceResult.Success("Liên kết tài khoản thành công.");
        }

        public async Task<ServiceResult> GetAllNhanVienAsync()
        {
            var listNhanVien = await _nhanVienRepository.GetAllNhanVienAsync();
            if (listNhanVien == null || !listNhanVien.Any())
            {
                return ServiceResult.Failure("Không có nhân viên nào trong hệ thống.");
            }
            var nhanVienDTOs = _mapper.Map<IEnumerable<NhanVienDTO>>(listNhanVien);
            return ServiceResult.Success("Lấy danh sách nhân viên thành công.", nhanVienDTOs);
        }

  
        #endregion
    }
}