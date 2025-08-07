import { useEffect, useMemo, useCallback } from "react";
import { useAppDispatch, useAppSelector } from "../../src/redux/hooks";
import { message } from "antd";

// Import API functions
import {
  lockAccount,
  unlockAccount,
} from "../../src/api/services/shared/accountAPI";

import {
  // Thunks - Sử dụng tên đã rename
  fetchAllNhanVienQLHeThong,
  fetchAllKhachHang,
  fetchAllNhanVienQLNhanSu,
  fetchLoaiTaiKhoan,
  createNhanVienQLHeThong,

  // Actions - Sử dụng tên đã rename
  setActiveTab,
  setSearchQueryQLHeThong,
  setLoaiTaiKhoanFilter,
  setTrangThaiFilterQLHeThong,
  clearFiltersQLHeThong,
  clearNhanVienErrorQLHeThong,
  clearKhachHangError,
  clearLoaiTaiKhoanError,
  clearAdminAccountError,

  // Selectors - Sử dụng tên đã rename
  selectNhanVienStateQLHeThong,
  selectKhachHangState,
  selectLoaiTaiKhoanState,
  selectUIStateQLHeThong,
  selectFilteredNhanVienQLHeThong,
  selectFilteredKhachHang,
  selectNhanVienStatsQLHeThong,
  selectKhachHangStats,
  selectAdminAccountState,
  selectFilteredAdminAccount,
  selectAdminAccountStats,
  fetchAllAdminAccount,
} from "../../src/redux/admin";

import type {
  AddNhanVienDTO,
  AddTaiKhoanForNhanVienDTO,
} from "../../src/api/types";

