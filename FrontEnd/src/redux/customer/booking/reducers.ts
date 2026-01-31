import type { PayloadAction } from "@reduxjs/toolkit";
import type { BookingState } from "./types";
import type { PhongHatForCustomerDTO } from "../../../api";

export const bookingReducers = {
    setSearchQuery(state: BookingState, { payload }: PayloadAction<string>) {
        state.ui.searchQuery = payload;
    },
    openBooking(state: BookingState, { payload }: PayloadAction<PhongHatForCustomerDTO>) {
        state.ui.selectedRoom = payload;
        state.ui.showBookingModal = true;
        state.ui.note = "";
    },
    closeBooking(state: BookingState) {
        state.ui.showBookingModal = false;
        state.ui.selectedRoom = null;
        state.ui.note = "";
    },
    setFilterLoaiPhong(state: BookingState, { payload }: PayloadAction<number | null>) {
        state.ui.filterLoaiPhong = payload;
    },
    setDurationHours(state: BookingState, { payload }: PayloadAction<number>) {
        state.ui.durationHours = payload;
    },
    setNote(state: BookingState, { payload }: PayloadAction<string>) {
        state.ui.note = payload;
    },
    openInvoice(state: BookingState) {
        state.ui.showInvoiceModal = true;
    },
    closeInvoice(state: BookingState) {
        state.ui.showInvoiceModal = false;
        state.lastInvoice = null;
        //state.paymentResult = null;
    },
    resetRedirect(state: BookingState) {
        state.ui.bookingRedirectUrl = null;
    },
};