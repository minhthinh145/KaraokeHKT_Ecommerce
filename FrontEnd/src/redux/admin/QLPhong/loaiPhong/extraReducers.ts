import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { LoaiPhongSliceState } from "../types";
import {
    fetchAllLoaiPhongThunk,
    createLoaiPhongThunk,
    updateLoaiPhongThunk,
} from "./thunks";

export const loaiPhongExtraReducers = (
    builder: ActionReducerMapBuilder<LoaiPhongSliceState>
) => {
    builder
        .addCase(fetchAllLoaiPhongThunk.pending, (s) => {
            s.loading = true;
            s.error = null;
        })
        .addCase(fetchAllLoaiPhongThunk.fulfilled, (s, a) => {
            s.loading = false;
            s.data = a.payload;
            s.total = a.payload.length;
        })
        .addCase(fetchAllLoaiPhongThunk.rejected, (s, a) => {
            s.loading = false;
            s.error = a.payload || "Lỗi";
        })
        .addCase(createLoaiPhongThunk.fulfilled, (s, a) => {
            s.data.unshift(a.payload);
            s.total = s.data.length;
        })
        .addCase(updateLoaiPhongThunk.fulfilled, (s, a) => {
            const i = s.data.findIndex((x) => x.maLoaiPhong === a.payload.maLoaiPhong);
            if (i >= 0) s.data[i] = a.payload;
        });
};