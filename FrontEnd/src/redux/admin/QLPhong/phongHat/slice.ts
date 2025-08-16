import { createSlice } from "@reduxjs/toolkit";
import { phongHatInitialState } from "../types";
import { phongHatReducers } from "./reducers";
import { phongHatExtraReducers } from "./extraReducers";

const phongHatSlice = createSlice({
    name: "qlPhong/phongHat",
    initialState: phongHatInitialState,
    reducers: phongHatReducers,
    extraReducers: phongHatExtraReducers,
});

export const {
    resetPhongHatState,
    setPhongHatSearchQuery,
    setPhongHatStatusFilter,
    openAddPhongHatModal,
    closeAddPhongHatModal,
    openEditPhongHatModal,
    closeEditPhongHatModal,
} = phongHatSlice.actions;

export default phongHatSlice.reducer;