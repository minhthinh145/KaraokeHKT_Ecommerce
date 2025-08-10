import type {
  AdminAccountDTO,
  ApiResponse,
  AddAdminAccountDTO,
} from "../../types/index";
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

export const addAdminAccount = async (
  data: AddAdminAccountDTO
): Promise<ApiResponse<AdminAccountDTO>> => {
  try {
    const response = await axiosInstance.post(
      "QLHeThong/taikhoan/admin/tao-tai-khoan",
      data
    );
    return response.data as ApiResponse<AdminAccountDTO>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Lỗi khi thêm tài khoản quản trị"
    );
  }
};


