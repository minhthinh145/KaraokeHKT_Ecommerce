import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  getAllEmployees,
  createEmployeeWithAccount,
  updateEmployee,
  type NhanVienDTO,
  type AddNhanVienDTO,
  type ApiResponse,
} from "../../../api/services/shared";

// 🔥 Fetch All Nhân viên - Renamed with QLNhanSu prefix
export const fetchAllNhanVienQLNhanSu = createAsyncThunk(
  "qlNhanSu/fetchAllNhanVien",
  async (_, { rejectWithValue }) => {
    try {
      const response: ApiResponse<NhanVienDTO[]> = await getAllEmployees();

      if (response.isSuccess && response.data) {
        return response.data;
      } else {
        return rejectWithValue(
          response.message || "Lỗi khi lấy danh sách nhân viên"
        );
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);

// 🔥 Create Nhân viên - Renamed
export const createNhanVienQLNhanSu = createAsyncThunk(
  "qlNhanSu/createNhanVien",
  async (data: AddNhanVienDTO, { rejectWithValue }) => {
    try {
      const response: ApiResponse<NhanVienDTO> =
        await createEmployeeWithAccount(data);

      if (response.isSuccess && response.data) {
        return response.data;
      } else {
        return rejectWithValue(response.message || "Lỗi khi tạo nhân viên");
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);

// 🔥 Update Nhân viên - Renamed
export const updateNhanVienQLNhanSu = createAsyncThunk(
  "qlNhanSu/updateNhanVien",
  async (data: NhanVienDTO, { rejectWithValue }) => {
    try {
      const response: ApiResponse<NhanVienDTO> = await updateEmployee(data);

      if (response.isSuccess && response.data) {
        return response.data;
      } else {
        return rejectWithValue(
          response.message || "Lỗi khi cập nhật nhân viên"
        );
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);
