import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { PhongHatSliceState } from "../types";
import {
    fetchAllPhongHatThunk,
    createPhongHatThunk,
    updatePhongHatThunk,
    toggleNgungHoatDongThunk,
    toggleDangSuDungThunk,
} from "./thunks";

export const phongHatExtraReducers = (builder: ActionReducerMapBuilder<PhongHatSliceState>) => {
    builder
        .addCase(fetchAllPhongHatThunk.pending, (s) => { s.loading = true; s.error = null; })
        .addCase(fetchAllPhongHatThunk.fulfilled, (s, a) => {
            s.loading = false;
            s.data = a.payload;
            s.total = a.payload.length;
        })
        .addCase(fetchAllPhongHatThunk.rejected, (s, a) => {
            s.loading = false;
            s.error = a.payload || "Lỗi";
        })
        .addCase(createPhongHatThunk.fulfilled, (s, a) => {
            s.data.unshift(a.payload);
            s.total = s.data.length;
        })
        .addCase(updatePhongHatThunk.fulfilled, (s, a) => {
            const i = s.data.findIndex(p => p.maPhong === a.payload.maPhong);
            if (i >= 0) s.data[i] = a.payload;
        })
        .addCase(toggleNgungHoatDongThunk.fulfilled, (s, a) => {
            const row = s.data.find(p => p.maPhong === a.payload.maPhong);
            if (row) row.ngungHoatDong = a.payload.ngungHoatDong;
        })
        .addCase(toggleDangSuDungThunk.fulfilled, (s, a) => {
            const row = s.data.find(p => p.maPhong === a.payload.maPhong);
            if (row) row.dangSuDung = a.payload.dangSuDung;
        });
};