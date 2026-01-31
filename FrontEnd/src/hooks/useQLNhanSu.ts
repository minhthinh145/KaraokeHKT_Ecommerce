import { useNhanVien } from "./QLNhanSu/useNhanVien";
import { useNhanVienUI } from "./QLNhanSu/useNhanVienUI";
import { useNhanVienFilters } from "./QLNhanSu/useNhanVienFilters";
import { useTienLuong } from "./QLNhanSu/useTienLuong";
import { useLichLamViec } from "./QLNhanSu/useLichLamViec";
import { useCaLamViec } from "./QLNhanSu/useCaLamViec";
import { usePheDuyetYeuCauChuyenCa } from "./QLNhanSu/usePheDuyetYeuCauChuyenCa";
export const useQLNhanSu = () => {
  // Nhân viên
  const nhanVien = useNhanVien({ autoLoad: true });
  const nhanVienUI = useNhanVienUI();
  const nhanVienFilters = useNhanVienFilters();

  // Tiền lương
  const tienLuong = useTienLuong({ autoLoad: true });
  const caLamViec = useCaLamViec(); // Thêm dòng này
  const lichLamViec = useLichLamViec({ autoLoad: true });

  // NEW: Phe duyet yêu cầu chuyển ca
  const pheDuyet = usePheDuyetYeuCauChuyenCa();

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
    updateDaNghiViec: nhanVien.toggleNghiViec,
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

    // Ca làm việc
    caLamViecList: caLamViec.caLamViecList,
    caMap: caLamViec.caMap,
    caOptions: caLamViec.caOptions,
    caLamViecLoading: caLamViec.loading,
    caLamViecError: caLamViec.error,
    refreshCaLamViec: caLamViec.refreshCaLamViec,
    addCaLamViec: caLamViec.addCaLamViec,
    setCurrentCaLamViec: caLamViec.setCurrent,
    clearCaLamViecError: caLamViec.clearError,
    clearCaLamViecCurrent: caLamViec.clearCurrent,
    resetCaLamViecState: caLamViec.resetState,

    // Lịch làm việc
    lichLamViecData: lichLamViec.data,
    filteredLichLamViec: lichLamViec.filtered,
    lichLamViecStats: lichLamViec.stats,
    lichLamViecUI: lichLamViec.ui,
    lichLamViecLoading: lichLamViec.loading,
    lichLamViecError: lichLamViec.error,
    lichLamViecActions: {
      refresh: lichLamViec.refresh,
      setSearch: lichLamViec.setSearch,
      setNhanVienFilter: lichLamViec.setNhanVienFilter,
      setDateRange: lichLamViec.setDateRangeFilter,
      clearFilters: lichLamViec.clearFiltersAction,
      loadByNhanVien: lichLamViec.loadByNhanVien,
      loadByRange: lichLamViec.loadByRange,
      openEdit: lichLamViec.openEditModalAction,
      closeEdit: lichLamViec.closeEditModalAction,     // thêm
      sendNotiRange: lichLamViec.sendNotiRange,


    },
    lichLamViecHandlers: {
      add: lichLamViec.add,
      update: lichLamViec.update,
      remove: lichLamViec.remove,
    },
    lichLamViecSendingNoti: lichLamViec.sendingNoti,

    // Phe duyet yêu cau
    pheDuyetLoading: pheDuyet.loading,
    pheDuyetApproving: pheDuyet.approving,
    pheDuyetLists: pheDuyet.lists,
    pheDuyetCurrent: pheDuyet.current,
    pheDuyetUI: pheDuyet.ui,
    pheDuyetFiltered: pheDuyet.filtered,
    pheDuyetError: pheDuyet.error,
    pheDuyetActions: {
      loadAll: pheDuyet.loadAll,
      loadDetail: pheDuyet.loadDetail,
      approve: pheDuyet.approve, // (maYeuCau, ghiChu?, ketQua?)
      setSearch: pheDuyet.setSearch,
      setStatusFilter: pheDuyet.setStatusFilter,
      clearError: pheDuyet.clearError,
    },

  };

};
