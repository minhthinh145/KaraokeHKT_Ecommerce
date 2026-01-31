import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { VatLieuSliceState } from "../types";
import { fetchAllVatLieu, createVatLieu, updateSoLuongVatLieu, updateNgungCungCap, updateVatLieu } from "./thunks";

export const registerVatLieuExtraReducers = (builder: ActionReducerMapBuilder<VatLieuSliceState>) => {
    builder
        .addCase(fetchAllVatLieu.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(fetchAllVatLieu.fulfilled, (state, action) => {
            state.loading = false;
            state.items = action.payload;
            state.total = action.payload.length;
        })
        .addCase(fetchAllVatLieu.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload || "Tải danh sách thất bại";
        })

        .addCase(createVatLieu.pending, (state) => {
            state.loading = true;
        })
        .addCase(createVatLieu.fulfilled, (state, action) => {
            state.loading = false;
            state.items.unshift(action.payload);
            state.total = state.items.length;
        })
        .addCase(createVatLieu.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload || "Tạo vật liệu thất bại";
        })

        .addCase(updateSoLuongVatLieu.fulfilled, (state, action) => {
            const { maVatLieu, soLuongMoi } = action.payload;
            const i = state.items.findIndex((x) => x.maVatLieu === maVatLieu);
            if (i !== -1) state.items[i].soLuongTonKho = soLuongMoi;
            if (state.ui.selected?.maVatLieu === maVatLieu) state.ui.selected.soLuongTonKho = soLuongMoi;
        })

        .addCase(updateNgungCungCap.fulfilled, (state, action) => {
            const { maVatLieu, ngungCungCap } = action.payload;
            const i = state.items.findIndex((x) => x.maVatLieu === maVatLieu);
            if (i !== -1) state.items[i].ngungCungCap = ngungCungCap;
            if (state.ui.selected?.maVatLieu === maVatLieu) state.ui.selected.ngungCungCap = ngungCungCap;
        })

        .addCase(updateVatLieu.fulfilled, (state, action) => {
            const updated = action.payload;
            const i = state.items.findIndex(x => x.maVatLieu === updated.maVatLieu);
            if (i !== -1) state.items[i] = { ...state.items[i], ...updated };
            if (state.ui.selected?.maVatLieu === updated.maVatLieu) {
                state.ui.selected = { ...state.ui.selected, ...updated };
            }
        });
};