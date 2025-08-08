import type {
  NhanVienTaiKhoanDTO,
  KhachHangTaiKhoanDTO,
  AdminAccountDTO,
} from "../../../api/types/admins/QLHeThongtypes";

// 🎯 Generic Data State Interface
export interface DataState<T> {
  data: T[];
  loading: boolean;
  error: string | null;
  total: number;
}

// 🎯 UI State Interface
export interface UIState {
  activeTab: "nhan-vien" | "khach-hang" | "quan-ly";
  searchQuery: string;
  filters: {
    loaiTaiKhoan: string;
    trangThai: string; // không optional
  };
}

// 🎯 Redux State Interface - sử dụng generic DataState
export interface QLHeThongState {
  nhanVien: DataState<NhanVienTaiKhoanDTO>;
  khachHang: DataState<KhachHangTaiKhoanDTO>;
  adminAccount: DataState<AdminAccountDTO>;
  loaiTaiKhoan: {
    data: string[];
    loading: boolean;
    error: string | null;
  };
  ui: UIState;
}

// 🎯 Initial State - đồng nhất tất cả
export const qlHeThongInitialState: QLHeThongState = {
  nhanVien: {
    data: [],
    loading: false,
    error: null,
    total: 0,
  },
  khachHang: {
    data: [],
    loading: false,
    error: null,
    total: 0,
  },
  adminAccount: {
    data: [],
    loading: false,
    error: null,
    total: 0,
  },
  loaiTaiKhoan: {
    data: [],
    loading: false,
    error: null,
  },
  ui: {
    activeTab: "nhan-vien",
    searchQuery: "",
    filters: {
      loaiTaiKhoan: "", // đồng nhất: dùng "" thay vì "All"
      trangThai: "", // đồng nhất: luôn có giá trị
    },
  },
};
