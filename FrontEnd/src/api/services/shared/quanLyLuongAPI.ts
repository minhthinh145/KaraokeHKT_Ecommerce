import axiosInstance from "../../axiosConfig";
import type { ApiResponse } from "../../types/apiResponse";
import type { LuongCaLamViecDTO, AddLuongCaLamViecDTO } from "../../types";

const BASE = "QLTienLuong";

// Lấy tất cả
export const getAllLuongCaLamViecs = async (): Promise<
  ApiResponse<LuongCaLamViecDTO[]>
> => {
  try {
    const res = await axiosInstance.get(`${BASE}/getall`);
    return res.data as ApiResponse<LuongCaLamViecDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
        "Có lỗi xảy ra khi lấy danh sách lương ca làm việc"
    );
  }
};

// Lấy theo id
export const getLuongCaLamViecById = async (
  maLuongCaLamViec: number
): Promise<ApiResponse<LuongCaLamViecDTO>> => {
  try {
    const res = await axiosInstance.get(`${BASE}/getbyid/${maLuongCaLamViec}`);
    return res.data as ApiResponse<LuongCaLamViecDTO>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Có lỗi xảy ra khi lấy lương ca làm việc"
    );
  }
};

// Tạo mới
export const createLuongCaLamViec = async (
  data: AddLuongCaLamViecDTO
): Promise<ApiResponse<LuongCaLamViecDTO>> => {
  try {
    const payload = {
      maCa: data.maCa,
      ngayApDung: data.ngayApDung || null,
      ngayKetThuc: data.ngayKetThuc || null,
      giaCa: data.giaCa,
      isDefault: data.isDefault ?? false,
    };
    const res = await axiosInstance.post(`${BASE}/create`, payload);
    return res.data as ApiResponse<LuongCaLamViecDTO>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Có lỗi xảy ra khi tạo lương ca làm việc"
    );
  }
};

// Xóa
export const deleteLuongCaLamViec = async (
  maLuongCaLamViec: number
): Promise<ApiResponse<null>> => {
  try {
    const res = await axiosInstance.delete(
      `${BASE}/delete/${maLuongCaLamViec}`
    );
    return res.data as ApiResponse<null>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Có lỗi xảy ra khi xóa lương ca làm việc"
    );
  }
};
