import {
  EMPLOYEE_ROLES,
  MANAGER_ROLES,
  RoleDescriptions,
} from "../../../constants/auth";
import { createSelector } from "@reduxjs/toolkit";
import type { RootState } from "../../store";

// 🎯 Base selectors - Fix property access
export const selectQLHeThong = (state: RootState) => state.qlHeThong;

export const selectNhanVienState = createSelector(
  [selectQLHeThong],
  (qlHeThong) => qlHeThong.nhanVien
);

export const selectKhachHangState = createSelector(
  [selectQLHeThong],
  (qlHeThong) => qlHeThong.khachHang
);

export const selectLoaiTaiKhoanState = createSelector(
  [selectQLHeThong],
  (qlHeThong) => qlHeThong.loaiTaiKhoan
);

export const selectUIState = createSelector(
  [selectQLHeThong],
  (qlHeThong) => qlHeThong.ui
);

export const selectAdminAccountState = createSelector(
  [selectQLHeThong],
  (qlHeThong) => qlHeThong.adminAccount
);
// 🔍 Filtered selectors
export const selectFilteredNhanVien = createSelector(
  [selectNhanVienState, selectUIState],
  (nhanVienState, uiState) => {
    const { data } = nhanVienState;
    const { searchQuery, filters } = uiState;
    const base = data.filter((item) =>
      EMPLOYEE_ROLES.includes(item.loaiTaiKhoan as any)
    );

    let result = base;

    // Filter role cụ thể (trong nhóm nhân viên)
    if (filters.loaiTaiKhoan && filters.loaiTaiKhoan !== "All") {
      result = result.filter((r) => r.loaiTaiKhoan === filters.loaiTaiKhoan);
    }

    // Trạng thái
    if (filters.trangThai) {
      result = result.filter((acc) => {
        if (filters.trangThai === "active")
          return acc.daKichHoat && !acc.daBiKhoa;
        if (filters.trangThai === "inactive") return !acc.daKichHoat;
        if (filters.trangThai === "locked") return acc.daBiKhoa;
        return true;
      });
    }

    // Search
    if (searchQuery?.trim()) {
      const q = searchQuery.toLowerCase();
      result = result.filter(
        (acc) =>
          (acc.hoTen || "").toLowerCase().includes(q) ||
          (acc.email || "").toLowerCase().includes(q) ||
          (acc.userName || "").toLowerCase().includes(q)
      );
    }

    return result;
  }
);

export const selectFilteredKhachHang = createSelector(
  [selectKhachHangState, selectUIState],
  (khachHangState, uiState) => {
    let filteredData = [...khachHangState.data];

    // 🔥 Search filter - Updated field names
    if (uiState.searchQuery.trim()) {
      const query = uiState.searchQuery.toLowerCase();
      filteredData = filteredData.filter(
        (kh) =>
          kh.tenKhachHang.toLowerCase().includes(query) || // 🔥 Changed from hoTen
          kh.fullName.toLowerCase().includes(query) || // 🔥 Added fullName
          kh.email.toLowerCase().includes(query) ||
          kh.userName.toLowerCase().includes(query) // 🔥 Added userName
      );
    }

    // 🔥 Status filter - Use daKichHoat/daBiKhoa instead of trangThai
    if (uiState.filters.trangThai) {
      filteredData = filteredData.filter((kh) => {
        if (uiState.filters.trangThai === "active") {
          return kh.daKichHoat && !kh.daBiKhoa;
        } else if (uiState.filters.trangThai === "inactive") {
          return !kh.daKichHoat;
        } else if (uiState.filters.trangThai === "locked") {
          return kh.daBiKhoa;
        }
        return true;
      });
    }

    return filteredData;
  }
);

export const selectFilteredAdminAccount = createSelector(
  [selectAdminAccountState, selectUIState],
  (adminAccountState, uiState) => {
    let filtered = adminAccountState.data;

    // Filter by search query
    if (uiState.searchQuery) {
      const query = uiState.searchQuery.toLowerCase();
      filtered = filtered.filter(
        (item) =>
          item.maTaiKhoan.toLowerCase().includes(query) ||
          item.email.toLowerCase().includes(query)
      );
    }

    // Filter by role
    if (
      uiState.filters.loaiTaiKhoan &&
      uiState.filters.loaiTaiKhoan !== "All"
    ) {
      filtered = filtered.filter(
        (item) => item.loaiTaiKhoan === uiState.filters.loaiTaiKhoan
      );
    }

    // Filter by status
    if (uiState.filters.trangThai) {
      switch (uiState.filters.trangThai) {
        case "active":
          filtered = filtered.filter(
            (item) => item.daKichHoat && !item.daBiKhoa
          );
          break;
        case "inactive":
          filtered = filtered.filter((item) => !item.daKichHoat);
          break;
        case "locked":
          filtered = filtered.filter((item) => item.daBiKhoa);
          break;
      }
    }

    return filtered;
  }
);

// 📊 Stats selectors
export const selectNhanVienStats = createSelector(
  [selectNhanVienState],
  (nhanVienState) => ({
    total: nhanVienState.total,
    active: nhanVienState.data.filter((nv) => nv.daKichHoat && !nv.daBiKhoa)
      .length,
    inactive: nhanVienState.data.filter((nv) => !nv.daKichHoat || nv.daBiKhoa)
      .length,
    byRole: nhanVienState.data.reduce((acc, nv) => {
      acc[nv.loaiTaiKhoan] = (acc[nv.loaiTaiKhoan] || 0) + 1;
      return acc;
    }, {} as Record<string, number>),
  })
);

export const selectKhachHangStats = createSelector(
  [selectKhachHangState],
  (khachHangState) => ({
    total: khachHangState.total,
    // 🔥 Fixed: Use daKichHoat/daBiKhoa instead of trangThai
    active: khachHangState.data.filter((kh) => kh.daKichHoat && !kh.daBiKhoa)
      .length,
    inactive: khachHangState.data.filter((kh) => !kh.daKichHoat || kh.daBiKhoa)
      .length,
  })
);

export const selectAdminAccountStats = createSelector(
  [selectFilteredAdminAccount],
  (filteredData) => ({
    total: filteredData.length,
    active: filteredData.filter((item) => item.daKichHoat && !item.daBiKhoa)
      .length,
    inactive: filteredData.filter((item) => !item.daKichHoat).length,
    locked: filteredData.filter((item) => item.daBiKhoa).length,
  })
);

// Options cho dropdown role theo “context” (employee | manager)
export const makeSelectRoleFilterOptions = (context: "employee" | "manager") =>
  createSelector([], () => {
    const roles = context === "employee" ? EMPLOYEE_ROLES : MANAGER_ROLES;
    return [
      { value: "All", label: "Tất cả vai trò" },
      ...roles.map((r) => ({ value: r, label: RoleDescriptions[r] || r })),
    ];
  });
