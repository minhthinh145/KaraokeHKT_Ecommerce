import { createSlice } from "@reduxjs/toolkit";
import { initialBookingState } from "./types";
import { bookingReducers } from "./reducers";
import { bookingExtraReducers } from "./extraReducers";

export const bookingSlice = createSlice({
    name: "booking",
    initialState: initialBookingState,
    reducers: bookingReducers,
    extraReducers: bookingExtraReducers,
});

export const {
    setSearchQuery,
    openBooking,
    closeBooking,
    setFilterLoaiPhong,
    setDurationHours,
    setNote,
    openInvoice,
    closeInvoice,
} = bookingSlice.actions;

export default bookingSlice.reducer;