import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { QLHeThongState } from "./types";
import {
  fetchAllNhanVien,
  fetchAllKhachHang,
  createNhanVien,
  fetchAllAdminAccount,
} from "./thunks";
import { fetchLoaiTaiKhoan } from "./thunks";

// 🔥 Extra Reducers for Async Thunks
export const qlHeThongExtraReducers = (
  builder: ActionReducerMapBuilder<QLHeThongState>
) => {
  // 📋 Fetch All Nhân viên
  builder
    .addCase(fetchAllNhanVien.pending, (state) => {
      state.nhanVien.loading = true;
      state.nhanVien.error = null;
    })
    .addCase(fetchAllNhanVien.fulfilled, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.data = action.payload;
      state.nhanVien.total = action.payload.length;
      state.nhanVien.error = null;
    })
    .addCase(fetchAllNhanVien.rejected, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.error = action.payload as string;
    });

  // 👤 Fetch All Khách hàng
  builder
    .addCase(fetchAllKhachHang.pending, (state) => {
      state.khachHang.loading = true;
      state.khachHang.error = null;
    })
    .addCase(fetchAllKhachHang.fulfilled, (state, action) => {
      state.khachHang.loading = false;
      state.khachHang.data = action.payload;
      state.khachHang.total = action.payload.length;
      state.khachHang.error = null;
    })
    .addCase(fetchAllKhachHang.rejected, (state, action) => {
      state.khachHang.loading = false;
      state.khachHang.error = action.payload as string;
    });

  // 🏷️ Fetch Loại tài khoản
  builder
    .addCase(fetchLoaiTaiKhoan.pending, (state) => {
      state.loaiTaiKhoan.loading = true;
      state.loaiTaiKhoan.error = null;
    })
    .addCase(fetchLoaiTaiKhoan.fulfilled, (state, action) => {
      state.loaiTaiKhoan.loading = false;
      state.loaiTaiKhoan.data = action.payload;
      state.loaiTaiKhoan.error = null;
    })
    .addCase(fetchLoaiTaiKhoan.rejected, (state, action) => {
      state.loaiTaiKhoan.loading = false;
      state.loaiTaiKhoan.error = action.payload as string;
    });

  // ➕ Create Nhân viên
  builder
    .addCase(createNhanVien.pending, (state) => {
      state.nhanVien.loading = true;
      state.nhanVien.error = null;
    })
    .addCase(createNhanVien.fulfilled, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.data.push(action.payload);
      state.nhanVien.total += 1;
      state.nhanVien.error = null;
    })
    .addCase(createNhanVien.rejected, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.error = action.payload as string;
    });

  //Extra reducer quản lý tài khoản
  builder
    .addCase(fetchAllAdminAccount.pending, (state) => {
      state.adminAccount.loading = true;
      state.adminAccount.error = null;
    })
    .addCase(fetchAllAdminAccount.fulfilled, (state, action) => {
      state.adminAccount.loading = false;
      state.adminAccount.data = action.payload;
      state.adminAccount.error = null;
    })
    .addCase(fetchAllAdminAccount.rejected, (state, action) => {
      state.adminAccount.loading = false;
      state.adminAccount.error = action.payload as string;
    });
};
