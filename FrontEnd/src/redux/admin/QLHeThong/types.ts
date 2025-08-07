import type {
  NhanVienTaiKhoanDTO,
  KhachHangTaiKhoanDTO,
  AdminAccountDTO,
} from "../../../api/types/admins/QLHeThongtypes";

// 🎯 Redux State Interface
export interface QLHeThongState {
  nhanVien: {
    data: NhanVienTaiKhoanDTO[];
    loading: boolean;
    error: string | null;
    total: number;
  };

  khachHang: {
    data: KhachHangTaiKhoanDTO[];
    loading: boolean;
    error: string | null;
    total: number;
  };

  adminAccount: {
    data: AdminAccountDTO[];
    loading: boolean;
    error: string | null;
    total: number;
  };

  loaiTaiKhoan: {
    data: string[];
    loading: boolean;
    error: string | null;
  };

  ui: {
    activeTab: "nhan-vien" | "khach-hang";
    searchQuery: string;
    filters: {
      loaiTaiKhoan: string;
      trangThai?: string;
    };
  };
}

// 🎯 Initial State
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
  loaiTaiKhoan: {
    data: [],
    loading: false,
    error: null,
  },
  adminAccount: {
    data: [],
    loading: false,
    error: null,
    total: 0,
  },
  ui: {
    activeTab: "nhan-vien",
    searchQuery: "",
    filters: {
      loaiTaiKhoan: "All",
    },
  },
};
