import { createAsyncThunk } from "@reduxjs/toolkit";
import {
    apiGetAllPhongHat,
    apiCreatePhongHat,
    apiUpdatePhongHat,
    apiToggleNgungHoatDong,
    apiToggleDangSuDung,
} from "../../../../api/services/admin/qlPhong/phongHatAPI";
import type {
    AddPhongHatDTO,
    UpdatePhongHatDTO,
    PhongHatDetailDTO,
} from "../../QLPhong/types";

const reject = (message?: string) => message || "Thao tác thất bại";

export const fetchAllPhongHatThunk = createAsyncThunk<
    PhongHatDetailDTO[],
    void,
    { rejectValue: string }
>("qlPhong/phongHat/fetchAll", async (_, { rejectWithValue }) => {
    try {
        const res = await apiGetAllPhongHat();
        if (!res.isSuccess || !res.data) return rejectWithValue(res.message);
        return res.data;
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

export const createPhongHatThunk = createAsyncThunk<
    PhongHatDetailDTO,
    AddPhongHatDTO,
    { rejectValue: string }
>("qlPhong/phongHat/create", async (payload, { rejectWithValue }) => {
    try {
        const res = await apiCreatePhongHat(payload);
        if (!res.isSuccess || !res.data) return rejectWithValue(res.message);
        return res.data;
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

export const updatePhongHatThunk = createAsyncThunk<
    PhongHatDetailDTO,
    UpdatePhongHatDTO,
    { rejectValue: string }
>("qlPhong/phongHat/update", async (payload, { rejectWithValue }) => {
    try {
        const res = await apiUpdatePhongHat(payload);
        if (!res.isSuccess || !res.data) return rejectWithValue(res.message);
        return res.data;
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

export const toggleNgungHoatDongThunk = createAsyncThunk<
    { maPhong: number; ngungHoatDong: boolean },
    { maPhong: number; ngungHoatDong: boolean },
    { rejectValue: string }
>("qlPhong/phongHat/toggleNgungHoatDong", async (p, { rejectWithValue }) => {
    try {
        const res = await apiToggleNgungHoatDong(p.maPhong, p.ngungHoatDong);
        if (!res.isSuccess) return rejectWithValue(res.message);
        return p;
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});

export const toggleDangSuDungThunk = createAsyncThunk<
    { maPhong: number; dangSuDung: boolean },
    { maPhong: number; dangSuDung: boolean },
    { rejectValue: string }
>("qlPhong/phongHat/toggleDangSuDung", async (p, { rejectWithValue }) => {
    try {
        const res = await apiToggleDangSuDung(p.maPhong, p.dangSuDung);
        if (!res.isSuccess) return rejectWithValue(res.message);
        return p;
    } catch (e: any) {
        return rejectWithValue(e.message);
    }
});