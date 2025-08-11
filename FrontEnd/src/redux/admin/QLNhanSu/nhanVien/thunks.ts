import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  getAllEmployees,
  createEmployeeWithAccount,
  updateEmployee,
  type NhanVienDTO,
  type AddNhanVienDTO,
  type ApiResponse,
} from "../../../../api/services/shared";

// Fetch all nhân viên
export const fetchAllNhanVien = createAsyncThunk<
  NhanVienDTO[],
  void,
  { rejectValue: string }
>("qlNhanSu/nhanVien/fetchAll", async (_, { rejectWithValue }) => {
  try {
    const res: ApiResponse<NhanVienDTO[]> = await getAllEmployees();
    if (res.isSuccess && res.data) return res.data;
    return rejectWithValue(res.message || "Lỗi khi lấy danh sách nhân viên");
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});

// Create nhân viên
export const createNhanVien = createAsyncThunk<
  NhanVienDTO,
  AddNhanVienDTO,
  { rejectValue: string }
>("qlNhanSu/nhanVien/create", async (data, { rejectWithValue }) => {
  try {
    const res: ApiResponse<NhanVienDTO> = await createEmployeeWithAccount(data);
    if (res.isSuccess && res.data) return res.data;
    return rejectWithValue(res.message || "Lỗi khi tạo nhân viên");
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});

// Update nhân viên
export const updateNhanVien = createAsyncThunk<
  NhanVienDTO,
  NhanVienDTO,
  { rejectValue: string }
>("qlNhanSu/nhanVien/update", async (data, { rejectWithValue }) => {
  try {
    const res: ApiResponse<NhanVienDTO> = await updateEmployee(data);
    if (res.isSuccess && res.data) return res.data;
    return rejectWithValue(res.message || "Lỗi khi cập nhật nhân viên");
  } catch (e: any) {
    return rejectWithValue(e.message || "Lỗi hệ thống");
  }
});
