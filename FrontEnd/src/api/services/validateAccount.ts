import axiosInstance from "../axiosConfig";
import type { ApiResponse } from "../types/apiResponse";
import type { VerifyAccountDTO } from "../types/auth/VerifyAccountDTO";

export const SendOtp = async (email: string): Promise<ApiResponse<null>> => {
  const response = await axiosInstance.post<ApiResponse<null>>(
    "VerifyAuth/sendOtp",
    email,
    { headers: { "Content-Type": "application/json" } }
  );
  return response.data;
};

export const VerifyAccount = async (
  data: VerifyAccountDTO
): Promise<ApiResponse<null>> => {
  const response = await axiosInstance.post<ApiResponse<null>>(
    "VerifyAuth/verify-account",
    data
  );
  return response.data;
};
