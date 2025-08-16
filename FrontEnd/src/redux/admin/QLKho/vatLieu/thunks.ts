import { createAsyncThunk } from "@reduxjs/toolkit";
import {
    getAllVatLieu,
    getVatLieuDetail,
    createVatLieu as apiCreateVatLieu,
    updateSoLuongVatLieu as apiUpdateSoLuong,
    updateNgungCungCap as apiUpdateNgungCungCap,
    updateVatLieu as apiUpdateVatLieu,
} from "../../../../api/services/admin/qlKho/qlVatLieuAPI";
import type { AddVatLieuDTO, UpdateSoLuongDTO, VatLieuDetailDTO, UpdateVatLieuDTO } from "../../../../api/index";

export const fetchAllVatLieu = createAsyncThunk<VatLieuDetailDTO[], void, { rejectValue: string }>(
    "qlKho/vatLieu/fetchAll",
    async (_, { rejectWithValue }) => {
        try {
            const res = await getAllVatLieu();
            if (!res.isSuccess || !res.data) return rejectWithValue(res.message || "Tải danh sách thất bại");
            return res.data;
        } catch (e: any) {
            return rejectWithValue(e.message || "Tải danh sách thất bại");
        }
    }
);

export const fetchVatLieuDetail = createAsyncThunk<VatLieuDetailDTO, number, { rejectValue: string }>(
    "qlKho/vatLieu/fetchDetail",
    async (maVatLieu, { rejectWithValue }) => {
        try {
            const res = await getVatLieuDetail(maVatLieu);
            if (!res.isSuccess || !res.data) return rejectWithValue(res.message || "Không tìm thấy vật liệu");
            return res.data;
        } catch (e: any) {
            return rejectWithValue(e.message || "Tải chi tiết thất bại");
        }
    }
);

export const createVatLieu = createAsyncThunk<VatLieuDetailDTO, AddVatLieuDTO, { rejectValue: string }>(
    "qlKho/vatLieu/create",
    async (payload, { rejectWithValue }) => {
        try {
            const res = await apiCreateVatLieu(payload);
            if (!res.isSuccess || !res.data) return rejectWithValue(res.message || "Tạo vật liệu thất bại");
            return res.data;
        } catch (e: any) {
            return rejectWithValue(e.message || "Tạo vật liệu thất bại");
        }
    }
);

export const updateSoLuongVatLieu = createAsyncThunk<
    { maVatLieu: number; soLuongMoi: number },
    UpdateSoLuongDTO,
    { rejectValue: string }
>("qlKho/vatLieu/updateSoLuong", async (payload, { rejectWithValue }) => {
    try {
        const res = await apiUpdateSoLuong(payload);
        if (!res.isSuccess) return rejectWithValue(res.message || "Cập nhật số lượng thất bại");
        return { maVatLieu: payload.maVatLieu, soLuongMoi: payload.soLuongMoi };
    } catch (e: any) {
        return rejectWithValue(e.message || "Cập nhật số lượng thất bại");
    }
});

export const updateNgungCungCap = createAsyncThunk<
    { maVatLieu: number; ngungCungCap: boolean; message?: string },
    { maVatLieu: number; ngungCungCap: boolean },
    { rejectValue: string }
>("qlKho/vatLieu/updateNgungCungCap", async ({ maVatLieu, ngungCungCap }, { rejectWithValue }) => {
    try {
        const res = await apiUpdateNgungCungCap(maVatLieu, ngungCungCap);
        if (!res.isSuccess) return rejectWithValue(res.message || "Cập nhật trạng thái thất bại");
        return { maVatLieu, ngungCungCap, message: res.message };
    } catch (e: any) {
        return rejectWithValue(e.message || "Cập nhật trạng thái thất bại");
    }
});

export const updateVatLieu = createAsyncThunk<VatLieuDetailDTO, UpdateVatLieuDTO, { rejectValue: string }>(
    "qlKho/vatLieu/update",
    async (payload, { rejectWithValue }) => {
        try {
            const res = await apiUpdateVatLieu(payload);
            if (!res.isSuccess || !res.data) return rejectWithValue(res.message || "Cập nhật vật liệu thất bại");
            return res.data;
        } catch (e: any) {
            return rejectWithValue(e.message || "Cập nhật vật liệu thất bại");
        }
    }
);