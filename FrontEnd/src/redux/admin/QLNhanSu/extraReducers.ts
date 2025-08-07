import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { QLNhanSuState } from "./types";
import {
  fetchAllNhanVienQLNhanSu,
  createNhanVienQLNhanSu,
  updateNhanVienQLNhanSu,
} from "./thunks";

// 🔥 Extra Reducers for Async Thunks
export const qlNhanSuExtraReducers = (
  builder: ActionReducerMapBuilder<QLNhanSuState>
) => {
  // 📋 Fetch All Nhân viên
  builder
    .addCase(fetchAllNhanVienQLNhanSu.pending, (state) => {
      state.nhanVien.loading = true;
      state.nhanVien.error = null;
    })
    .addCase(fetchAllNhanVienQLNhanSu.fulfilled, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.data = action.payload;
      state.nhanVien.total = action.payload.length;
      state.nhanVien.error = null;
    })
    .addCase(fetchAllNhanVienQLNhanSu.rejected, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.error = action.payload as string;
    });

  // ➕ Create Nhân viên
  builder
    .addCase(createNhanVienQLNhanSu.pending, (state) => {
      state.nhanVien.loading = true;
      state.nhanVien.error = null;
    })
    .addCase(createNhanVienQLNhanSu.fulfilled, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.data.push(action.payload);
      state.nhanVien.total += 1;
      state.nhanVien.error = null;
      state.ui.showAddModal = false;
      state.ui.selectedNhanVien = null;
    })
    .addCase(createNhanVienQLNhanSu.rejected, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.error = action.payload as string;
    });

  // ✏️ Update Nhân viên
  builder
    .addCase(updateNhanVienQLNhanSu.pending, (state) => {
      state.nhanVien.loading = true;
      state.nhanVien.error = null;
    })
    .addCase(updateNhanVienQLNhanSu.fulfilled, (state, action) => {
      state.nhanVien.loading = false;

      const index = state.nhanVien.data.findIndex(
        (nv) => nv.maNv === action.payload.maNv
      );
      if (index !== -1) {
        state.nhanVien.data[index] = action.payload;
      }

      state.nhanVien.error = null;
      state.ui.showEditModal = false;
      state.ui.selectedNhanVien = null;
    })
    .addCase(updateNhanVienQLNhanSu.rejected, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.error = action.payload as string;
    });
};
