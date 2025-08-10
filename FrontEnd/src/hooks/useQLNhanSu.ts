import { useNhanVien } from "./QLNhanSu/useNhanVien";
import { useNhanVienUI } from "./QLNhanSu/useNhanVienUI";
import { useNhanVienFilters } from "./QLNhanSu/useNhanVienFilters";

export const useQLNhanSu = () => {
  const nhanVien = useNhanVien({ autoLoad: true });
  const ui = useNhanVienUI();
  const filters = useNhanVienFilters();

  // Actions
  const actions = {
    loadNhanVien: nhanVien.refreshNhanVienData,
    setSearchQuery: filters.setSearchQuery,
    setRoleFilter: filters.setRoleFilter,
    setStatusFilter: filters.setStatusFilter,
    clearAllFilters: filters.clearAllFilters,
    openAddModal: ui.openAddModal,
    closeAddModal: ui.closeAddModal,
    openEditModal: ui.openEditModal,
    closeEditModal: ui.closeEditModal,
  };

  // Handlers
  const handlers = {
    addNhanVien: nhanVien.addNhanVien,
    updateNhanVien: nhanVien.updateNhanVien,
    deleteNhanVien: nhanVien.deleteNhanVien,
  };

  return {
    nhanVienData: nhanVien.nhanVienData,
    filteredNhanVien: filters.filteredNhanVien,
    nhanVienStats: nhanVien.nhanVienStats,
    loading: { nhanVien: nhanVien.loading },
    errors: { nhanVien: nhanVien.error },
    ui: {
      searchQuery: filters.searchQuery,
      filters: {
        loaiNhanVien: filters.roleFilter,
        trangThai: filters.statusFilter,
      },
      showAddModal: ui.showAddModal,
      showEditModal: ui.showEditModal,
      selectedNhanVien: ui.selectedNhanVien,
    },
    actions,
    handlers,
  };
};
