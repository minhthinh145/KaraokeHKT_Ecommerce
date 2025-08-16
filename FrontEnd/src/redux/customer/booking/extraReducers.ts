import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { BookingState } from "./types";
import {
    fetchBookingHistory,
    fetchUnpaidBookings,
    cancelBooking,
    rePayBooking,
    confirmPayment,
    fetchAvailableRooms,
    fetchRoomsByType,
    bookRoom,
    createBooking,
} from "./thunks";

export const bookingExtraReducers = (builder: ActionReducerMapBuilder<BookingState>) => {
    builder
        // ROOMS
        .addCase(fetchAvailableRooms.pending, (s) => {
            s.loading = true;
        })
        .addCase(fetchAvailableRooms.fulfilled, (s, a) => {
            s.loading = false;
            s.data = a.payload || [];
        })
        .addCase(fetchAvailableRooms.rejected, (s, a) => {
            s.loading = false;
            s.error = a.payload as string || "";
        })
        .addCase(fetchRoomsByType.pending, (s) => {
            s.loading = true;
        })
        .addCase(fetchRoomsByType.fulfilled, (s, a) => {
            s.loading = false;
            s.data = a.payload || [];
        })
        .addCase(fetchRoomsByType.rejected, (s, a) => {
            s.loading = false;
            s.error = a.payload as string || "";
        })
        // BOOK ROOM (simple)
        .addCase(bookRoom.pending, (s) => {
            s.bookingCreating = true;
        })
        .addCase(bookRoom.fulfilled, (s) => {
            s.bookingCreating = false;
        })
        .addCase(bookRoom.rejected, (s, a) => {
            s.bookingCreating = false;
            s.error = a.payload as string || "";
        })
        // INVOICE CREATE
        .addCase(createBooking.pending, (s) => {
            s.bookingCreating = true;
        })
        .addCase(createBooking.fulfilled, (s, a) => {
            s.bookingCreating = false;
            s.lastInvoice = a.payload; // lưu hóa đơn draft
            // Mở modal xem hóa đơn (không redirect)
            s.ui.showInvoiceModal = true;
        })
        .addCase(createBooking.rejected, (s, a) => {
            s.bookingCreating = false;
            s.error = a.payload as string || "";
        })
        // HISTORY
        .addCase(fetchBookingHistory.pending, (s) => {
            s.historyLoading = true;
        })
        .addCase(fetchBookingHistory.fulfilled, (s, a) => {
            s.historyLoading = false;
            s.history = a.payload || [];
        })
        .addCase(fetchBookingHistory.rejected, (s, a) => {
            s.historyLoading = false;
            s.error = a.payload || "";
        })
        // UNPAID
        .addCase(fetchUnpaidBookings.pending, (s) => {
            s.unpaidLoading = true;
        })
        .addCase(fetchUnpaidBookings.fulfilled, (s, a) => {
            s.unpaidLoading = false;
            s.unpaid = a.payload || [];
        })
        .addCase(fetchUnpaidBookings.rejected, (s, a) => {
            s.unpaidLoading = false;
            s.error = a.payload || "";
        })
        // CANCEL
        .addCase(cancelBooking.fulfilled, (s, a) => {
            const id = a.payload;
            s.history = s.history.map((x) =>
                x.maThuePhong === id ? { ...x, trangThai: "DaHuy", coTheHuy: false } : x
            );
            s.unpaid = s.unpaid.filter((x) => x.maThuePhong !== id);
        })
        // CONFIRM PAY
        .addCase(confirmPayment.pending, (s) => {
            s.confirmingPayment = true;
        })
        .addCase(confirmPayment.fulfilled, (s, a) => {
            s.confirmingPayment = false;
            s.ui.bookingRedirectUrl = a.payload?.urlThanhToan ?? null;
        })
        .addCase(confirmPayment.rejected, (s) => {
            s.confirmingPayment = false;
        })
        // REPAY
        .addCase(rePayBooking.pending, (s) => {
            s.rePayLoading = true;
        })
        .addCase(rePayBooking.fulfilled, (s, a) => {
            s.rePayLoading = false;
            s.ui.bookingRedirectUrl = a.payload?.urlThanhToan ?? null;
        })
        .addCase(rePayBooking.rejected, (s) => {
            s.rePayLoading = false;
        });
};