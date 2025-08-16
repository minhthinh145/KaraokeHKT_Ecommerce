// Đảm bảo dùng apiGetScheduleByNhanVien từ employeeAPI (đã chuẩn hoá)
import { createAsyncThunk } from "@reduxjs/toolkit";
import type {
    LichLamViecDTO,
    YeuCauChuyenCaDTO,
    NhanVienTaiKhoanDTO,
} from "../../api";
import type { CreateShiftChangePayload } from "./types";
import {
    apiGetScheduleByNhanVien,
    apiGetMyShiftChangeRequests,
    apiCreateShiftChange,
    apiDeleteShiftChange,
} from "../../api/services/employee/employeeAPI"; // chỉnh path đúng
import { apiGetMyProfile } from "../../api/services/employee/employeeAPI";

// Profile
export const fetchNhanVienProfile = createAsyncThunk<
    NhanVienTaiKhoanDTO,
    void,
    { rejectValue: string }
>("nhanVien/profile/fetch", async (_, { rejectWithValue }) => {
    try {
        const res = await apiGetMyProfile();
        if (!res.isSuccess || !res.data)
            return rejectWithValue(res.message || "Không lấy được profile");
        return res.data;
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

// Schedule
export const fetchScheduleNV = createAsyncThunk<
    LichLamViecDTO[],
    { maNhanVien: string },
    { rejectValue: string }
>("nhanVien/schedule/fetch", async (p, { rejectWithValue }) => {
    try {
        const res = await apiGetScheduleByNhanVien(p.maNhanVien);
        if (!res.isSuccess) return rejectWithValue(res.message || "Lỗi lấy lịch");
        return res.data || [];
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

// Shift changes list
export const fetchShiftChanges = createAsyncThunk<
    YeuCauChuyenCaDTO[],
    { maNhanVien: string },
    { rejectValue: string }
>("nhanVien/shiftChanges/fetch", async (p, { rejectWithValue }) => {
    try {
        const res = await apiGetMyShiftChangeRequests(p.maNhanVien);
        if (!res.isSuccess)
            return rejectWithValue(res.message || "Lỗi lấy yêu cầu");
        return res.data || [];
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

// Create shift change
export const createShiftChange = createAsyncThunk<
    YeuCauChuyenCaDTO,
    CreateShiftChangePayload,
    { rejectValue: string }
>("nhanVien/shiftChanges/create", async (payload, { rejectWithValue }) => {
    try {
        const res = await apiCreateShiftChange(payload);
        if (!res.isSuccess || !res.data)
            return rejectWithValue(res.message || "Tạo thất bại");
        return res.data;
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

// Delete shift change
export const deleteShiftChange = createAsyncThunk<
    number,
    { maYeuCau: number },
    { rejectValue: string }
>("nhanVien/shiftChanges/delete", async ({ maYeuCau }, { rejectWithValue }) => {
    try {
        const res = await apiDeleteShiftChange(maYeuCau);
        if (!res.isSuccess)
            return rejectWithValue(res.message || "Xóa thất bại");
        return maYeuCau;
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});