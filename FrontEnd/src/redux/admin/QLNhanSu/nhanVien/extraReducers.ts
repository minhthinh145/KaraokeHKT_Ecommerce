import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { NhanVienSliceState } from "../types";
import { fetchAllNhanVien, createNhanVien, updateNhanVien, updateNhanVienDaNghiViecThunk } from "./thunks";
import { updateNhanVienDaNghiViec } from "../../../../api/services";

export const nhanVienExtraReducers = (
  builder: ActionReducerMapBuilder<NhanVienSliceState>
) => {
  builder
    .addCase(fetchAllNhanVien.pending, (state) => {
      state.loading = true;
      state.error = null;
    })
    .addCase(fetchAllNhanVien.fulfilled, (state, action) => {
      state.loading = false;
      state.data = action.payload;
      state.total = action.payload.length;
    })
    .addCase(fetchAllNhanVien.rejected, (state, action) => {
      state.loading = false;
      state.error = action.payload as string;
    })

    .addCase(createNhanVien.pending, (state) => {
      state.loading = true;
      state.error = null;
    })
    .addCase(createNhanVien.fulfilled, (state, action) => {
      state.loading = false;
      state.data.push(action.payload);
      state.total += 1;
      state.ui.showAddModal = false;
      state.ui.selectedNhanVien = null;
    })
    .addCase(createNhanVien.rejected, (state, action) => {
      state.loading = false;
      state.error = action.payload as string;
    })

    .addCase(updateNhanVien.pending, (state) => {
      state.loading = true;
      state.error = null;
    })
    .addCase(updateNhanVien.fulfilled, (state, action) => {
      state.loading = false;
      const idx = state.data.findIndex((nv) => nv.maNv === action.payload.maNv);
      if (idx !== -1) state.data[idx] = action.payload;
      state.ui.showEditModal = false;
      state.ui.selectedNhanVien = null;
    })
    .addCase(updateNhanVien.rejected, (state, action) => {
      state.loading = false;
      state.error = action.payload as string;
    });
  builder.addCase(updateNhanVienDaNghiViecThunk.fulfilled, (state, action) => {
    const { maNhanVien, daNghiViec } = action.payload;
    const i = state.data.findIndex((x) => x.maNv === maNhanVien);
    if (i !== -1) state.data[i] = { ...state.data[i], daNghiViec };
    // nếu có filteredData/entities, cập nhật tương tự
  });
  builder.addCase(updateNhanVienDaNghiViecThunk.rejected, (state, action) => {
    state.error = action.payload || "Cập nhật trạng thái nghỉ việc thất bại";
  });
};