export const useQLHeThong = () => {
  const dispatch = useAppDispatch();

  // 🎯 Selectors - Sử dụng tên đã rename
  const nhanVienState = useAppSelector(selectNhanVienStateQLHeThong);
  const khachHangState = useAppSelector(selectKhachHangState);
  const adminAccountState = useAppSelector(selectAdminAccountState);
  const loaiTaiKhoanState = useAppSelector(selectLoaiTaiKhoanState);

  const uiState = useAppSelector(selectUIStateQLHeThong);

  // 🔍 Filtered data - Sử dụng tên đã rename
  const filteredNhanVien = useAppSelector(selectFilteredNhanVienQLHeThong);
  const filteredKhachHang = useAppSelector(selectFilteredKhachHang);
  const filteredAdminAccount = useAppSelector(selectFilteredAdminAccount);

  // 📊 Stats - Sử dụng tên đã rename
  const nhanVienStats = useAppSelector(selectNhanVienStatsQLHeThong);
  const khachHangStats = useAppSelector(selectKhachHangStats);
  const adminAccountStats = useAppSelector(selectAdminAccountStats);

  // 🔒 Lock/Unlock handlers
  const handleLockAccount = useCallback(
    async (maTaiKhoan: string) => {
      try {
        const response = await lockAccount(maTaiKhoan);
        if (response.isSuccess) {
          message.success("Khóa tài khoản thành công!");
          // Reload data to refresh UI
          dispatch(fetchAllNhanVienQLHeThong());
          return { success: true };
        } else {
          message.error(response.message || "Không thể khóa tài khoản!");
          return { success: false, error: response.message };
        }
      } catch (error: any) {
        message.error(error.message || "Có lỗi xảy ra khi khóa tài khoản!");
        return { success: false, error: error.message };
      }
    },
    [dispatch]
  );

  const handleUnlockAccount = useCallback(
    async (maTaiKhoan: string) => {
      try {
        const response = await unlockAccount(maTaiKhoan);
        if (response.isSuccess) {
          message.success("Mở khóa tài khoản thành công!");
          // Reload data to refresh UI
          dispatch(fetchAllNhanVienQLHeThong());
          return { success: true };
        } else {
          message.error(response.message || "Không thể mở khóa tài khoản!");
          return { success: false, error: response.message };
        }
      } catch (error: any) {
        message.error(error.message || "Có lỗi xảy ra khi mở khóa tài khoản!");
        return { success: false, error: error.message };
      }
    },
    [dispatch]
  );

  // Combined lock/unlock toggle
  const handleLockToggle = useCallback(
    async (maTaiKhoan: string, isCurrentlyLocked: boolean) => {
      if (isCurrentlyLocked) {
        return await handleUnlockAccount(maTaiKhoan);
      } else {
        return await handleLockAccount(maTaiKhoan);
      }
    },
    [handleLockAccount, handleUnlockAccount]
  );

  // 🎬 Actions
  const actions = useMemo(
    () => ({
      // 🔄 Tab Management
      switchTab: (tab: "nhan-vien" | "khach-hang") => {
        dispatch(setActiveTab(tab));
      },

      // 🔍 Search & Filter - Updated cho component mới
      setSearchQuery: (query: string) => {
        dispatch(setSearchQueryQLHeThong(query));
      },

      setRoleFilter: (role: string) => {
        dispatch(setLoaiTaiKhoanFilter(role));
      },

      setStatusFilter: (status: string) => {
        dispatch(setTrangThaiFilterQLHeThong(status));
      },

      clearAllFilters: () => {
        dispatch(clearFiltersQLHeThong());
      },

      // 📊 Data Loading
      loadNhanVien: () => {
        return dispatch(fetchAllNhanVienQLHeThong());
      },

      loadKhachHang: () => {
        return dispatch(fetchAllKhachHang());
      },
      loadAdminAccount: () => {
        return dispatch(fetchAllAdminAccount());
      },

      loadLoaiTaiKhoan: () => {
        return dispatch(fetchLoaiTaiKhoan());
      },

      // ➕ CRUD Operations - Sử dụng tên đã rename
      addNhanVien: (data: AddTaiKhoanForNhanVienDTO) => {
        return dispatch(createNhanVienQLHeThong(data));
      },

      // 🧹 Error Management - Sử dụng tên đã rename
      clearErrors: () => {
        dispatch(clearNhanVienErrorQLHeThong());
        dispatch(clearKhachHangError());
        dispatch(clearLoaiTaiKhoanError());
      },

      clearNhanVienError: () => {
        dispatch(clearNhanVienErrorQLHeThong());
      },

      clearKhachHangError: () => {
        dispatch(clearKhachHangError());
      },
      clearAdminAccountError: () => {
        dispatch(clearAdminAccountError());
      },
    }),
    [dispatch]
  );

  // 🎯 Auto-load data on mount
  useEffect(() => {
    // Load loại tài khoản first (for filters)
    if (loaiTaiKhoanState.data.length === 0 && !loaiTaiKhoanState.loading) {
      actions.loadLoaiTaiKhoan();
    }
  }, []);

  // 🎯 Auto-load nhân viên data by default
  useEffect(() => {
    if (nhanVienState.data.length === 0 && !nhanVienState.loading) {
      actions.loadNhanVien();
    }
  }, []);

  // 🎯 Auto-load khách hàng data
  useEffect(() => {
    if (khachHangState.data.length === 0 && !khachHangState.loading) {
      actions.loadKhachHang();
    }
  }, []);

  // 🎯 Auto-load admin account data
  useEffect(() => {
    if (adminAccountState.data.length === 0 && !adminAccountState.loading) {
      actions.loadAdminAccount();
    }
  }, []);

  // 🎯 Filter options with computed values
  const filterOptions = useMemo(() => {
    const baseOptions = [
      { value: "", label: "Tất cả loại tài khoản" },

      { value: "NhanVienKho", label: "Nhân viên kho" },
      { value: "NhanVienPhucVu", label: "Nhân viên phục vụ" },
      { value: "NhanVienTiepTan", label: "Nhân viên tiếp tân" },
      { value: "QuanTriHeThong", label: "Quản trị hệ thống" },
      { value: "KhachHang", label: "Khách hàng" }, // 🔥 Add customer role
    ];

    // Add dynamic options from API if available
    const dynamicOptions = loaiTaiKhoanState.data
      .filter((role) => !baseOptions.some((opt) => opt.value === role))
      .map((role) => ({
        value: role,
        label: role.replace(/([A-Z])/g, " $1").trim(),
      }));

    return [...baseOptions, ...dynamicOptions];
  }, [loaiTaiKhoanState.data]);

  // 🎯 Loading states
  const loading = useMemo(
    () => ({
      nhanVien: nhanVienState.loading,
      khachHang: khachHangState.loading,
      loaiTaiKhoan: loaiTaiKhoanState.loading,
      adminAccount: adminAccountState.loading,
      any:
        nhanVienState.loading ||
        khachHangState.loading ||
        loaiTaiKhoanState.loading ||
        adminAccountState.loading,
    }),
    [
      nhanVienState.loading,
      khachHangState.loading,
      loaiTaiKhoanState.loading,
      adminAccountState.loading,
    ]
  );

  // 🎯 Error states
  const errors = useMemo(
    () => ({
      nhanVien: nhanVienState.error,
      khachHang: khachHangState.error,
      loaiTaiKhoan: loaiTaiKhoanState.error,
      adminAccount: adminAccountState.error,
      any:
        nhanVienState.error ||
        khachHangState.error ||
        loaiTaiKhoanState.error ||
        adminAccountState.error,
    }),
    [
      nhanVienState.error,
      khachHangState.error,
      loaiTaiKhoanState.error,
      adminAccountState.error,
    ]
  );

  // 🎯 UI state for component
  const ui = useMemo(
    () => ({
      searchQuery: uiState.searchQuery,
      filters: {
        loaiTaiKhoan: uiState.filters.loaiTaiKhoan,
        trangThai: uiState.filters.trangThai,
      },
      filteredNhanVien: filteredNhanVien, // 🔥 For NhanVien tab
      filteredKhachHang: filteredKhachHang, // 🔥 For KhachHang tab
      filteredAdminAccount: filteredAdminAccount, // 🔥 For AdminAccount tab

      filterOptions,
    }),
    [
      uiState.searchQuery,
      uiState.filters,
      filteredNhanVien,
      filteredKhachHang,
      filterOptions,
      filteredAdminAccount,
    ]
  );

  // 🎯 CRUD handlers with error handling
  const handleAddNhanVien = useCallback(
    async (data: AddTaiKhoanForNhanVienDTO) => {
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

  // 🎯 Return hook interface - Enhanced with KhachHang data
  return {
    // 📊 Main data
    data: nhanVienState.data,
    khachHangData: khachHangState.data, // 🔥 Add khachHang data
    adminAccountData: adminAccountState.data, // 🔥 Add admin account data

    // 🔄 UI State
    ui,

    // ⚡ Loading & Error States
    loading,
    errors,

    // 🎬 Actions - Simplified names
    actions: {
      setSearchQuery: actions.setSearchQuery,
      setRoleFilter: actions.setRoleFilter,
      setStatusFilter: actions.setStatusFilter,
      clearFilters: actions.clearAllFilters,
      loadNhanVien: actions.loadNhanVien,
      loadKhachHang: actions.loadKhachHang,
      loadAdminAccount: actions.loadAdminAccount,
      clearAdminAccountError: actions.clearAdminAccountError,
      clearErrors: actions.clearErrors,
    },

    // 🔄 CRUD Handlers
    handlers: {
      addNhanVien: handleAddNhanVien,
      lockAccount: handleLockAccount,
      unlockAccount: handleUnlockAccount,
      lockToggle: handleLockToggle, // 🔥 Main function to use in components
    },
  };
};

// 🎯 Export hook with type for easier imports
export type UseQLHeThongReturn = ReturnType<typeof useQLHeThong>;
