import { useNhanVien } from "./QLNhanSu/useNhanVien";
import { useNhanVienUI } from "./QLNhanSu/useNhanVienUI";
import { useNhanVienFilters } from "./QLNhanSu/useNhanVienFilters";
import { useTienLuong } from "./QLNhanSu/useTienLuong";

export const useQLNhanSu = () => {
  // Nhân viên
  const nhanVien = useNhanVien({ autoLoad: true });
  const nhanVienUI = useNhanVienUI();
  const nhanVienFilters = useNhanVienFilters();

  // Tiền lương
  const tienLuong = useTienLuong({ autoLoad: true });

  // Actions
  const nhanVienActions = {
    load: nhanVien.refreshNhanVienData,
    setSearchQuery: nhanVienFilters.setSearchQuery,
    setRoleFilter: nhanVienFilters.setRoleFilter,
    setStatusFilter: nhanVienFilters.setStatusFilter,
    clearAllFilters: nhanVienFilters.clearAllFilters,
    openAddModal: nhanVienUI.openAddModal,
    closeAddModal: nhanVienUI.closeAddModal,
    openEditModal: nhanVienUI.openEditModal,
    closeEditModal: nhanVienUI.closeEditModal,
  };

  const tienLuongActions = {
    load: tienLuong.refreshTienLuongData,
    setSearchQuery: tienLuong.setSearch,
    setCaFilter: tienLuong.setCaFilter,
    setDateRange: tienLuong.setDateRangeFilter,
    clearAllFilters: tienLuong.clearFilters,
    openAddModal: tienLuong.openAdd,
    closeAddModal: tienLuong.closeAdd,
    openEditModal: tienLuong.openEdit,
    closeEditModal: tienLuong.closeEdit,
    setCurrent: tienLuong.setCurrent,
    clearError: tienLuong.clearError,
  };

  // Handlers
  const nhanVienHandlers = {
    add: nhanVien.addNhanVien,
    update: nhanVien.updateNhanVien,
    delete: nhanVien.deleteNhanVien,
  };

  const tienLuongHandlers = {
    add: tienLuong.addTienLuong,
    delete: tienLuong.removeTienLuong,
    // ...bổ sung nếu có các hàm khác
  };

  return {
    // Nhân viên
    nhanVienData: nhanVien.nhanVienData,
    filteredNhanVien: nhanVienFilters.filteredNhanVien,
    nhanVienStats: nhanVien.nhanVienStats,
    nhanVienUI: {
      ...nhanVienUI,
      searchQuery: nhanVienFilters.searchQuery,
      filters: {
        loaiNhanVien: nhanVienFilters.roleFilter,
        trangThai: nhanVienFilters.statusFilter,
      },
      setSearchQuery: nhanVienFilters.setSearchQuery,
      setRoleFilter: nhanVienFilters.setRoleFilter,
      setStatusFilter: nhanVienFilters.setStatusFilter,
      clearAllFilters: nhanVienFilters.clearAllFilters,
    },
    nhanVienLoading: nhanVien.loading,
    nhanVienError: nhanVien.error,
    nhanVienActions,
    nhanVienHandlers,

    // Tiền lương
    tienLuongData: tienLuong.tienLuongData,
    filteredTienLuong: tienLuong.filteredTienLuong,
    tienLuongStats: tienLuong.tienLuongStats,
    tienLuongUI: tienLuong.ui,
    tienLuongLoading: tienLuong.loading,
    tienLuongError: tienLuong.error,
    tienLuongActions,
    tienLuongHandlers,
  };
};
