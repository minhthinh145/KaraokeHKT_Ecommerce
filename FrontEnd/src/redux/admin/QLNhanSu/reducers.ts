import type { PayloadAction } from "@reduxjs/toolkit";
import type { QLNhanSuState } from "./types";
import type { NhanVienDTO } from "../../../api";

// 🎯 Sync Reducers với tên không trùng
export const qlNhanSuReducers = {
  // 🔍 Search & Filter Actions - Renamed
  setSearchQueryQLNhanSu: (
    state: QLNhanSuState,
    action: PayloadAction<string>
  ) => {
    state.ui.searchQuery = action.payload;
  },

  setLoaiNhanVienFilter: (
    state: QLNhanSuState,
    action: PayloadAction<string>
  ) => {
    state.ui.filters.loaiNhanVien = action.payload;
  },

  setTrangThaiFilterQLNhanSu: (
    state: QLNhanSuState,
    action: PayloadAction<string | undefined>
  ) => {
    state.ui.filters.trangThai = action.payload;
  },

  clearFiltersQLNhanSu: (state: QLNhanSuState) => {
    state.ui.searchQuery = "";
    state.ui.filters = {
      loaiNhanVien: "All",
      trangThai: undefined,
    };
  },

  // 📝 Modal Management
  setShowAddModal: (state: QLNhanSuState, action: PayloadAction<boolean>) => {
    state.ui.showAddModal = action.payload;
    if (!action.payload) {
      state.ui.selectedNhanVien = null;
    }
  },

  setShowEditModal: (state: QLNhanSuState, action: PayloadAction<boolean>) => {
    state.ui.showEditModal = action.payload;
    if (!action.payload) {
      state.ui.selectedNhanVien = null;
    }
  },

  setSelectedNhanVien: (
    state: QLNhanSuState,
    action: PayloadAction<NhanVienDTO | null>
  ) => {
    state.ui.selectedNhanVien = action.payload;
  },

  // 🧹 Error Management - Renamed
  clearNhanVienErrorQLNhanSu: (state: QLNhanSuState) => {
    state.nhanVien.error = null;
  },

  // 🔄 Data Management
  removeNhanVienFromList: (
    state: QLNhanSuState,
    action: PayloadAction<string>
  ) => {
    state.nhanVien.data = state.nhanVien.data.filter(
      (nv) => nv.maNv !== action.payload
    );
    state.nhanVien.total = state.nhanVien.data.length;
  },
};
