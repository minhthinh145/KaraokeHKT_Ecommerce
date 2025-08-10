import axiosInstance from "../axiosConfig";
import type { ApiResponse } from "../types/apiResponse";
import type {
  SignInDTO,
  SignUpDTO,
  LoginResponseDTO,
} from "../types/auth/AuthDTO";

export const SignIn = async (
  data: SignInDTO
): Promise<ApiResponse<LoginResponseDTO>> => {
  const response = await axiosInstance.post<ApiResponse<LoginResponseDTO>>(
    "auth/signin",
    data
  );
  return response.data;
};

export const SignUp = async (
  data: SignUpDTO
): Promise<ApiResponse<LoginResponseDTO>> => {
  const response = await axiosInstance.post<ApiResponse<LoginResponseDTO>>(
    "auth/signup",
    data
  );
  return response.data;
};
export const SignOut = async (): Promise<ApiResponse<null>> => {
  const response = await axiosInstance.post<ApiResponse<null>>("/signout");
  return response.data;
};
