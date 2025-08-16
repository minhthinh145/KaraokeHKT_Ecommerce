import { createSlice } from "@reduxjs/toolkit";
import { loaiPhongInitialState } from "../types";
import { loaiPhongReducers } from "./reducers";
import { loaiPhongExtraReducers } from "./extraReducers";

const loaiPhongSlice = createSlice({
    name: "qlPhong/loaiPhong",
    initialState: loaiPhongInitialState,
    reducers: loaiPhongReducers,
    extraReducers: loaiPhongExtraReducers,
});

export const {
    resetLoaiPhongState,
    setLoaiPhongSearchQuery,
    openAddLoaiPhongModal,
    closeAddLoaiPhongModal,
    openEditLoaiPhongModal,
    closeEditLoaiPhongModal,
} = loaiPhongSlice.actions;

export default loaiPhongSlice.reducer;