import { createSelector } from "@reduxjs/toolkit";
import type { RootState } from "../../store";

// 🎯 Base selectors - Renamed to avoid conflicts
export const selectQLNhanSu = (state: RootState) => state.qlNhanSu;

export const selectNhanVienStateQLNhanSu = createSelector(
  [selectQLNhanSu],
  (qlNhanSu) => qlNhanSu.nhanVien
);

export const selectUIStateQLNhanSu = createSelector(
  [selectQLNhanSu],
  (qlNhanSu) => qlNhanSu.ui
);

// 🔍 Filtered selectors - Renamed
export const selectFilteredNhanVienQLNhanSu = createSelector(
  [selectNhanVienStateQLNhanSu, selectUIStateQLNhanSu],
  (nhanVienState, uiState) => {
    let filteredData = [...nhanVienState.data];

    // Search filter
    if (uiState.searchQuery.trim()) {
      const query = uiState.searchQuery.toLowerCase();
      filteredData = filteredData.filter(
        (nv) =>
          nv.hoTen.toLowerCase().includes(query) ||
          nv.email.toLowerCase().includes(query) ||
          (nv.soDienThoai && nv.soDienThoai.includes(query))
      );
    }

    // Loại nhân viên filter
    if (
      uiState.filters.loaiNhanVien &&
      uiState.filters.loaiNhanVien !== "All"
    ) {
      filteredData = filteredData.filter(
        (nv) => nv.loaiNhanVien === uiState.filters.loaiNhanVien
      );
    }

    return filteredData;
  }
);

// 📊 Stats selectors - Renamed
export const selectNhanVienStatsQLNhanSu = createSelector(
  [selectNhanVienStateQLNhanSu],
  (nhanVienState) => ({
    total: nhanVienState.total,
    byType: nhanVienState.data.reduce((acc, nv) => {
      const type = nv.loaiNhanVien || "Chưa phân loại";
      acc[type] = (acc[type] || 0) + 1;
      return acc;
    }, {} as Record<string, number>),
  })
);

// 🎯 Modal & Selected selectors
export const selectSelectedNhanVienQLNhanSu = createSelector(
  [selectUIStateQLNhanSu],
  (uiState) => uiState.selectedNhanVien
);

export const selectShowAddModalQLNhanSu = createSelector(
  [selectUIStateQLNhanSu],
  (uiState) => uiState.showAddModal
);

export const selectShowEditModalQLNhanSu = createSelector(
  [selectUIStateQLNhanSu],
  (uiState) => uiState.showEditModal
);
