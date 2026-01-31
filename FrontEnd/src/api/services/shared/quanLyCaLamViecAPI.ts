import axiosInstance from "../../axiosConfig";
import type { ApiResponse } from "../../types/apiResponse";
import type {
  CaLamViecDTO,
  AddCaLamViecDTO,
} from "../../index";

const BASE = "QLCaLamViec";

// Lấy tất cả ca làm việc
export const getAllCaLamViecs = async (): Promise<
  ApiResponse<CaLamViecDTO[]>
> => {
  try {
    const res = await axiosInstance.get(`${BASE}/getall`);
    return res.data as ApiResponse<CaLamViecDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
        "Có lỗi xảy ra khi lấy danh sách ca làm việc"
    );
  }
};

// Lấy ca làm việc theo id
export const getCaLamViecById = async (
  maCa: number
): Promise<ApiResponse<CaLamViecDTO>> => {
  try {
    const res = await axiosInstance.get(`${BASE}/getbyid/${maCa}`);
    return res.data as ApiResponse<CaLamViecDTO>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Có lỗi xảy ra khi lấy ca làm việc"
    );
  }
};

// Tạo mới ca làm việc (nếu cần)
export const createCaLamViec = async (
  data: AddCaLamViecDTO
): Promise<ApiResponse<CaLamViecDTO>> => {
  try {
    const res = await axiosInstance.post(`${BASE}/create`, data);
    return res.data as ApiResponse<CaLamViecDTO>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Có lỗi xảy ra khi tạo ca làm việc"
    );
  }
};
