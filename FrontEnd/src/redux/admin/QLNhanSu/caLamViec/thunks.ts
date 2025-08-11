import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  getAllCaLamViecs,
  createCaLamViec,
} from "../../../../api/services/shared/quanLyCaLamViecAPI";
import type { CaLamViecDTO, AddCaLamViecDTO } from "../../../../api";

export const fetchAllCaLamViec = createAsyncThunk<
  CaLamViecDTO[],
  void,
  { rejectValue: string }
>("caLamViec/fetchAll", async (_, { rejectWithValue }) => {
  try {
    const res = await getAllCaLamViecs();
    if (res.isSuccess && res.data) {
      return res.data;
    } else {
      return rejectWithValue(
        res.message || "Không lấy được danh sách ca làm việc"
      );
    }
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});

export const createCaLamViecThunk = createAsyncThunk<
  CaLamViecDTO,
  AddCaLamViecDTO,
  { rejectValue: string }
>("caLamViec/create", async (payload, { rejectWithValue }) => {
  try {
    const res = await createCaLamViec(payload);
    if (res.isSuccess && res.data) {
      return res.data;
    } else {
      return rejectWithValue(res.message || "Không thể tạo ca làm việc");
    }
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});
