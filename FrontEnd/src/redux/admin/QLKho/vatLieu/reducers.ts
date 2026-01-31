import type { PayloadAction } from "@reduxjs/toolkit";
import type { VatLieuSliceState, VatLieuUISubState } from "../types";
import type { VatLieuDetailDTO } from "../../../../api/index";

export const vatLieuReducers = {
    setVatLieuSearchQuery(state: VatLieuSliceState, action: PayloadAction<string>) {
        state.ui.searchQuery = action.payload;
    },
    setVatLieuFilters(
        state: VatLieuSliceState,
        action: PayloadAction<Partial<VatLieuUISubState["filters"]>>
    ) {
        state.ui.filters = { ...state.ui.filters, ...action.payload };
    },
    setVatLieuSelected(state: VatLieuSliceState, action: PayloadAction<VatLieuDetailDTO | null>) {
        state.ui.selected = action.payload;
    },
    setVatLieuShowCreateModal(state: VatLieuSliceState, action: PayloadAction<boolean>) {
        state.ui.showCreateModal = action.payload;
    },
    setVatLieuShowUpdateSoLuongModal(state: VatLieuSliceState, action: PayloadAction<boolean>) {
        state.ui.showUpdateSoLuongModal = action.payload;
    },
    setVatLieuShowEditModal(state: VatLieuSliceState, action: PayloadAction<boolean>) {
        state.ui.showEditModal = action.payload;
    },
};