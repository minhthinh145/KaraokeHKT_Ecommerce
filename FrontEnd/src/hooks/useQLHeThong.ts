import { useEffect, useMemo } from "react";
import { useAppDispatch, useAppSelector } from "../../src/redux/hooks";

import {
  setActiveTab,
  setSearchQueryQLHeThong,
  setLoaiTaiKhoanFilter,
  setTrangThaiFilterQLHeThong,
  clearFiltersQLHeThong,
  selectUIStateQLHeThong,
} from "../../src/redux/admin";

import { useNhanVienAccount } from "./QLHeThong/useNhanVienAccount";
import { useKhachHangAccount } from "./QLHeThong/useKhachHangAccount";
import { useAdminAccount } from "./QLHeThong/useAdminAccount";
import { useLoaiTaiKhoan } from "./QLHeThong/useLoaiTaiKhoan";

export const useQLHeThong = () => {
  const dispatch = useAppDispatch();

  // 🎯 UI State selectors
  const uiState = useAppSelector(selectUIStateQLHeThong);

  // 🟣 Sử dụng các hook con - KHÔNG auto-load trong hook con
  const nhanVien = useNhanVienAccount({ autoLoad: false });
  const khachHang = useKhachHangAccount({ autoLoad: false });
  const admin = useAdminAccount({ autoLoad: false });
  const loaiTaiKhoan = useLoaiTaiKhoan({ autoLoad: false });

  // Auto load
  useEffect(() => {
    // Load loại tài khoản
    if (loaiTaiKhoan.loaiTaiKhoanData.length === 0 && !loaiTaiKhoan.loading) {
      dispatch(loaiTaiKhoan.thunks.fetchLoaiTaiKhoan());
    }

    // Load nhân viên
    if (nhanVien.nhanVienData.length === 0 && !nhanVien.loading) {
      dispatch(nhanVien.thunks.fetchAllNhanVienQLHeThong());
    }

    // Load khách hàng
    if (khachHang.khachHangData.length === 0 && !khachHang.loading) {
      dispatch(khachHang.thunks.fetchAllKhachHang());
    }

    // Load admin account
    if (admin.adminAccountData.length === 0 && !admin.loading) {
      dispatch(admin.thunks.fetchAllAdminAccount());
    }
  }, [dispatch]); // ✅ CHỈ phụ thuộc dispatch

  // 🎬 Common Actions
  const actions = useMemo(
    () => ({
      switchTab: (tab: "nhan-vien" | "khach-hang") => {
        dispatch(setActiveTab(tab));
      },
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

      // Data loading actions
      loadNhanVien: () => dispatch(nhanVien.thunks.fetchAllNhanVienQLHeThong()),
      loadKhachHang: () => dispatch(khachHang.thunks.fetchAllKhachHang()),
      loadAdminAccount: () => dispatch(admin.thunks.fetchAllAdminAccount()),
      loadLoaiTaiKhoan: () => dispatch(loaiTaiKhoan.thunks.fetchLoaiTaiKhoan()),

      // Error clearing
      clearErrors: () => {
        dispatch(nhanVien.actions.clearNhanVienError());
        dispatch(khachHang.actions.clearKhachHangError());
        dispatch(admin.actions.clearAdminAccountError());
        dispatch(loaiTaiKhoan.actions.clearLoaiTaiKhoanError());
      },
      clearNhanVienError: () => dispatch(nhanVien.actions.clearNhanVienError()),
      clearKhachHangError: () =>
        dispatch(khachHang.actions.clearKhachHangError()),
      clearAdminAccountError: () =>
        dispatch(admin.actions.clearAdminAccountError()),
      clearLoaiTaiKhoanError: () =>
        dispatch(loaiTaiKhoan.actions.clearLoaiTaiKhoanError()),
    }),
    [dispatch]
  );

  // 🎯 Filter options - SỬ DỤNG hook con
  const filterOptions = useMemo(() => {
    const baseOptions = [
      { value: "", label: "Tất cả loại tài khoản" },
      { value: "NhanVienKho", label: "Nhân viên kho" },
      { value: "NhanVienPhucVu", label: "Nhân viên phục vụ" },
      { value: "NhanVienTiepTan", label: "Nhân viên tiếp tân" },
      { value: "QuanTriHeThong", label: "Quản trị hệ thống" },
      { value: "KhachHang", label: "Khách hàng" },
    ];
    const dynamicOptions = loaiTaiKhoan.loaiTaiKhoanData // ✅ Dùng hook con
      .filter((role) => !baseOptions.some((opt) => opt.value === role))
      .map((role) => ({
        value: role,
        label: role.replace(/([A-Z])/g, " $1").trim(),
      }));
    return [...baseOptions, ...dynamicOptions];
  }, [loaiTaiKhoan.loaiTaiKhoanData]);

  // 🎯 Loading states
  const loading = useMemo(
    () => ({
      nhanVien: nhanVien.loading,
      khachHang: khachHang.loading,
      adminAccount: admin.loading,
      loaiTaiKhoan: loaiTaiKhoan.loading, // ✅ Dùng hook con
      any:
        nhanVien.loading ||
        khachHang.loading ||
        admin.loading ||
        loaiTaiKhoan.loading,
    }),
    [nhanVien.loading, khachHang.loading, admin.loading, loaiTaiKhoan.loading]
  );

  // 🎯 Error states
  const errors = useMemo(
    () => ({
      nhanVien: nhanVien.error,
      khachHang: khachHang.error,
      adminAccount: admin.error,
      loaiTaiKhoan: loaiTaiKhoan.error, // ✅ Dùng hook con
      any:
        nhanVien.error || khachHang.error || admin.error || loaiTaiKhoan.error,
    }),
    [nhanVien.error, khachHang.error, admin.error, loaiTaiKhoan.error]
  );

  // 🎯 UI state for component
  const ui = useMemo(
    () => ({
      searchQuery: uiState.searchQuery,
      filters: {
        loaiTaiKhoan: uiState.filters.loaiTaiKhoan,
        trangThai: uiState.filters.trangThai,
      },
      filteredNhanVien: nhanVien.filteredNhanVien,
      filteredKhachHang: khachHang.filteredKhachHang,
      filteredAdminAccount: admin.filteredAdminAccount,
      filterOptions,
    }),
    [
      uiState.searchQuery,
      uiState.filters,
      nhanVien.filteredNhanVien,
      khachHang.filteredKhachHang,
      admin.filteredAdminAccount,
      filterOptions,
    ]
  );

  return {
    // 📊 Main data
    nhanVienData: nhanVien.nhanVienData,
    khachHangData: khachHang.khachHangData,
    adminAccountData: admin.adminAccountData,
    loaiTaiKhoanData: loaiTaiKhoan.loaiTaiKhoanData, // ✅ Thêm data từ hook con

    // 🔄 UI State
    ui,

    // ⚡ Loading & Error States
    loading,
    errors,

    // 🎬 Actions
    actions,

    // 🔄 CRUD Handlers
    handlers: {
      addNhanVien: nhanVien.addNhanVienAccount,
      addAdminAccount: admin.addAdminAccount,
      deleteAdminAccount: admin.deleteAdminAccount,
      updateAdminAccount: admin.updateAdminAccount,
    },

    // 🔄 Lock handlers
    lockHandlers: {
      nhanVien: nhanVien.lockHandlers,
      khachHang: khachHang.lockHandlers,
      adminAccount: admin.lockHandlers,
    },

    // 📊 Stats
    stats: {
      nhanVien: nhanVien.nhanVienStats,
      khachHang: khachHang.khachHangStats,
      adminAccount: admin.adminAccountStats,
    },
  };
};

export type UseQLHeThongReturn = ReturnType<typeof useQLHeThong>;
