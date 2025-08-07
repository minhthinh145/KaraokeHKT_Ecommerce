import { createSlice } from '@reduxjs/toolkit';
import { qlNhanSuInitialState } from './types';
import { qlNhanSuReducers } from './reducers';
import { qlNhanSuExtraReducers } from './extraReducers';

// 🏪 Create Slice cho QL Nhân Sú
const qlNhanSuSlice = createSlice({
  name: 'qlNhanSu',
  initialState: qlNhanSuInitialState,
  reducers: qlNhanSuReducers,
  extraReducers: qlNhanSuExtraReducers
});

// 🎯 Export actions với tên mới
export const {
  setSearchQueryQLNhanSu,
  setLoaiNhanVienFilter,
  setTrangThaiFilterQLNhanSu,
  clearFiltersQLNhanSu,
  setShowAddModal,
  setShowEditModal,
  setSelectedNhanVien,
  clearNhanVienErrorQLNhanSu,
  removeNhanVienFromList
} = qlNhanSuSlice.actions;

// 🎯 Export reducer
export default qlNhanSuSlice.reducer;