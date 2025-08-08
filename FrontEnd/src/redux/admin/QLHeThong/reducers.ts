import type { PayloadAction } from "@reduxjs/toolkit";
import type { QLHeThongState } from "./types";

// 🎯 Sync Reducers
export const qlHeThongReducers = {
  // UI Actions
  setActiveTab: (
    state: QLHeThongState,
    action: PayloadAction<"nhan-vien" | "khach-hang" | "quan-ly">
  ) => {
    state.ui.activeTab = action.payload;
  },

  setSearchQuery: (state: QLHeThongState, action: PayloadAction<string>) => {
    state.ui.searchQuery = action.payload;
  },

  setLoaiTaiKhoanFilter: (
    state: QLHeThongState,
    action: PayloadAction<string>
  ) => {
    state.ui.filters.loaiTaiKhoan = action.payload;
  },

  setTrangThaiFilter: (
    state: QLHeThongState,
    action: PayloadAction<string>
  ) => {
    state.ui.filters.trangThai = action.payload;
  },

  clearFilters: (state: QLHeThongState) => {
    state.ui.searchQuery = "";
    state.ui.filters = {
      loaiTaiKhoan: "",
      trangThai: "",
    };
  },

  // Error Management
  clearNhanVienError: (state: QLHeThongState) => {
    state.nhanVien.error = null;
  },

  clearKhachHangError: (state: QLHeThongState) => {
    state.khachHang.error = null;
  },

  clearLoaiTaiKhoanError: (state: QLHeThongState) => {
    state.loaiTaiKhoan.error = null;
  },
  clearAdminAccountError: (state: QLHeThongState) => {
    state.adminAccount.error = null;
  },
};
