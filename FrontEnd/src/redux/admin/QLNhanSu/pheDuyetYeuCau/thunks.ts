import { createAsyncThunk } from "@reduxjs/toolkit";
import {
    getAllYeuCauChuyenCaAdmin,
    getPendingYeuCauChuyenCaAdmin,
    getApprovedYeuCauChuyenCaAdmin,
    getYeuCauChuyenCaByIdAdmin,
    approveYeuCauChuyenCaAdmin,
} from "../../../../api/services/admin/qlNhanSu/quanLyLichLamViecAPI";
import type { YeuCauChuyenCaDTO } from "../../../../api";

/**
 * Note: approve thunk accepts { maYeuCau, ghiChu? } (friendly param)
 * and maps to API DTO (ghiChuPheDuyet, ketQuaPheDuyet).
 */

export const fetchAllYeuCauChuyenCa = createAsyncThunk<
    YeuCauChuyenCaDTO[],
    void,
    { rejectValue: string }
>("pheDuyetYeuCau/fetchAll", async (_, { rejectWithValue }) => {
    try {
        const res = await getAllYeuCauChuyenCaAdmin();
        if (res?.isSuccess && res.data) return res.data;
        return rejectWithValue(res?.message || "Không thể lấy danh sách yêu cầu");
    } catch (e: any) {
        return rejectWithValue(e?.message || "Lỗi hệ thống");
    }
});

export const fetchPendingYeuCauChuyenCa = createAsyncThunk<
    YeuCauChuyenCaDTO[],
    void,
    { rejectValue: string }
>("pheDuyetYeuCau/fetchPending", async (_, { rejectWithValue }) => {
    try {
        const res = await getPendingYeuCauChuyenCaAdmin();
        if (res?.isSuccess && res.data) return res.data;
        return rejectWithValue(res?.message || "Không thể lấy yêu cầu chờ duyệt");
    } catch (e: any) {
        return rejectWithValue(e?.message || "Lỗi hệ thống");
    }
});

export const fetchApprovedYeuCauChuyenCa = createAsyncThunk<
    YeuCauChuyenCaDTO[],
    void,
    { rejectValue: string }
>("pheDuyetYeuCau/fetchApproved", async (_, { rejectWithValue }) => {
    try {
        const res = await getApprovedYeuCauChuyenCaAdmin();
        if (res?.isSuccess && res.data) return res.data;
        return rejectWithValue(res?.message || "Không thể lấy yêu cầu đã duyệt");
    } catch (e: any) {
        return rejectWithValue(e?.message || "Lỗi hệ thống");
    }
});

export const fetchYeuCauChuyenCaDetail = createAsyncThunk<
    YeuCauChuyenCaDTO,
    number,
    { rejectValue: string }
>("pheDuyetYeuCau/fetchDetail", async (id, { rejectWithValue }) => {
    try {
        const res = await getYeuCauChuyenCaByIdAdmin(id);
        if (res?.isSuccess && res.data) return res.data;
        return rejectWithValue(res?.message || "Không tìm thấy yêu cầu");
    } catch (e: any) {
        return rejectWithValue(e?.message || "Lỗi hệ thống");
    }
});

/**
 * approve: accept friendly payload { maYeuCau, ghiChu? }
 * map to API DTO { maYeuCau, ketQuaPheDuyet: true/false, ghiChuPheDuyet }
 */
export const approveYeuCauChuyenCa = createAsyncThunk<
    { isSuccess: boolean; message: string; data: any | null },
    { maYeuCau: number; ghiChu?: string; ketQua?: boolean },
    { rejectValue: string }
>("pheDuyetYeuCau/approve", async (payload, { rejectWithValue }) => {
    try {
        const dto = {
            maYeuCau: payload.maYeuCau,
            ketQuaPheDuyet: payload.ketQua ?? true,
            ghiChuPheDuyet: payload.ghiChu ?? "",
        };
        const res = await approveYeuCauChuyenCaAdmin(dto);
        if (res?.isSuccess) {
            return { isSuccess: true, message: res.message, data: res.data ?? null };
        }
        return rejectWithValue(res?.message || "Không thể phê duyệt yêu cầu");
    } catch (e: any) {
        return rejectWithValue(e?.message || "Lỗi hệ thống");
    }
});