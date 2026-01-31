import { createSlice } from "@reduxjs/toolkit";
import { initialBookingState } from "./types";
import { bookingReducers } from "./reducers";
import { fetchAvailableRoomsPaged } from "./thunks";

export const bookingSlice = createSlice({
    name: "booking",
    initialState: {
        ...initialBookingState,
        availableRoomsPaged: null as any,          // PagedResult<PhongHatForCustomerDTO> | null
        availableRoomsLoading: false,
        availableRoomsError: null as string | null,
        availableRoomsPage: 1,
        availableRoomsPageSize: 12,
    },
    reducers: {
        ...bookingReducers,
        setAvailableRoomsPage(state, action) {
            state.availableRoomsPage = action.payload;
        },
        setAvailableRoomsPageSize(state, action) {
            state.availableRoomsPageSize = action.payload;
            state.availableRoomsPage = 1;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchAvailableRoomsPaged.pending, (state) => {
                state.availableRoomsLoading = true;
                state.availableRoomsError = null;
            })
            .addCase(fetchAvailableRoomsPaged.fulfilled, (state, action) => {
                state.availableRoomsLoading = false;
                state.availableRoomsPaged = action.payload;
            })
            .addCase(fetchAvailableRoomsPaged.rejected, (state, action) => {
                state.availableRoomsLoading = false;
                state.availableRoomsError =
                    (action.payload as string) || "Tải danh sách phòng thất bại";
            });
    },
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
    setAvailableRoomsPage,
    setAvailableRoomsPageSize,
} = bookingSlice.actions;

export default bookingSlice.reducer;