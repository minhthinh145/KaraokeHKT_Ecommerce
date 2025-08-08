// 🎯 Explicit exports to avoid naming conflicts

// QLHeThong exports
export {
  qlHeThongReducer,
  // Thunks
  fetchAllNhanVien as fetchAllNhanVienQLHeThong,
  fetchAllKhachHang,
  fetchLoaiTaiKhoan,
  fetchAllAdminAccount,
  createNhanVienAccount as createNhanVienQLHeThong,
  createAdminAccount,
  // Actions
  setActiveTab,
  setSearchQuery as setSearchQueryQLHeThong,
  setLoaiTaiKhoanFilter,
  setTrangThaiFilter as setTrangThaiFilterQLHeThong,
  clearFilters as clearFiltersQLHeThong,
  clearNhanVienError as clearNhanVienErrorQLHeThong,
  clearKhachHangError,
  clearLoaiTaiKhoanError,
  clearAdminAccountError,
  // Selectors
  selectQLHeThong,
  selectNhanVienState as selectNhanVienStateQLHeThong,
  selectKhachHangState,
  selectLoaiTaiKhoanState,
  selectUIState as selectUIStateQLHeThong,
  selectFilteredNhanVien as selectFilteredNhanVienQLHeThong,
  selectFilteredKhachHang,
  selectNhanVienStats as selectNhanVienStatsQLHeThong,
  selectKhachHangStats,
  selectAdminAccountState,
  selectFilteredAdminAccount,
  selectAdminAccountStats,
} from "./QLHeThong";

// QLNhanSu exports
export {
  qlNhanSuReducer,
  // Thunks
  fetchAllNhanVienQLNhanSu,
  createNhanVienQLNhanSu,
  updateNhanVienQLNhanSu,
  // Actions
  setSearchQueryQLNhanSu,
  setLoaiNhanVienFilter,
  setTrangThaiFilterQLNhanSu,
  clearFiltersQLNhanSu,
  setShowAddModal,
  setShowEditModal,
  setSelectedNhanVien,
  clearNhanVienErrorQLNhanSu,
  removeNhanVienFromList,

  // Selectors
  selectQLNhanSu,
  selectNhanVienStateQLNhanSu,
  selectUIStateQLNhanSu,
  selectFilteredNhanVienQLNhanSu,
  selectNhanVienStatsQLNhanSu,
  selectSelectedNhanVienQLNhanSu,
  selectShowAddModalQLNhanSu,
  selectShowEditModalQLNhanSu,
} from "./QLNhanSu";

// Types
export type { QLHeThongState } from "./QLHeThong";
export type { QLNhanSuState } from "./QLNhanSu";
