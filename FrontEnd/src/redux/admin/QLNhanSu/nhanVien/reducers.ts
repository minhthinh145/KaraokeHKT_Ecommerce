import type { PayloadAction } from "@reduxjs/toolkit";
import type { NhanVienSliceState } from "../types";
import type { NhanVienDTO } from "../../../../api";

export const nhanVienReducers = {
  setSearchQuery(state: NhanVienSliceState, action: PayloadAction<string>) {
    state.ui.searchQuery = action.payload;
  },
  setLoaiNhanVienFilter(
    state: NhanVienSliceState,
    action: PayloadAction<string>
  ) {
    state.ui.filters.loaiNhanVien = action.payload;
  },
  setTrangThaiFilter(
    state: NhanVienSliceState,
    action: PayloadAction<string | undefined>
  ) {
    state.ui.filters.trangThai = action.payload;
  },
  clearFilters(state: NhanVienSliceState) {
    state.ui.searchQuery = "";
    state.ui.filters = { loaiNhanVien: "All", trangThai: undefined };
  },
  setShowAddModal(state: NhanVienSliceState, action: PayloadAction<boolean>) {
    state.ui.showAddModal = action.payload;
    if (!action.payload) state.ui.selectedNhanVien = null;
  },
  setShowEditModal(state: NhanVienSliceState, action: PayloadAction<boolean>) {
    state.ui.showEditModal = action.payload;
    if (!action.payload) state.ui.selectedNhanVien = null;
  },
  setSelectedNhanVien(
    state: NhanVienSliceState,
    action: PayloadAction<NhanVienDTO | null>
  ) {
    state.ui.selectedNhanVien = action.payload;
  },
  clearNhanVienError(state: NhanVienSliceState) {
    state.error = null;
  },
  setNhanVienList(
    state: NhanVienSliceState,
    action: PayloadAction<NhanVienDTO[]>
  ) {
    state.data = action.payload;
    state.total = action.payload.length;
  },
  removeNhanVienFromList(
    state: NhanVienSliceState,
    action: PayloadAction<string>
  ) {
    state.data = state.data.filter((nv) => nv.maNv !== action.payload);
    state.total = state.data.length;
  },
};
