import { createSelector } from "@reduxjs/toolkit";
import type { RootState } from "../../store";
import type { PhongHatForCustomerDTO } from "../../../api";

export const selectBookingState = (s: RootState) => s.customer.booking;

export const selectRooms = createSelector([selectBookingState], (s) => s.data);
export const selectHistory = createSelector([selectBookingState], (s) => s.history);
export const selectBookingUI = createSelector([selectBookingState], (s) => s.ui);
export const selectBookingLoading = createSelector([selectBookingState], (s) => s.loading);
export const selectBookingCreating = createSelector([selectBookingState], (s) => s.bookingCreating);

export const selectFilteredRooms = createSelector(
    [selectBookingState],
    (s): PhongHatForCustomerDTO[] => {
        const q = s.ui.searchQuery.toLowerCase().trim();
        const f = s.ui.filterLoaiPhong;
        return s.data.filter((r: PhongHatForCustomerDTO & { maLoaiPhong?: number }) => {
            const matchQ =
                !q ||
                r.tenPhong.toLowerCase().includes(q) ||
                r.tenLoaiPhong.toLowerCase().includes(q);
            const matchType = f == null || r.maLoaiPhong === f;
            return matchQ && matchType;
        });
    }
);

// New selectors for paged data
export const selectAvailableRoomsPaged = createSelector(
    [selectBookingState],
    (bookingState) => bookingState.rooms
);

export const selectAvailableRoomsLoading = createSelector(
    [selectBookingState],
    (bookingState) => bookingState.availableRoomsLoading
);

export const selectAvailableRoomsError = createSelector(
    [selectBookingState],
    (bookingState) => bookingState.availableRoomsError
);