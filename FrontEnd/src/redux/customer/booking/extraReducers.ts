import { ActionReducerMapBuilder, PayloadAction } from "@reduxjs/toolkit";
import { BookingState } from "./types";
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
    fetchAvailableRoomsPaged,
} from "./thunks";

export const addExtraReducers = (
    builder: ActionReducerMapBuilder<BookingState>
) => {
    builder
        // OLD API - Fetch rooms (trả về array) - để backward compatibility
        .addCase(fetchAvailableRooms.pending, (s) => {
            s.loading = true;
        })
        .addCase(fetchAvailableRooms.fulfilled, (s, a) => {
            s.loading = false;
            // Nếu payload là array, lưu vào data
            if (Array.isArray(a.payload)) {
                s.data = a.payload;
            } else {
                s.data = [];
            }
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
            if (Array.isArray(a.payload)) {
                s.data = a.payload;
            } else {
                s.data = [];
            }
        })
        .addCase(fetchRoomsByType.rejected, (s, a) => {
            s.loading = false;
            s.error = a.payload as string || "";
        })

        // NEW API - Fetch rooms paged (trả về PagedResult)
        .addCase(fetchAvailableRoomsPaged.pending, (state) => {
            state.availableRoomsLoading = true;
            state.availableRoomsError = null;
        })
        .addCase(fetchAvailableRoomsPaged.fulfilled, (state, action) => {
            state.availableRoomsLoading = false;
            state.availableRoomsError = null;
            // Lưu PagedResult vào rooms
            state.rooms = action.payload ?? null;
        })
        .addCase(fetchAvailableRoomsPaged.rejected, (state, action) => {
            state.availableRoomsLoading = false;
            state.availableRoomsError = action.payload as string;
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
            s.lastInvoice = a.payload;
            s.invoice = a.payload;
            s.ui.showInvoiceModal = true;
            s.ui.showBookingModal = false;
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

        // CONFIRM PAY
        .addCase(confirmPayment.pending, (s) => {
            s.confirmingPayment = true;
        })
        .addCase(confirmPayment.fulfilled, (s, a) => {
            s.confirmingPayment = false;
            s.paymentUrl = a.payload?.urlThanhToan ?? null;
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
            s.paymentUrl = a.payload?.urlThanhToan ?? null;
            s.ui.bookingRedirectUrl = a.payload?.urlThanhToan ?? null;
        })
        .addCase(rePayBooking.rejected, (s) => {
            s.rePayLoading = false;
        });
};