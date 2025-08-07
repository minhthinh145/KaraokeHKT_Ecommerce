import { useEffect, useMemo, useCallback } from "react";
import { useAppDispatch, useAppSelector } from "../../src/redux/hooks";
import {
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
  selectNhanVienStateQLNhanSu,
  selectUIStateQLNhanSu,
  selectFilteredNhanVienQLNhanSu,
  selectNhanVienStatsQLNhanSu,
  selectSelectedNhanVienQLNhanSu,
  selectShowAddModalQLNhanSu,
  selectShowEditModalQLNhanSu,
} from "../../src/redux";

import type {
  AddNhanVienDTO,
  NhanVienDTO,
  QL_NHAN_SU_FILTER_OPTIONS,
} from "../../src/api";

export const useQLNhanSu = () => {
  const dispatch = useAppDispatch();

  // 🎯 Selectors
  const nhanVienState = useAppSelector(selectNhanVienStateQLNhanSu);
  const uiState = useAppSelector(selectUIStateQLNhanSu);

  // 🔍 Filtered data
  const filteredNhanVien = useAppSelector(selectFilteredNhanVienQLNhanSu);

  // 📊 Stats
  const nhanVienStats = useAppSelector(selectNhanVienStatsQLNhanSu);

  // 🎯 Modal states
  const selectedNhanVien = useAppSelector(selectSelectedNhanVienQLNhanSu);
  const showAddModal = useAppSelector(selectShowAddModalQLNhanSu);
  const showEditModal = useAppSelector(selectShowEditModalQLNhanSu);

  // 🎬 Actions
  const actions = useMemo(
    () => ({
      // 🔍 Search & Filter
      search: (query: string) => {
        dispatch(setSearchQueryQLNhanSu(query));
      },

      filterByType: (type: string) => {
        dispatch(setLoaiNhanVienFilter(type));
      },

      filterByStatus: (status: string | undefined) => {
        dispatch(setTrangThaiFilterQLNhanSu(status));
      },

      clearAllFilters: () => {
        dispatch(clearFiltersQLNhanSu());
      },

      // 📊 Data Loading
      loadNhanVien: () => {
        return dispatch(fetchAllNhanVienQLNhanSu());
      },

      // ➕ CRUD Operations
      addNhanVien: (data: AddNhanVienDTO) => {
        return dispatch(createNhanVienQLNhanSu(data));
      },

      updateNhanVien: (data: NhanVienDTO) => {
        return dispatch(updateNhanVienQLNhanSu(data));
      },

      removeNhanVien: (maNv: string) => {
        dispatch(removeNhanVienFromList(maNv));
      },

      // 📝 Modal Management
      openAddModal: () => {
        dispatch(setShowAddModal(true));
      },

      closeAddModal: () => {
        dispatch(setShowAddModal(false));
      },

      openEditModal: (nhanVien: NhanVienDTO) => {
        dispatch(setSelectedNhanVien(nhanVien));
        dispatch(setShowEditModal(true));
      },

      closeEditModal: () => {
        dispatch(setShowEditModal(false));
      },

      selectNhanVien: (nhanVien: NhanVienDTO | null) => {
        dispatch(setSelectedNhanVien(nhanVien));
      },

      // 🧹 Error Management
      clearErrors: () => {
        dispatch(clearNhanVienErrorQLNhanSu());
      },
    }),
    [dispatch]
  );

  // 🎯 Auto-load data on mount
  useEffect(() => {
    if (nhanVienState.data.length === 0 && !nhanVienState.loading) {
      actions.loadNhanVien();
    }
  }, []);

  // 🎯 Filter options - Match exact backend role strings
  const filterOptions = useMemo(() => {
    const employeeRoles = [
      { value: "All", label: "Tất cả nhân viên" },
      { value: "Nhân viên kho", label: "Nhân viên kho" },
      { value: "Nhân viên phục vụ", label: "Nhân viên phục vụ" },
      { value: "Nhân viên tiếp tân", label: "Nhân viên tiếp tân" },
      { value: "QuanLyKho", label: "Quản lý kho vật liệu" },
      { value: "QuanTriHeThong", label: "Quản trị hệ thống" },
      { value: "QuanLyPhongHat", label: "Quản lý phòng hát" },
      { value: "QuanLyNhanSu", label: "Quản lý nhân sự" },
    ];

    return employeeRoles;
  }, []);

  // 🎯 Loading states
  const isLoading = useMemo(
    () => ({
      nhanVien: nhanVienState.loading,
      any: nhanVienState.loading,
    }),
    [nhanVienState.loading]
  );

  // 🎯 Error states
  const errors = useMemo(
    () => ({
      nhanVien: nhanVienState.error,
      any: !!nhanVienState.error,
    }),
    [nhanVienState.error]
  );

  // 🎯 CRUD handlers with error handling
  const handleAddNhanVien = useCallback(
    async (data: AddNhanVienDTO) => {
      try {
        const result = await actions.addNhanVien(data);
        if (result.meta.requestStatus === "fulfilled") {
          return { success: true, data: result.payload };
        } else {
          return { success: false, error: result.payload as string };
        }
      } catch (error: any) {
        return { success: false, error: error.message || "Có lỗi xảy ra" };
      }
    },
    [actions]
  );

  const handleUpdateNhanVien = useCallback(
    async (data: NhanVienDTO) => {
      try {
        const result = await actions.updateNhanVien(data);
        if (result.meta.requestStatus === "fulfilled") {
          return { success: true, data: result.payload };
        } else {
          return { success: false, error: result.payload as string };
        }
      } catch (error: any) {
        return { success: false, error: error.message || "Có lỗi xảy ra" };
      }
    },
    [actions]
  );

  const handleDeleteNhanVien = useCallback(
    (maNv: string) => {
      // TODO: Implement actual delete API call when available
      actions.removeNhanVien(maNv);
      return { success: true };
    },
    [actions]
  );

  // 🎯 Return hook interface
  return {
    // 📊 State Data
    state: {
      nhanVien: {
        raw: nhanVienState.data,
        filtered: filteredNhanVien,
        total: nhanVienState.total,
        stats: nhanVienStats,
      },
    },

    // 🔄 UI State
    ui: {
      searchQuery: uiState.searchQuery,
      filters: uiState.filters,
      filterOptions,
      modals: {
        showAdd: showAddModal,
        showEdit: showEditModal,
        selectedNhanVien,
      },
    },

    // ⚡ Loading & Error States
    loading: isLoading,
    errors,

    // 🎬 Actions
    actions: {
      // Search & Filter
      search: actions.search,
      filterByType: actions.filterByType,
      filterByStatus: actions.filterByStatus,
      clearFilters: actions.clearAllFilters,

      // Data Management
      loadData: actions.loadNhanVien,

      // Modal Management
      openAddModal: actions.openAddModal,
      closeAddModal: actions.closeAddModal,
      openEditModal: actions.openEditModal,
      closeEditModal: actions.closeEditModal,

      // Error Management
      clearErrors: actions.clearErrors,
    },

    // 🔄 CRUD Handlers
    handlers: {
      addNhanVien: handleAddNhanVien,
      updateNhanVien: handleUpdateNhanVien,
      deleteNhanVien: handleDeleteNhanVien,
    },
  };
};

// 🎯 Export hook type for easier imports
export type UseQLNhanSuReturn = ReturnType<typeof useQLNhanSu>;
