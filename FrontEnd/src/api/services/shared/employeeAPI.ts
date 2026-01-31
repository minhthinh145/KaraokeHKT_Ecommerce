import axiosInstance from "../../axiosConfig";
import type { ApiResponse } from "../../types/apiResponse";
import type { AddNhanVienDTO } from "../../types/admins/QLHeThongtypes";
import type { NhanVienDTO } from "../../types/admins/QLNhanSutypes";

// ✅ GET /api/QLHeThong/nhanvienAll - Lấy nhân viên chưa có tài khoản
export const getEmployeesWithoutAccounts = async (): Promise<
  ApiResponse<NhanVienDTO[]>
> => {
  try {
    const response = await axiosInstance.get("QLHeThong/nhanvienAll");
    return response.data as ApiResponse<NhanVienDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Lỗi khi lấy nhân viên chưa có tài khoản"
    );
  }
};

// ✅ GET /api/QLNhanSu/nhanvien - Lấy tất cả nhân viên
export const getAllEmployees = async (): Promise<
  ApiResponse<NhanVienDTO[]>
> => {
  try {
    const response = await axiosInstance.get("QLNhanSu/nhanvien");
    return response.data as ApiResponse<NhanVienDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Lỗi khi lấy danh sách nhân viên"
    );
  }
};

// ✅ POST /api/QLNhanSu/nhanvien - Tạo nhân viên và tài khoản
export const createEmployeeWithAccount = async (
  data: AddNhanVienDTO
): Promise<ApiResponse<any>> => {
  try {
    const response = await axiosInstance.post("QLNhanSu/nhanvien", data);
    return response.data as ApiResponse<any>;
  } catch (error: any) {
    throw new Error(error.response?.data?.message || "Lỗi khi tạo nhân viên");
  }
};

// ✅ PUT /api/QLNhanSu/nhanvien - Cập nhật nhân viên
export const updateEmployee = async (
  data: NhanVienDTO
): Promise<ApiResponse<any>> => {
  try {
    const response = await axiosInstance.put("QLNhanSu/nhanvien", data);
    return response.data as ApiResponse<any>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Lỗi khi cập nhật nhân viên"
    );
  }
};

export const updateNhanVienDaNghiViec = async (
  maNhanVien: string,
  daNghiViec: boolean
): Promise<ApiResponse<null>> => {
  try {
    const res = await axiosInstance.patch(`QLNhanSu/nhanvien/danghiviec`,
      null,
      { params: { maNhanVien, daNghiViec } }
    );
    return res.data as ApiResponse<null>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Lỗi khi cập nhật trạng thái nghỉ việc"
    );
  }
}