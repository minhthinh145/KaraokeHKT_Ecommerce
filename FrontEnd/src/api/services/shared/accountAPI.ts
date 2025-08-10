import axiosInstance from "../../axiosConfig";
import type { ApiResponse } from "../../types/apiResponse";
import type {
  NhanVienTaiKhoanDTO,
  KhachHangTaiKhoanDTO,
  AddTaiKhoanForNhanVienDTO,
  UpdateAccountDTO,
} from "../../types/admins/QLHeThongtypes";

// ✅ GET /api/QLHeThong/taikhoan/nhanvien
export const getAllTaiKhoanNhanVien = async (): Promise<
  ApiResponse<NhanVienTaiKhoanDTO[]>
> => {
  try {
    const response = await axiosInstance.get("QLHeThong/taikhoan/nhanvien");
    return response.data as ApiResponse<NhanVienTaiKhoanDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Lỗi khi lấy tài khoản nhân viên"
    );
  }
};

// ✅ GET /api/QLHeThong/taikhoan/khachhang
export const getAllTaiKhoanKhachHang = async (): Promise<
  ApiResponse<KhachHangTaiKhoanDTO[]>
> => {
  try {
    const response = await axiosInstance.get("QLHeThong/taikhoan/khachhang");
    return response.data as ApiResponse<KhachHangTaiKhoanDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Lỗi khi lấy tài khoản khách hàng"
    );
  }
};

// ✅ POST /api/QLHeThong/taikhoan/nhanvien/gan-tai-khoan
export const addTaiKhoanNhanVien = async (
  data: AddTaiKhoanForNhanVienDTO
): Promise<ApiResponse<any>> => {
  try {
    const response = await axiosInstance.post(
      "QLHeThong/taikhoan/nhanvien/gan-tai-khoan",
      data
    );
    return response.data as ApiResponse<any>;
  } catch (error: any) {
    throw new Error(error.response?.data?.message || "Lỗi khi tạo tài khoản");
  }
};

// ✅ GET /api/QLHeThong/loai-tai-khoan
export const getLoaiTaiKhoan = async (): Promise<ApiResponse<string[]>> => {
  try {
    const response = await axiosInstance.get("QLHeThong/loai-tai-khoan");
    return response.data as ApiResponse<string[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Lỗi khi lấy loại tài khoản"
    );
  }
};

// ✅ PUT /api/QLHeThong/taikhoan/{maTaiKhoan}/lock
export const lockAccount = async (
  maTaiKhoan: string
): Promise<ApiResponse<any>> => {
  try {
    const response = await axiosInstance.put(
      `QLHeThong/taikhoan/${maTaiKhoan}/lock`
    );
    return response.data as ApiResponse<any>;
  } catch (error: any) {
    throw new Error(error.response?.data?.message || "Lỗi khi khóa tài khoản");
  }
};

// ✅ PUT /api/QLHeThong/taikhoan/{maTaiKhoan}/unlock
export const unlockAccount = async (
  maTaiKhoan: string
): Promise<ApiResponse<any>> => {
  try {
    const response = await axiosInstance.put(
      `QLHeThong/taikhoan/${maTaiKhoan}/unlock`
    );
    return response.data as ApiResponse<any>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Lỗi khi mở khóa tài khoản"
    );
  }
};

//Delete /api/QLHeThong/Taikhoan/{maTaiKhoan}/delete
export const deleteAccount = async (
  maTaiKhoan: string
): Promise<ApiResponse<any>> => {
  try {
    const response = await axiosInstance.delete(
      `QLHeThong/taikhoan/${maTaiKhoan}/delete`
    );
    return response.data as ApiResponse<any>;
  } catch (error: any) {
    throw new Error(error.response?.data?.message || "Lỗi khi xóa tài khoản");
  }
};

//update /api/QLHeThong/TaiKhoan/update
export const updateAccount = async (
  data: UpdateAccountDTO
): Promise<ApiResponse<any>> => {
  try {
    const response = await axiosInstance.put(`QLHeThong/taikhoan/update`, data);
    return response.data as ApiResponse<any>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Lỗi khi cập nhật tài khoản"
    );
  }
};
