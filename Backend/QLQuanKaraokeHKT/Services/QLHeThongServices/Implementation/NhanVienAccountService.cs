using AutoMapper;
using QLQuanKaraokeHKT.DTOs;
using QLQuanKaraokeHKT.DTOs.QLHeThongDTOs;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.Interfaces;
using QLQuanKaraokeHKT.Repositories.TaiKhoanRepo;
using QLQuanKaraokeHKT.Services.QLHeThongServices.Interface;

namespace QLQuanKaraokeHKT.Services.QLHeThongServices.Implementation
{
    public class NhanVienAccountService : INhanVienAccountService
    {
        private readonly IAccountBaseService _accountBaseService;
        private readonly ITaiKhoanRepository _taiKhoanRepository;
        private readonly INhanVienRepository _nhanVienRepository;
        private readonly IMapper _mapper;

        public NhanVienAccountService(IAccountBaseService accountBaseService, 
            ITaiKhoanRepository taiKhoanRepository, 
            INhanVienRepository nhanVienRepository, 
            IMapper mapper)
        {
            _accountBaseService = accountBaseService;
            _taiKhoanRepository = taiKhoanRepository;
            _nhanVienRepository = nhanVienRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllTaiKhoanNhanVienAsync()
        {
            try
            {
                var nhanViens = await _nhanVienRepository.GetAllNhanVienWithTaiKhoanAsync(); 
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
                var validationResult = ValidationHelper.ValidateAddTaiKhoanForNhanVien(request);
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

        public async Task<ServiceResult> GetProfileByUserIdAsync(Guid userId)
        {
            var taiKhoan = await _taiKhoanRepository.FindByUserIDAsync(userId.ToString());
            if (taiKhoan == null) return ServiceResult.Failure("Không tìm thấy tài khoản.");
            var nhanVien = await _nhanVienRepository.GetNhanVienByTaiKhoanIdAsync(taiKhoan.Id);
            if (nhanVien == null) return ServiceResult.Failure("Không tìm thấy nhân viên.");
            var dto = _mapper.Map<NhanVienTaiKhoanDTO>(nhanVien);
            return ServiceResult.Success("Lấy profile thành công.", dto);
        }
    }
}
