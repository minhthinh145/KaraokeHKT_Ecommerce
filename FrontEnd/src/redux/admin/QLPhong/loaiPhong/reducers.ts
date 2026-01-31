import type { PayloadAction } from "@reduxjs/toolkit";
import type { LoaiPhongSliceState, LoaiPhongDTO } from "../types";

export const loaiPhongReducers = {
    resetLoaiPhongState(): LoaiPhongSliceState {
        return {
            data: [],
            loading: false,
            error: null,
            total: 0,
            ui: {
                searchQuery: "",
                showAddModal: false,
                showEditModal: false,
                selected: null,
            },
        };
    },

    // UI
    setLoaiPhongSearchQuery(state: LoaiPhongSliceState, action: PayloadAction<string>) {
        state.ui.searchQuery = action.payload;
    },
    openAddLoaiPhongModal(state: LoaiPhongSliceState) {
        state.ui.showAddModal = true;
    },
    closeAddLoaiPhongModal(state: LoaiPhongSliceState) {
        state.ui.showAddModal = false;
    },
    openEditLoaiPhongModal(state: LoaiPhongSliceState, action: PayloadAction<LoaiPhongDTO>) {
        state.ui.selected = action.payload;
        state.ui.showEditModal = true;
    },
    closeEditLoaiPhongModal(state: LoaiPhongSliceState) {
        state.ui.showEditModal = false;
        state.ui.selected = null;
    },
};