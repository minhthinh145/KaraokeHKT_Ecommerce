import type { PayloadAction } from "@reduxjs/toolkit";
import type { LichLamViecSliceState } from "../types";
import type { LichLamViecDTO } from "../../../../api";

export const lichLamViecReducers = {
  setCurrentLichLamViec: (
    state: LichLamViecSliceState,
    action: PayloadAction<LichLamViecDTO | null>
  ) => {
    state.current = action.payload;
  },
  clearLichLamViecError: (state: LichLamViecSliceState) => {
    state.error = null;
  },
  resetLichLamViecState(): LichLamViecSliceState {
    return {
      data: [],
      loading: false,
      error: null,
      total: 0,
      current: null,
      ui: {
        searchQuery: "",
        filters: { selectedNhanVien: "ALL", dateRange: [null, null] },
        showAddModal: false,
        showEditModal: false,
        sendingNoti: false,
      },
    };
  },

  // UI
  setSearchQuery: (
    state: LichLamViecSliceState,
    action: PayloadAction<string>
  ) => {
    state.ui.searchQuery = action.payload;
  },
  setSelectedNhanVien: (
    state: LichLamViecSliceState,
    action: PayloadAction<string | "ALL">
  ) => {
    state.ui.filters.selectedNhanVien = action.payload;
  },
  setDateRange: (
    state: LichLamViecSliceState,
    action: PayloadAction<[string | null, string | null]>
  ) => {
    state.ui.filters.dateRange = action.payload;
  },
  clearAllFilters: (state: LichLamViecSliceState) => {
    state.ui.searchQuery = "";
    state.ui.filters.selectedNhanVien = "ALL";
    state.ui.filters.dateRange = [null, null];
  },
  openEditModal: (
    state: LichLamViecSliceState,
    action: PayloadAction<LichLamViecDTO>
  ) => {
    state.current = action.payload;
    state.ui.showEditModal = true;
  },
  closeEditModal: (state: LichLamViecSliceState) => {
    state.ui.showEditModal = false;
    state.current = null;
  },
  setShowEditModal: (state: LichLamViecSliceState, action: PayloadAction<boolean>) => {
    state.ui.showEditModal = action.payload;
  },

  setSendingNoti: (state: LichLamViecSliceState, action: PayloadAction<boolean>) => {
    state.ui.sendingNoti = action.payload;
  },
  selectLichLamViecSendingNoti: (state: LichLamViecSliceState) => {
    state.ui.sendingNoti;
  }

};
