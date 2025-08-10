import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  getAllTaiKhoanNhanVien,
  getAllTaiKhoanKhachHang,
  addTaiKhoanNhanVien,
  getLoaiTaiKhoan,
  getEmployeesWithoutAccounts,
  lockAccount,
  unlockAccount,
  type NhanVienTaiKhoanDTO,
  type KhachHangTaiKhoanDTO,
  type AddTaiKhoanForNhanVienDTO,
  getAllAdminAccount,
  addAdminAccount,
  deleteAccount,
  updateAccount,
} from "../../../api/services/shared";
import type { ApiResponse } from "../../../api/types/apiResponse";
import type { NhanVienDTO } from "../../../api/services/shared";
import type { AddAdminAccountDTO, UpdateAccountDTO } from "../../../api";
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
export const createNhanVienAccount = createAsyncThunk(
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

export const createAdminAccount = createAsyncThunk(
  "qlHeThong/createAdminAccount",
  async (data: AddAdminAccountDTO, { rejectWithValue }) => {
    try {
      const response = await addAdminAccount(data);
      if (response.isSuccess) {
        return response.data;
      } else {
        return rejectWithValue(
          response.message || "Không thể tạo tài khoản quản lý"
        );
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi khi tạo tài khoản quản lý");
    }
  }
);

//lock account
export const lockAccountThunk = createAsyncThunk(
  "qlHeThong/lockAccount",
  async (maTaiKhoan: string, { rejectWithValue }) => {
    try {
      const response = await lockAccount(maTaiKhoan);
      if (response.isSuccess) {
        return response.data;
      } else {
        return rejectWithValue(response.message || "Không thể khóa tài khoản");
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);

export const unlockAccountThunk = createAsyncThunk(
  "qlHeThong/unlockAccount",
  async (maTaiKhoan: string, { rejectWithValue }) => {
    try {
      const response = await unlockAccount(maTaiKhoan);
      if (response.isSuccess) {
        return response.data;
      } else {
        return rejectWithValue(
          response.message || "Không thể mở khóa tài khoản"
        );
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);

export const deleteAccountThunk = createAsyncThunk(
  "qlHeThong/deleteAccount",
  async (maTaiKhoan: string, { rejectWithValue }) => {
    try {
      const response = await deleteAccount(maTaiKhoan);
      if (response.isSuccess) {
        return response.data;
      } else {
        return rejectWithValue(response.message || "Không thể xóa tài khoản");
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);

export const updateAdminAccountThunk = createAsyncThunk(
  "qlHeThong/updateAdminAccount",
  async (data: UpdateAccountDTO, { rejectWithValue }) => {
    try {
      const response = await updateAccount(data);
      if (response.isSuccess) {
        // Trả về cả request và apiData để reducer dùng
        return { request: data, apiData: response.data };
      } else {
        return rejectWithValue(
          response.message || "Không thể cập nhật tài khoản"
        );
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Lỗi hệ thống");
    }
  }
);
