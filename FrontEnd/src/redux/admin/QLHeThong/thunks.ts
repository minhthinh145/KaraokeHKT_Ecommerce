import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  getAllTaiKhoanNhanVien,
  getAllTaiKhoanKhachHang,
  addTaiKhoanNhanVien,
  getLoaiTaiKhoan, // ✅ API này có trong backend
  getEmployeesWithoutAccounts,
  type NhanVienTaiKhoanDTO,
  type KhachHangTaiKhoanDTO,
  type AddTaiKhoanForNhanVienDTO,
  getAllAdminAccount, // ✅ Fix type name
} from "../../../api/services/shared";
import type { ApiResponse } from "../../../api/types/apiResponse";
import type { NhanVienDTO } from "../../../api/services/shared";
// 🔥 Fetch All Tài khoản Nhân viên
export const fetchAllNhanVien = createAsyncThunk(
  "qlHeThong/fetchAllNhanVien",
  async (_, { rejectWithValue }) => {
    try {
      const response: ApiResponse<NhanVienTaiKhoanDTO[]> =
        await getAllTaiKhoanNhanVien();

      if (response.isSuccess && response.data) {
        return response.data;
      } else {
        return rejectWithValue(
          response.message || "Lỗi khi lấy danh sách tài khoản nhân viên"
        );
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);

// 🔥 Fetch All Tài khoản Khách hàng
export const fetchAllKhachHang = createAsyncThunk(
  "qlHeThong/fetchAllKhachHang",
  async (_, { rejectWithValue }) => {
    try {
      const response: ApiResponse<KhachHangTaiKhoanDTO[]> =
        await getAllTaiKhoanKhachHang();

      if (response.isSuccess && response.data) {
        return response.data;
      } else {
        return rejectWithValue(
          response.message || "Lỗi khi lấy danh sách tài khoản khách hàng"
        );
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);

// ✅ Fetch Loại tài khoản - Backend CÓ endpoint này
export const fetchLoaiTaiKhoan = createAsyncThunk(
  "qlHeThong/fetchLoaiTaiKhoan",
  async (_, { rejectWithValue }) => {
    try {
      const response: ApiResponse<string[]> = await getLoaiTaiKhoan();

      if (response.isSuccess && response.data) {
        return response.data;
      } else {
        return rejectWithValue(
          response.message || "Lỗi khi lấy danh sách loại tài khoản"
        );
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);

// 🔥 Fetch Nhân viên chưa có tài khoản - Gọi endpoint nhanvienAll
export const fetchNhanVienChuaCoTaiKhoan = createAsyncThunk(
  "qlHeThong/fetchNhanVienChuaCoTaiKhoan",
  async (_, { rejectWithValue }) => {
    try {
      const response: ApiResponse<NhanVienDTO[]> =
        await getEmployeesWithoutAccounts();

      if (response.isSuccess && response.data) {
        return response.data;
      } else {
        return rejectWithValue(
          response.message ||
            "Lỗi khi lấy danh sách nhân viên chưa có tài khoản"
        );
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);

// 🔥 Create Tài khoản cho Nhân viên - Gọi endpoint gan-tai-khoan
export const createNhanVien = createAsyncThunk(
  "qlHeThong/createNhanVien",
  async (data: AddTaiKhoanForNhanVienDTO, { rejectWithValue }) => {
    try {
      const response: ApiResponse<any> = await addTaiKhoanNhanVien(data);

      if (response.isSuccess) {
        return response.data;
      } else {
        return rejectWithValue(response.message || "Lỗi khi tạo tài khoản");
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);

//Quản lý
export const fetchAllAdminAccount = createAsyncThunk(
  "qlHeThong/fetchAllAdminAccount",
  async (_, { rejectWithValue }) => {
    try {
      const response = await getAllAdminAccount();
      if (response.isSuccess && response.data) {
        return response.data;
      } else {
        return rejectWithValue(
          response.message || "Không thể tải danh sách quản lý"
        );
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi khi tải danh sách quản lý");
    }
  }
);
