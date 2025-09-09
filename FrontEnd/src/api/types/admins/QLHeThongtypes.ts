export interface NhanVienTaiKhoanDTO {
  // Thông tin nhân viên
  maNv: string;
  hoTen: string;
  email: string;
  ngaySinh?: string; // DateOnly from C# -> string
  soDienThoai?: string;
  loaiNhanVien?: string;

  // Thông tin tài khoản
  maTaiKhoan: string;
  userName: string;
  fullName: string;
  daKichHoat: boolean;
  daBiKhoa: boolean;
  loaiTaiKhoan: string; // ApplicationRole values
  emailConfirmed: boolean;
}
export interface AdminAccountDTO {
  maTaiKhoan: string;
  email: string;
  daKichHoat: boolean;
  daBiKhoa: boolean;
  loaiTaiKhoan: string;
}

export interface AddAdminAccountDTO {
  userName: string;
  phoneNumber: string;
  loaiTaiKhoan: string;
}

export interface UpdateAccountDTO {
  maTaiKhoan: string;
  newUserName: string;
  newPassword: string;
  newLoaiTaiKhoan: string;
}

// 🎯 Khách hàng DTO (placeholder - chưa có từ backend)
export interface KhachHangTaiKhoanDTO {
  // Thông tin khách hàng - Match C# properties
  maKhachHang: string; // Guid MaKhachHang -> string
  tenKhachHang: string; // string TenKhachHang
  email: string; // string Email
  ngaySinh?: string; // DateOnly? NgaySinh -> string?

  // Thông tin tài khoản - Match C# properties
  maTaiKhoan: string; // Guid MaTaiKhoan -> string
  userName: string; // string UserName
  fullName: string; // string FullName
  daKichHoat: boolean; // bool DaKichHoat
  daBiKhoa: boolean; // bool DaBiKhoa
  loaiTaiKhoan: string; // string LoaiTaiKhoan
  emailConfirmed: boolean; // bool EmailConfirmed
}

// 🎯 Application Roles Constants
export const ApplicationRole = {
  QuanLyKho: "QuanLyKho",
  QuanLyNhanSu: "QuanLyNhanSu",
  QuanLyPhongHat: "QuanLyPhongHat",
  NhanVienKho: "NhanVienKho",
  NhanVienPhucVu: "NhanVienPhucVu",
  NhanVienTiepTan: "NhanVienTiepTan",
  QuanTriHeThong: "QuanTriHeThong",
  KhachHang: "KhachHang",
} as const;

// Tạo type từ ApplicationRole:
export type ApplicationRoleType = typeof ApplicationRole[keyof typeof ApplicationRole];

// 🎯 Add Tài khoản cho Nhân viên DTO
export interface AddTaiKhoanForNhanVienDTO {
  maNhanVien: string; // Guid -> string
  email: string;
}
// 🎯 Filter Options cho Frontend
export interface NhanVienFilterOption {
  value: string;
  label: string;
}

export const NHAN_VIEN_FILTER_OPTIONS: NhanVienFilterOption[] = [
  { value: "All", label: "Tất cả" },
  { value: ApplicationRole.NhanVienKho, label: "Nhân viên kho" },
  { value: ApplicationRole.NhanVienPhucVu, label: "Nhân viên phục vụ" },
  { value: ApplicationRole.NhanVienTiepTan, label: "Nhân viên tiếp tân" },
  { value: ApplicationRole.QuanLyKho, label: "Quản lý kho vật liệu" },
  { value: ApplicationRole.QuanTriHeThong, label: "Quản trị hệ thống" },
  { value: ApplicationRole.QuanLyPhongHat, label: "Quản lý phòng hát" },
  { value: ApplicationRole.QuanLyNhanSu, label: "Quản lý nhân sự" },
];

// 🎯 Add Nhân viên DTO (để tạo mới)
export interface AddNhanVienDTO {
  hoTen: string;
  email: string;
  ngaySinh?: string;
  soDienThoai?: string;
  loaiNhanVien?: string;
  loaiTaiKhoan: string;
}

// 🎯 API Error Response
export interface ApiErrorResponse {
  message: string;
  success: false;
  error: string;
}
