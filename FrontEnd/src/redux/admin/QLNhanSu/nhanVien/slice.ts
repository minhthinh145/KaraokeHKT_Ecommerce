import { createSlice } from "@reduxjs/toolkit";
import { nhanVienInitialState } from "../types";
import { nhanVienReducers } from "./reducers";
import { nhanVienExtraReducers } from "./extraReducers";

const nhanVienSlice = createSlice({
  name: "qlNhanSu/nhanVien",
  initialState: nhanVienInitialState,
  reducers: nhanVienReducers,
  extraReducers: nhanVienExtraReducers,
});

export const {
  setSearchQuery,
  setLoaiNhanVienFilter,
  setTrangThaiFilter,
  clearFilters,
  setShowAddModal,
  setShowEditModal,
  setSelectedNhanVien,
  clearNhanVienError,
  setNhanVienList,
  removeNhanVienFromList,
} = nhanVienSlice.actions;

export default nhanVienSlice.reducer;
