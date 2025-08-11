import { combineReducers } from "@reduxjs/toolkit";
import nhanVienReducer from "./nhanVien/slice";
import tienLuongReducer from "./tienLuong/slice";
import type { RootState } from "../../store";
import caLamViecReducer from "./caLamViec/slice";

export const qlNhanSuReducer = combineReducers({
  nhanVien: nhanVienReducer,
  tienLuong: tienLuongReducer,
  caLamViec: caLamViecReducer,
});

// State type
export type QLNhanSuState = ReturnType<typeof qlNhanSuReducer>;

// Base domain selector
export const selectQLNhanSu = (state: RootState) => state.qlNhanSu;

// Re-export types
export * from "./types";

/* -------- CHỈ EXPORT ALIAS (không duplicate) -------- */
// Thunks với alias
export {
  fetchAllNhanVien as fetchAllNhanVienQLNhanSu,
  createNhanVien as createNhanVienQLNhanSu,
  updateNhanVien as updateNhanVienQLNhanSu,
} from "./nhanVien/thunks";

// Actions với alias (chỉ những cái cần đổi tên)
export {
  setSearchQuery as setSearchQueryQLNhanSu,
  setTrangThaiFilter as setTrangThaiFilterQLNhanSu,
  clearFilters as clearFiltersQLNhanSu,
  clearNhanVienError as clearNhanVienErrorQLNhanSu,
} from "./nhanVien/slice";

// Actions giữ nguyên tên (không duplicate)
export {
  setLoaiNhanVienFilter,
  setShowAddModal,
  setShowEditModal,
  setSelectedNhanVien,
  removeNhanVienFromList,
} from "./nhanVien/slice";

// Selectors với alias
export {
  selectNhanVienSlice as selectNhanVienStateQLNhanSu,
  selectNhanVienUI as selectUIStateQLNhanSu,
  selectFilteredNhanVien as selectFilteredNhanVienQLNhanSu,
  selectNhanVienStats as selectNhanVienStatsQLNhanSu,
  selectSelectedNhanVien as selectSelectedNhanVienQLNhanSu,
  selectShowAddModalNhanVien as selectShowAddModalQLNhanSu,
  selectShowEditModalNhanVien as selectShowEditModalQLNhanSu,
} from "./nhanVien/selectors";

// Tiền lương exports
// Tiền lương exports
export {
  fetchAllTienLuong,
  createTienLuong,
  deleteTienLuong,
} from "./tienLuong/thunks";

export {
  setCurrentTienLuong,
  clearTienLuongError,
  clearTienLuongCurrent,
  resetTienLuongState,
  // UI Actions với alias rõ ràng
  setSearchQuery as setSearchQueryTienLuong,
  setSelectedCa as setSelectedCaTienLuong,
  setDateRange as setDateRangeTienLuong,
  openAddModal as openAddModalTienLuong,
  closeAddModal as closeAddModalTienLuong,
  openEditModal as openEditModalTienLuong,
  closeEditModal as closeEditModalTienLuong,
  clearAllFilters as clearAllFiltersTienLuong,
} from "./tienLuong/slice";

export {
  selectTienLuongSlice,
  selectTienLuongData,
  selectTienLuongLoading,
  selectTienLuongError,
  selectCurrentTienLuong,
  selectTienLuongUI,
  selectFilteredTienLuong,
  selectDefaultLuongCardsData,
  selectTienLuongStats,
} from "./tienLuong/selectors";
//ca làm việc export
export * from "./caLamViec/thunks";
export * from "./caLamViec/slice";
export * from "./caLamViec/selectors";
