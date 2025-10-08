import { createAsyncThunk } from "@reduxjs/toolkit";
import {
    getAvailableRooms,
    getRoomsByType,
    postBookRoom,
    getBookingHistory,
    cancelBookingApi,
    createBookingInvoice,
    confirmPaymentApi,
    type DatPhongDTO,
    type TaoHoaDonPhongResponseDTO,
    type ConfirmPaymentResponse,
    type XacNhanThanhToanDTO,
    getUnpaidBookings,
    rePayBookingApi,
    type RePayResponse,
    mapHistoryDto,
} from "../../../api/customer/bookingApi";
import type { LichSuDatPhongDTO } from "../../../api";

export const fetchAvailableRooms = createAsyncThunk(
    "booking/fetchAvailableRooms",
    async (_, { rejectWithValue }) => {
        try {
            const res = await getAvailableRooms();
            if (res.isSuccess) return res.data ?? [];
            return rejectWithValue(res.message);
        } catch (e: any) {
            return rejectWithValue(e.message);
        }
    }
);

export const fetchRoomsByType = createAsyncThunk(
    "booking/fetchRoomsByType",
    async (maLoaiPhong: number, { rejectWithValue }) => {
        try {
            const res = await getRoomsByType(maLoaiPhong);
            if (res.isSuccess) return res.data ?? [];
            return rejectWithValue(res.message);
        } catch (e: any) {
            return rejectWithValue(e.message);
        }
    }
);

export const fetchBookingHistory = createAsyncThunk<
    LichSuDatPhongDTO[],
    void,
    { rejectValue: string }
>("booking/fetchHistory", async (_, { rejectWithValue }) => {
    try {
        const res = await getBookingHistory();
        if (res.isSuccess) {
            return (res.data ?? []).map(mapHistoryDto);
        }
        return rejectWithValue(res.message);
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

export const bookRoom = createAsyncThunk(
    "booking/bookRoom",
    async (payload: DatPhongDTO, { rejectWithValue }) => {
        try {
            const res = await postBookRoom(payload);
            if (res.isSuccess) return res.data;
            return rejectWithValue(res.message);
        } catch (e: any) {
            return rejectWithValue(e.message);
        }
    }
);

export const cancelBooking = createAsyncThunk(
    "booking/cancelBooking",
    async (maThuePhong: string, { rejectWithValue }) => {
        try {
            const res = await cancelBookingApi(maThuePhong);
            if (res.isSuccess) return maThuePhong;
            return rejectWithValue(res.message);
        } catch (e: any) {
            return rejectWithValue(e.message);
        }
    }
);

export const createBooking = createAsyncThunk<
    TaoHoaDonPhongResponseDTO | null,
    DatPhongDTO,
    { rejectValue: string }
>("booking/createBooking", async (payload, { rejectWithValue }) => {
    try {
        const res = await createBookingInvoice(payload);
        if (res.isSuccess) return res.data ?? null;
        return rejectWithValue(res.message);
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

export const confirmPayment = createAsyncThunk<
    ConfirmPaymentResponse | null,
    XacNhanThanhToanDTO,
    { rejectValue: string }
>("booking/confirmPayment", async (payload, { rejectWithValue }) => {
    try {
        const res = await confirmPaymentApi(payload);
        if (res.isSuccess) return res.data ?? null;
        return rejectWithValue(res.message);
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

export const fetchUnpaidBookings = createAsyncThunk<
    LichSuDatPhongDTO[],
    void,
    { rejectValue: string }
>("booking/fetchUnpaid", async (_, { rejectWithValue }) => {
    try {
        const res = await getUnpaidBookings();
        if (res.isSuccess) {
            return (res.data ?? []).map(mapHistoryDto);
        }
        return rejectWithValue(res.message);
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

export const rePayBooking = createAsyncThunk<
    RePayResponse | null,
    string,
    { rejectValue: string }
>("booking/rePay", async (maThuePhong, { rejectWithValue }) => {
    try {
        const res = await rePayBookingApi(maThuePhong);
        if (res.isSuccess) return res.data ?? null;
        return rejectWithValue(res.message);
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

export const fetchAvailableRoomsPaged = createAsyncThunk(
    "customerBooking/fetchAvailableRoomsPaged",
    async (_: void, { getState, rejectWithValue }) => {
        try {
            const state: any = getState();
            const {
                availableRoomsPage: pageNumber,
                availableRoomsPageSize: pageSize,
            } = state.customerBooking;
            const res = await getAvailableRooms(pageNumber, pageSize);
            if (!res.isSuccess) return rejectWithValue(res.message || "Lỗi");
            return res.data;
        } catch (e: any) {
            return rejectWithValue(e.message || "Lỗi không xác định");
        }
    }
);