import type { PayloadAction } from "@reduxjs/toolkit";
import type { PhongHatSliceState, PhongHatDetailDTO, PhongHatStatusFilter } from "../types";

export const phongHatReducers = {
    resetPhongHatState(): PhongHatSliceState {
        return {
            data: [],
            loading: false,
            error: null,
            total: 0,
            ui: {
                searchQuery: "",
                statusFilter: "ALL",
                showAddModal: false,
                showEditModal: false,
                selected: null,
            },
        };
    },
    setPhongHatSearchQuery(state: PhongHatSliceState, action: PayloadAction<string>) {
        state.ui.searchQuery = action.payload;
    },
    setPhongHatStatusFilter(state: PhongHatSliceState, action: PayloadAction<PhongHatStatusFilter>) {
        state.ui.statusFilter = action.payload;
    },
    openAddPhongHatModal(state: PhongHatSliceState) {
        state.ui.showAddModal = true;
    },
    closeAddPhongHatModal(state: PhongHatSliceState) {
        state.ui.showAddModal = false;
    },
    openEditPhongHatModal(state: PhongHatSliceState, action: PayloadAction<PhongHatDetailDTO>) {
        state.ui.selected = action.payload;
        state.ui.showEditModal = true;
    },
    closeEditPhongHatModal(state: PhongHatSliceState) {
        state.ui.showEditModal = false;
        state.ui.selected = null;
    },
};