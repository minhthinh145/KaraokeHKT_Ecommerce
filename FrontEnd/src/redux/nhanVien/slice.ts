import { createSlice } from "@reduxjs/toolkit";
import { nvRootInitial } from "./types";
import { nvRegisterExtra } from "./extraReducers";

const nhanVienSlice = createSlice({
    name: "nhanVien",
    initialState: nvRootInitial,
    reducers: {
        setScheduleSearch(state, action) {
            state.schedule.ui.searchQuery = action.payload;
        },
        setDraggingLich(state, action) {
            state.schedule.ui.draggingLichId = action.payload;
        },
        setShiftSearch(state, action) {
            state.shiftChanges.ui.searchQuery = action.payload;
        },
        prependShiftChange(state, action) {
            state.shiftChanges.data.unshift(action.payload);
        },
        clearShiftError(state) {
            state.shiftChanges.error = null;
        },
        clearProfileError(state) {
            state.profileError = null;
        },
    },
    extraReducers: nvRegisterExtra,
});

export const {
    setScheduleSearch,
    setDraggingLich,
    setShiftSearch,
    prependShiftChange,
    clearShiftError,
    clearProfileError,
} = nhanVienSlice.actions;

export default nhanVienSlice.reducer;