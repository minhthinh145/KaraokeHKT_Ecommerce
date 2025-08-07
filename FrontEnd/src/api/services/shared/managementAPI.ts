import type { AdminAccountDTO, ApiResponse } from "../../types/index";
import axiosInstance from "../../axiosConfig";

export const getAllAdminAccount = async (): Promise<
  ApiResponse<AdminAccountDTO[]>
> => {
  try {
    const response = await axiosInstance.get("QLHeThong/adminAll");
    return response.data as ApiResponse<AdminAccountDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Lỗi khi lấy danh sách quản lý tài khoản"
    );
  }
};
