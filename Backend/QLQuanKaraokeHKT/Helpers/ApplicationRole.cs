namespace QLQuanKaraokeHKT.Helpers
{
    public class ApplicationRole
    {
        public const string QuanLyKho = "QuanLyKho";
        public const string QuanLyNhanSu = "QuanLyNhanSu";
        public const string QuanLyPhongHat = "QuanLyPhongHat";
        public const string NhanVienKho = "NhanVienKho";
        public const string NhanVienPhucVu = "NhanVienPhucVu";
        public const string NhanVienTiepTan = "NhanVienTiepTan";
        public const string KhacHang = "KhachHang";
        public const string QuanTriHeThong = "QuanTriHeThong";
    }
    public static class RoleHelper
    {
        private static readonly Dictionary<string, string> RoleDescriptions = new()
        {
            { ApplicationRole.QuanLyKho, "Quản lý kho" },
            { ApplicationRole.QuanLyNhanSu, "Quản lý nhân sự" },
            { ApplicationRole.QuanLyPhongHat, "Quản lý phòng hát" },
            { ApplicationRole.NhanVienKho, "Nhân viên kho" },
            { ApplicationRole.NhanVienPhucVu, "Nhân viên phục vụ" },
            { ApplicationRole.NhanVienTiepTan, "Nhân viên tiếp tân" },
            { ApplicationRole.QuanTriHeThong, "Quản trị hệ thống" }
        };

        public static string GetRoleDescription(string roleCode)
        {
            return RoleDescriptions.TryGetValue(roleCode, out var description) ? description : roleCode;
        }

        public static string GetRoleCode(string description)
        {
            var pair = RoleDescriptions.FirstOrDefault(x => x.Value.Equals(description, StringComparison.OrdinalIgnoreCase));
            return pair.Key ?? description;
        }

        public static List<string> GetAllRoleCodes()
        {
            return RoleDescriptions.Keys.ToList();
        }
    }
}
