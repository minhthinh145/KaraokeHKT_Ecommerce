using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLHeThongDTOs;

namespace QLQuanKaraokeHKT.Application.Helpers
{
    public static class ValidationHelper
    {
        public static ServiceResult ValidateAddTaiKhoanForNhanVien(AddTaiKhoanForNhanVienDTO request)
        {
            if (request == null)
                return ServiceResult.Failure("Thông tin yêu cầu không hợp lệ.");

            if (request.MaNhanVien == Guid.Empty)
                return ServiceResult.Failure("Mã nhân viên không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.Email))
                return ServiceResult.Failure("Email không được để trống.");

            return ServiceResult.Success();
        }

        public static ServiceResult ValidateAddAdminAccount(AddAccountForAdminDTO request)
        {
            if (request == null)
                return ServiceResult.Failure("Thông tin tài khoản quản lý không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.UserName))
                return ServiceResult.Failure("Tên người dùng không được để trống.");

            if (string.IsNullOrWhiteSpace(request.loaiTaiKhoan))
                return ServiceResult.Failure("Loại tài khoản không được để trống.");

            return ServiceResult.Success();
        }
    }
}