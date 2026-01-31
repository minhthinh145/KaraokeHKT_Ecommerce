import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  getAllLichLamViec,
  getLichLamViecByNhanVien,
  createLichLamViec,
  getLichLamViecByRange,
  deleteLichLamViec,
  updateLichLamViec,
  sendNotiWorkSchedules,
} from "../../../../api/services/admin";
import type { LichLamViecDTO, AddLichLamViecDTO } from "../../../../api/types";
import type { ApiResponse } from "../../../../api/types";

export const fetchAllLichLamViec = createAsyncThunk<
  LichLamViecDTO[],
  void,
  { rejectValue: string }
>("qlNhanSu/lichLamViec/fetchAll", async (_, { rejectWithValue }) => {
  try {
    const res: ApiResponse<LichLamViecDTO[]> = await getAllLichLamViec();
    if (res.isSuccess && res.data) return res.data;
    return rejectWithValue(
      res.message || "Lỗi khi tải danh sách lịch làm việc"
    );
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});

export const fetchLichLamViecByNhanVienThunk = createAsyncThunk<
  LichLamViecDTO[],
  string,
  { rejectValue: string }
>(
  "qlNhanSu/lichLamViec/fetchByNhanVien",
  async (maNhanVien, { rejectWithValue }) => {
    try {
      const res: ApiResponse<LichLamViecDTO[]> = await getLichLamViecByNhanVien(
        maNhanVien
      );
      if (res.isSuccess && res.data) return res.data;
      return rejectWithValue(
        res.message || "Không lấy được lịch làm việc theo nhân viên"
      );
    } catch (e: any) {
      return rejectWithValue(e.message || "Lỗi hệ thống");
    }
  }
);

export const fetchLichLamViecByRangeThunk = createAsyncThunk<
  LichLamViecDTO[],
  { start: string; end: string },
  { rejectValue: string }
>(
  "qlNhanSu/lichLamViec/fetchByRange",
  async ({ start, end }, { rejectWithValue }) => {
    try {
      const res: ApiResponse<LichLamViecDTO[]> = await getLichLamViecByRange(
        start,
        end
      );
      if (res.isSuccess && res.data) return res.data;
      return rejectWithValue(
        res.message || "Không lấy được lịch làm việc theo khoảng ngày"
      );
    } catch (e: any) {
      return rejectWithValue(e.message || "Lỗi hệ thống");
    }
  }
);

export const createLichLamViecThunk = createAsyncThunk<
  LichLamViecDTO,
  AddLichLamViecDTO,
  { rejectValue: string }
>("qlNhanSu/lichLamViec/create", async (payload, { rejectWithValue }) => {
  try {
    const res: ApiResponse<LichLamViecDTO> = await createLichLamViec(payload);
    if (res.isSuccess && res.data) return res.data;
    return rejectWithValue(res.message || "Tạo lịch làm việc thất bại");
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});

export const updateLichLamViecThunk = createAsyncThunk<
  LichLamViecDTO,
  LichLamViecDTO,
  { rejectValue: string }
>("qlNhanSu/lichLamViec/update", async (payload, { rejectWithValue }) => {
  try {
    const res: ApiResponse<LichLamViecDTO> = await updateLichLamViec(payload);
    if (res.isSuccess && res.data) return res.data;
    // Nếu API không trả về data, có thể return payload hoặc báo lỗi nhẹ:
    if (res.isSuccess) return payload as LichLamViecDTO;
    return rejectWithValue(res.message || "Cập nhật lịch làm việc thất bại");
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});

export const deleteLichLamViecThunk = createAsyncThunk<
  number, // trả về id đã xóa
  number, // nhận vào id
  { rejectValue: string }
>("qlNhanSu/lichLamViec/delete", async (ma, { rejectWithValue }) => {
  try {
    const res: ApiResponse<null> = await deleteLichLamViec(ma);
    if (res.isSuccess) return ma;
    return rejectWithValue(res.message || "Xóa lịch làm việc thất bại");
  } catch (e: any) {
    return rejectWithValue(e.message || "Xóa lịch làm việc thất bại");
  }
});


export const sendNotiWorkSchedulesThunk = createAsyncThunk<
  void,
  { start: string; end: string },
  { rejectValue: string }
>("qlNhanSu/lichLamViec/sendNotiWorkSchedules", async ({ start, end }, { rejectWithValue }) => {
  try {
    const res: ApiResponse<null> = await sendNotiWorkSchedules(start, end);
    if (res.isSuccess) return;
    return rejectWithValue(res.message || "Gửi thông báo lịch làm việc thất bại");
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống khi gửi thông báo");
  }
});