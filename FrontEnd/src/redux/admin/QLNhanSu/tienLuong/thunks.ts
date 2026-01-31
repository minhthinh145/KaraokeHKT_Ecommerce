import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  getAllLuongCaLamViecs,
  getLuongCaLamViecById,
  createLuongCaLamViec,
  deleteLuongCaLamViec,
  updateLuongCaLamViec,
} from "../../../../api/services/shared";
import type {
  LuongCaLamViecDTO,
  AddLuongCaLamViecDTO,
  ApiResponse,
} from "../../../../api/types";

export const fetchAllTienLuong = createAsyncThunk<
  LuongCaLamViecDTO[],
  void,
  { rejectValue: string }
>("qlNhanSu/tienLuong/fetchAll", async (_, { rejectWithValue }) => {
  try {
    const res: ApiResponse<LuongCaLamViecDTO[]> = await getAllLuongCaLamViecs();
    if (res.isSuccess && res.data) return res.data;
    return rejectWithValue(res.message || "Lỗi khi tải danh sách lương ca");
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});

export const fetchTienLuongById = createAsyncThunk<
  LuongCaLamViecDTO,
  number,
  { rejectValue: string }
>("qlNhanSu/tienLuong/fetchById", async (id, { rejectWithValue }) => {
  try {
    const res: ApiResponse<LuongCaLamViecDTO> = await getLuongCaLamViecById(id);
    if (res.isSuccess && res.data) return res.data;
    return rejectWithValue(res.message || "Không lấy được bản ghi");
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});

export const createTienLuong = createAsyncThunk<
  LuongCaLamViecDTO,
  AddLuongCaLamViecDTO,
  { rejectValue: string }
>("qlNhanSu/tienLuong/create", async (payload, { rejectWithValue }) => {
  try {
    const res: ApiResponse<LuongCaLamViecDTO> = await createLuongCaLamViec(
      payload
    );
    if (res.isSuccess && res.data) return res.data;
    return rejectWithValue(res.message || "Tạo thất bại");
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});

export const deleteTienLuong = createAsyncThunk<
  number,
  number,
  { rejectValue: string }
>("qlNhanSu/tienLuong/delete", async (id, { rejectWithValue }) => {
  try {
    const res = await deleteLuongCaLamViec(id);
    if (res.isSuccess) return id;
    return rejectWithValue(res.message || "Xóa thất bại");
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});

export const updateTienLuong = createAsyncThunk(
  "qlNhanSu/tienLuong/update",
  async (payload: LuongCaLamViecDTO, { rejectWithValue }) => {
    try {
      const res: ApiResponse<LuongCaLamViecDTO> = await updateLuongCaLamViec(
        payload
      );
      if (res.isSuccess && res.data) return res.data;
      return rejectWithValue(res.message || "Cập nhật thất bại");
    } catch (e: any) {
      return rejectWithValue(e.message || "Lỗi hệ thống");
    }
  }
);
