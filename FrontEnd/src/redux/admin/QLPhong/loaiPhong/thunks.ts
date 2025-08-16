import { createAsyncThunk } from "@reduxjs/toolkit";
import {
    apiGetAllLoaiPhong,
    apiCreateLoaiPhong,
    apiUpdateLoaiPhong
} from "../../../../api/services/admin/qlPhong/loaiPhongAPI";
import type { AddLoaiPhongDTO, UpdateLoaiPhongDTO, LoaiPhongDTO } from "../../../../api";

export const fetchAllLoaiPhongThunk = createAsyncThunk<
    LoaiPhongDTO[],
    void,
    { rejectValue: string }
>("qlPhong/loaiPhong/fetchAll", async (_, { rejectWithValue }) => {
    try {
        const res = await apiGetAllLoaiPhong();
        if (!res.isSuccess || !res.data) return rejectWithValue(res.message || "Lấy danh sách loại phòng thất bại");
        return res.data;
    } catch (e: any) {
        return rejectWithValue(e.message || "Lấy danh sách loại phòng thất bại");
    }
});

export const createLoaiPhongThunk = createAsyncThunk<
    LoaiPhongDTO,
    AddLoaiPhongDTO,
    { rejectValue: string }
>("qlPhong/loaiPhong/create", async (payload, { rejectWithValue }) => {
    try {
        const res = await apiCreateLoaiPhong(payload);
        if (!res.isSuccess || !res.data) return rejectWithValue(res.message || "Tạo loại phòng thất bại");
        return res.data;
    } catch (e: any) {
        return rejectWithValue(e.message || "Tạo loại phòng thất bại");
    }
});

export const updateLoaiPhongThunk = createAsyncThunk<
    LoaiPhongDTO,
    UpdateLoaiPhongDTO,
    { rejectValue: string }
>("qlPhong/loaiPhong/update", async (payload, { rejectWithValue }) => {
    try {
        const res = await apiUpdateLoaiPhong(payload);
        if (!res.isSuccess || !res.data) return rejectWithValue(res.message || "Cập nhật loại phòng thất bại");
        return res.data;
    } catch (e: any) {
        return rejectWithValue(e.message || "Cập nhật loại phòng thất bại");
    }
});