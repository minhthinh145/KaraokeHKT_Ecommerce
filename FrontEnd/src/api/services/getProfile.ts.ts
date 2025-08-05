import axios from "../axiosConfig";
import type { UserProfileDTO } from "../types/auth/UserProfileDTO";
import type { ApiResponse } from "../types/apiResponse";

export const getProfile = async (): Promise<ApiResponse<UserProfileDTO>> => {
  const response = await axios.get("Auth/profile");
  return response.data as ApiResponse<UserProfileDTO>;
};

export const updateProfile = async (
  userData: UserProfileDTO
): Promise<ApiResponse<UserProfileDTO>> => {
  const response = await axios.patch("Auth/update", userData);
  return response.data as ApiResponse<UserProfileDTO>;
};
