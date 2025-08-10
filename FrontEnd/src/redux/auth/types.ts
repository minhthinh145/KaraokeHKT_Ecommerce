import type { UserProfileDTO } from "../../api/types/auth/UserProfileDTO";
import type { AuthState, AuthUser } from "../../types/auth";

// 🔥 EXPORT lại AuthState từ common types (không duplicate)
export type { AuthState, AuthUser } from "../../types/auth";

// 🔥 Chỉ định nghĩa types đặc thù cho Redux slice
export interface SignUpResponse {
  accessToken: string;
  refreshToken: string;
  message: string;
}

// API Response types (đặc thù cho thunks)
export interface LoginApiResponse {
  loaiTaiKhoan: string;
  accessToken: string;
  refreshToken: string;
}

export interface ProfileApiResponse extends UserProfileDTO {
  // Profile data từ API
}

// Error response (đặc thù cho thunks)
export interface AuthErrorResponse {
  isSuccess: false;
  message: string;
  data: null;
}

// Thunk payload types (đặc thù cho Redux)
export interface LoginThunkPayload {
  accessToken: string;
  refreshToken: string;
  user: AuthUser;
}

export interface ProfileThunkPayload extends UserProfileDTO {
  // Profile update payload
}
