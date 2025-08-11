// 🎯 Base Response Interface (reuse existing)

// 🎯 Nhân viên DTO cho QL Nhân Sự (khác với QLHeThong)
export interface NhanVienDTO {
  maNv: string; // Guid from C# -> string
  hoTen: string;
  email: string;
  ngaySinh: string; // DateOnly from C# -> string (optional)
  soDienThoai?: string;
  loaiNhanVien?: string;
}

// 🎯 QL Nhân Sự State Types
export interface QLNhanSuState {
  nhanVien: {
    data: NhanVienDTO[];
    loading: boolean;
    error: string | null;
    total: number;
  };

  ui: {
    searchQuery: string;
    filters: {
      loaiNhanVien?: string;
      trangThai?: string;
    };
    selectedNhanVien?: NhanVienDTO | null;
    showAddModal: boolean;
    showEditModal: boolean;
  };
}

// 🎯 Filter Options cho QL Nhân Sự
export interface QLNhanSuFilterOption {
  value: string;
  label: string;
}

export const QL_NHAN_SU_FILTER_OPTIONS: QLNhanSuFilterOption[] = [
  { value: "All", label: "Tất cả" },
  { value: "NhanVienKho", label: "Nhân viên kho" },
  { value: "NhanVienPhucVu", label: "Nhân viên phục vụ" },
  { value: "NhanVienTiepTan", label: "Nhân viên tiếp tân" },
  { value: "QuanLyKho", label: "Quản lý kho" },
  { value: "QuanLyNhanSu", label: "Quản lý nhân sự" },
  { value: "QuanLyPhongHat", label: "Quản lý phòng hát" },
];

// 🎯 Form validation interface
export interface AddNhanVienFormData {
  hoTen: string;
  soDienThoai: string;
  email: string;
  diaChi: string;
  ngaySinh: string;
  loaiTaiKhoan: string;
}

export interface UpdateNhanVienFormData extends NhanVienDTO {
  diaChi?: string; // Có thể cần thêm địa chỉ cho update
}

// ====== Lương Ca Làm Việc (QL Tiền Lương) ======
export interface AddLuongCaLamViecDTO {
  maCa: number;
  ngayApDung?: string | null; // ISO date (backend dùng DateOnly)
  ngayKetThuc?: string | null; // ISO date
  giaCa: number;
  isDefault?: boolean;
}

export interface LuongCaLamViecDTO {
  maLuongCaLamViec: number;
  maCa: number;
  giaCa: number;
  isDefault: boolean;
  tenCaLamViec: string;
  ngayApDung?: string | null; // thêm (đặc biệt)
  ngayKetThuc?: string | null; // thêm (đặc biệt)
}
export interface CaLamViecDTO {
  maCa: number;
  tenCa: string;
  gioBatDauCa: string; // hoặc TimeOnly nếu bạn đã custom type
  gioKetThucCa: string;
}

export interface AddCaLamViecDTO {
  tenCa: string;
  gioBatDauCa: string;
  gioKetThucCa: string;
}
