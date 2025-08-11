import { createSlice } from "@reduxjs/toolkit";
import { caLamViecInitialState } from "../types";
import { caLamViecExtraReducers } from "./extraReducer";
import { caLamViecReducers } from "./reducers";

const caLamViecSlice = createSlice({
  name: "qlNhanSu/caLamViec",
  initialState: caLamViecInitialState,
  reducers: caLamViecReducers,
  extraReducers: caLamViecExtraReducers,
});

export const {
  clearCaLamViecError,
  clearCaLamViecCurrent,
  resetCaLamViecState,
  setCurrentCaLamViec,
} = caLamViecSlice.actions;
export default caLamViecSlice.reducer;
