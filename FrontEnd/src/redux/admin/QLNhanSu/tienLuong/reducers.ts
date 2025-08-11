import type { PayloadAction } from "@reduxjs/toolkit";
import type { TienLuongSliceState } from "../types";
import type { LuongCaLamViecDTO } from "../../../../api";

export const tienLuongReducers = {
  setCurrentTienLuong: (
    state: TienLuongSliceState,
    action: PayloadAction<LuongCaLamViecDTO>
  ) => {
    state.current = action.payload;
  },
  clearTienLuongError: (state: TienLuongSliceState) => {
    state.error = null;
  },
  clearTienLuongCurrent: (state: TienLuongSliceState) => {
    state.current = null;
  },
  resetTienLuongState(): TienLuongSliceState {
    return {
      data: [],
      current: null,
      loading: false,
      error: null,
      total: 0,
      ui: {
        searchQuery: "",
        filters: {
          selectedCa: "ALL",
          dateRange: [null, null],
        },
        selectedTienLuong: null,
        showAddModal: false,
        showEditModal: false,
      },
    };
  },
  // ✨ UI Actions
  setSearchQuery: (
    state: TienLuongSliceState,
    action: PayloadAction<string>
  ) => {
    state.ui.searchQuery = action.payload;
  },
  setSelectedCa: (
    state: TienLuongSliceState,
    action: PayloadAction<number | "ALL">
  ) => {
    state.ui.filters.selectedCa = action.payload;
  },
  setDateRange: (
    state: TienLuongSliceState,
    action: PayloadAction<[string | null, string | null]>
  ) => {
    state.ui.filters.dateRange = action.payload;
  },
  openAddModal: (state: TienLuongSliceState) => {
    state.ui.showAddModal = true;
  },
  closeAddModal: (state: TienLuongSliceState) => {
    state.ui.showAddModal = false;
  },
  openEditModal: (
    state: TienLuongSliceState,
    action: PayloadAction<LuongCaLamViecDTO>
  ) => {
    state.ui.showEditModal = true;
    state.ui.selectedTienLuong = action.payload;
  },
  closeEditModal: (state: TienLuongSliceState) => {
    state.ui.showEditModal = false;
    state.ui.selectedTienLuong = null;
  },
  clearAllFilters: (state: TienLuongSliceState) => {
    state.ui.searchQuery = "";
    state.ui.filters.selectedCa = "ALL";
    state.ui.filters.dateRange = [null, null];
  },
};
