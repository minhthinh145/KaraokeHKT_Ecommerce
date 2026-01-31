import axiosInstance from "../axiosConfig";
import type { ApiResponse } from "../types/apiResponse";
import type {
  ChangePasswordDTO,
  ConfirmChangePasswordDTO,
} from "../types/index";

/**
 * Request change password - Gửi OTP để xác thực
 * @param data - Thông tin mật khẩu cũ và mới
 * @returns Promise với kết quả request
 */
export const requestChangePassword = async (
  data: ChangePasswordDTO
): Promise<ApiResponse<any>> => {
  try {
    const response = await axiosInstance.post("changepassword/request", data);
    return response.data as ApiResponse<any>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Có lỗi xảy ra khi yêu cầu đổi mật khẩu"
    );
  }
};

/**
 * Confirm change password with OTP - Xác nhận đổi mật khẩu bằng OTP
 * @param data - Thông tin mật khẩu mới và OTP
 * @returns Promise với kết quả confirm
 */
export const confirmChangePassword = async (
  data: ConfirmChangePasswordDTO
): Promise<ApiResponse<any>> => {
  try {
    const response = await axiosInstance.post("changepassword/confirm", data);
    return response.data as ApiResponse<any>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Có lỗi xảy ra khi xác nhận đổi mật khẩu"
    );
  }
};

/**
 * Cancel change password request - Hủy yêu cầu đổi mật khẩu
 * @returns Promise với kết quả cancel
 */
export const cancelChangePasswordRequest = async (): Promise<
  ApiResponse<any>
> => {
  try {
    const response = await axiosInstance.post("changepassword/cancel");
    return response.data as ApiResponse<any>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
        "Có lỗi xảy ra khi hủy yêu cầu đổi mật khẩu"
    );
  }
};
