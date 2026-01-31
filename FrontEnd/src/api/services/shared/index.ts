// Account API functions
export {
  getAllTaiKhoanNhanVien,
  getAllTaiKhoanKhachHang,
  addTaiKhoanNhanVien,
} from "./accountAPI";

// Employee API functions
export { getEmployeesWithoutAccounts, getAllEmployees } from "./employeeAPI";

// Re-export types
export type {
  NhanVienTaiKhoanDTO,
  KhachHangTaiKhoanDTO,
  AddTaiKhoanForNhanVienDTO,
} from "../../types/admins/QLHeThongtypes";

export type { NhanVienDTO } from "../../types/admins/QLNhanSutypes";

export * from "./employeeAPI";
export * from "./accountAPI";
export * from "./adminAPI";
export type { ApiResponse } from "../../types/apiResponse";
export type { AddNhanVienDTO } from "../../types/admins/QLHeThongtypes";
export * from "./quanLyLuongAPI";
