import { createSlice } from "@reduxjs/toolkit";
import { vatLieuInitialState } from "../types";
import { vatLieuReducers } from "./reducers";
import { registerVatLieuExtraReducers } from "./extraReducers";

export const vatLieuSlice = createSlice({
    name: "qlKho/vatLieu",
    initialState: vatLieuInitialState,
    reducers: vatLieuReducers,
    extraReducers: (builder) => {
        registerVatLieuExtraReducers(builder);
    },
});

export const {
    setVatLieuSearchQuery,
    setVatLieuFilters,
    setVatLieuSelected,
    setVatLieuShowCreateModal,
    setVatLieuShowUpdateSoLuongModal,
    setVatLieuShowEditModal, // thêm
} = vatLieuSlice.actions;

export const vatLieuReducer = vatLieuSlice.reducer;