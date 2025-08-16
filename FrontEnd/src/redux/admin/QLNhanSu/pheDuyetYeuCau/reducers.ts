import type { PayloadAction } from "@reduxjs/toolkit";
import type { PheDuyetYeuCauChuyenCaSliceState } from "../types";

export const localReducers = {
    setSearchYeuCau(state: PheDuyetYeuCauChuyenCaSliceState, a: PayloadAction<string>) {
        state.ui.search = a.payload;
    },
    setStatusFilterYeuCau(
        state: PheDuyetYeuCauChuyenCaSliceState,
        a: PayloadAction<"ALL" | "PENDING" | "APPROVED">
    ) {
        state.ui.statusFilter = a.payload;
    },
    clearPheDuyetYeuCauError(state: PheDuyetYeuCauChuyenCaSliceState) {
        state.error = null;
    },
    resetPheDuyetYeuCauState() {
        // returning initialState handled in slice import
        return undefined as any;
    },
};