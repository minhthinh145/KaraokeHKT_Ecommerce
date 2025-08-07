import { createSlice } from "@reduxjs/toolkit";
import { qlHeThongInitialState } from "./types";
import { qlHeThongReducers } from "./reducers";
import { qlHeThongExtraReducers } from "./extraReducers";

// 🏪 Create Slice
const qlHeThongSlice = createSlice({
  name: "qlHeThong",
  initialState: qlHeThongInitialState,
  reducers: qlHeThongReducers,
  extraReducers: qlHeThongExtraReducers,
});

// 🎯 Export actions
export const {
  setActiveTab,
  setSearchQuery,
  setLoaiTaiKhoanFilter,
  setTrangThaiFilter,
  clearFilters,
  clearNhanVienError,
  clearKhachHangError,
  clearLoaiTaiKhoanError,
  clearAdminAccountError,
} = qlHeThongSlice.actions;

// 🎯 Export reducer
export default qlHeThongSlice.reducer;
