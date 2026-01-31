import { createSlice } from "@reduxjs/toolkit";
import { tienLuongInitialState } from "../types";
import { tienLuongReducers } from "./reducers";
import { tienLuongExtraReducers } from "./extraReducers";

const tienLuongSlice = createSlice({
  name: "qlNhanSu/tienLuong",
  initialState: tienLuongInitialState,
  reducers: tienLuongReducers,
  extraReducers: tienLuongExtraReducers,
});

export const {
  setCurrentTienLuong,
  clearTienLuongError,
  clearTienLuongCurrent,
  resetTienLuongState,
  // UI Actions
  setSearchQuery,
  setSelectedCa,
  setDateRange,
  openAddModal,
  closeAddModal,
  openEditModal,
  closeEditModal,
  clearAllFilters,
} = tienLuongSlice.actions;

export default tienLuongSlice.reducer;
