const ROLE_KEY_MAP: Record<string, string> = {
  "Nhân viên kho": "NhanVienKho",
  "Nhân viên phục vụ": "NhanVienPhucVu",
  "Nhân viên tiếp tân": "NhanVienTiepTan",
  "Quản trị hệ thống": "QuanTriHeThong",
  "Quản lý kho": "QuanLyKho",
  "Quản lý nhân sự": "QuanLyNhanSu",
  "Quản lý phòng hát": "QuanLyPhongHat",
};

export const ROLE_NAMES: Record<string, string> = {
  // Quản lý
  QuanTriHeThong: "Quản trị hệ thống",
  QuanLyKho: "Quản lý kho",
  QuanLyNhanSu: "Quản lý nhân sự",
  QuanLyPhongHat: "Quản lý phòng hát",
  // Nhân viên
  NhanVienKho: "Nhân viên kho",
  NhanVienPhucVu: "Nhân viên phục vụ",
  NhanVienTiepTan: "Nhân viên tiếp tân",
};

export const ROLE_COLORS: Record<string, string> = {
  // Quản lý
  QuanTriHeThong: "bg-purple-100 text-purple-800",
  QuanLyKho: "bg-yellow-100 text-yellow-800",
  QuanLyNhanSu: "bg-pink-100 text-pink-800",
  QuanLyPhongHat: "bg-blue-100 text-blue-800",
  // Nhân viên
  NhanVienKho: "bg-blue-100 text-blue-800",
  NhanVienPhucVu: "bg-green-100 text-green-800",
  NhanVienTiepTan: "bg-purple-100 text-purple-800",
};

// Macro phân loại
export const isQuanLyRole = (role: string) =>
  ["QuanTriHeThong", "QuanLyKho", "QuanLyNhanSu", "QuanLyPhongHat"].includes(
    role
  );

export const isNhanVienRole = (role: string) =>
  ["NhanVienKho", "NhanVienPhucVu", "NhanVienTiepTan"].includes(role);

export const renderRoleBadge = (role: string) => {
  const key = ROLE_KEY_MAP[role] || role;

  return (
    <span
      className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${
        ROLE_COLORS[key] || "bg-gray-100 text-gray-800"
      }`}
    >
      {ROLE_NAMES[key] || role}
    </span>
  );
};
