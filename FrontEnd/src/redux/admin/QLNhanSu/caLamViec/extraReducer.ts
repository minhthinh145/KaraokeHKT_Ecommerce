import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { CaLamViecSliceState } from "../types";
import { createCaLamViecThunk, fetchAllCaLamViec } from "./thunks";

export const caLamViecExtraReducers = (
  b: ActionReducerMapBuilder<CaLamViecSliceState>
) => {
  b.addCase(fetchAllCaLamViec.pending, (state) => {
    state.loading = true;
    state.error = null;
  })
    .addCase(fetchAllCaLamViec.fulfilled, (state, action) => {
      state.loading = false;
      state.data = action.payload;
      state.lastFetch = Date.now();
      state.error = null;
    })
    .addCase(fetchAllCaLamViec.rejected, (state, action) => {
      state.loading = false;
      state.error = action.payload || "Lỗi không xác định";
    })
    // Create
    .addCase(createCaLamViecThunk.pending, (state) => {
      state.loading = true;
      state.error = null;
    })
    .addCase(createCaLamViecThunk.fulfilled, (state, action) => {
      state.loading = false;
      state.data.push(action.payload);
      state.error = null;
    })
    .addCase(createCaLamViecThunk.rejected, (state, action) => {
      state.loading = false;
      state.error = action.payload || "Lỗi không xác định";
    });
};
