import type { NhanVienDTO, AddNhanVienDTO } from "../../../api/index";

// 🎯 Redux State Interface cho QL Nhân Sú
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
      loaiNhanVien: string; // "All" | specific types
      trangThai?: string;
    };
    selectedNhanVien?: NhanVienDTO | null;
    showAddModal: boolean;
    showEditModal: boolean;
  };
}

// 🎯 Initial State
export const qlNhanSuInitialState: QLNhanSuState = {
  nhanVien: {
    data: [],
    loading: false,
    error: null,
    total: 0,
  },
  ui: {
    searchQuery: "",
    filters: {
      loaiNhanVien: "All",
    },
    selectedNhanVien: null,
    showAddModal: false,
    showEditModal: false,
  },
};
