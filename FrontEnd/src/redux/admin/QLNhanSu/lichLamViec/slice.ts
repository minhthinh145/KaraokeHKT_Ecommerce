import { createSlice } from "@reduxjs/toolkit";
import { lichLamViecInitialState } from "../types";
import { lichLamViecReducers } from "./reducers";
import { lichLamViecExtraReducers } from "./extraReducers";

const lichLamViecSlice = createSlice({
  name: "qlNhanSu/lichLamViec",
  initialState: lichLamViecInitialState,
  reducers: lichLamViecReducers,
  extraReducers: lichLamViecExtraReducers,
});

export const {
  setCurrentLichLamViec,
  clearLichLamViecError,
  resetLichLamViecState,
  setSearchQuery,
  setSelectedNhanVien,
  setDateRange,
  clearAllFilters,
  setShowEditModal,
  openEditModal,
  closeEditModal,
  setSendingNoti,
  selectLichLamViecSendingNoti
} = lichLamViecSlice.actions;

export default lichLamViecSlice.reducer;
