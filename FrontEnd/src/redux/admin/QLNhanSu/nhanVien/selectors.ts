import { createSelector } from "@reduxjs/toolkit";
import type { RootState } from "../../../store";
import type { NhanVienDTO } from "../../../../api";
// Base slice
export const selectNhanVienSlice = (state: RootState) =>
  state.qlNhanSu.nhanVien;
export const selectNhanVienUI = (state: RootState) =>
  state.qlNhanSu.nhanVien.ui;
export const selectNhanVienList = (state: RootState) =>
  state.qlNhanSu.nhanVien.data;

export const selectFilteredNhanVien = createSelector(
  [selectNhanVienList, selectNhanVienUI],
  (list, ui) => {
    let data = [...list];
    if (ui.searchQuery.trim()) {
      const q = ui.searchQuery.toLowerCase();
      data = data.filter(
        (nv) =>
          nv.hoTen.toLowerCase().includes(q) ||
          nv.email.toLowerCase().includes(q) ||
          (nv.soDienThoai && nv.soDienThoai.includes(q))
      );
    }
    if (ui.filters.loaiNhanVien && ui.filters.loaiNhanVien !== "All") {
      data = data.filter((nv) => nv.loaiNhanVien === ui.filters.loaiNhanVien);
    }

    return data;
  }
);

export const selectNhanVienStats = createSelector(
  [selectNhanVienSlice],
  (s) => ({
    total: s.total,
    byType: s.data.reduce<Record<string, number>>((acc, nv: NhanVienDTO) => {
      const key = nv.loaiNhanVien || "Chưa phân loại";
      acc[key] = (acc[key] || 0) + 1;
      return acc;
    }, {}),
  })
);

export const selectNhanVienLoading = (state: RootState) =>
  state.qlNhanSu.nhanVien.loading;
export const selectNhanVienError = (state: RootState) =>
  state.qlNhanSu.nhanVien.error;

export const selectSelectedNhanVien = (state: RootState) =>
  state.qlNhanSu.nhanVien.ui.selectedNhanVien;
export const selectShowAddModalNhanVien = (state: RootState) =>
  state.qlNhanSu.nhanVien.ui.showAddModal;
export const selectShowEditModalNhanVien = (state: RootState) =>
  state.qlNhanSu.nhanVien.ui.showEditModal;
export const selectSearchQueryNhanVien = (state: RootState) =>
  state.qlNhanSu.nhanVien.ui.searchQuery;
