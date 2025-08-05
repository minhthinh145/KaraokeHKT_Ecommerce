import type { UserProfileDTO } from "../../api/types/auth/UserProfileDTO";

export interface AuthState {
  user: UserProfileDTO | null;
  accessToken: string | null;
  refreshToken: string | null;
  loading: boolean;
  error: string | null;
  isAuthenticated: boolean;
}

export interface SignInResponse {
  accessToken: string;
  refreshToken: string;
  user: UserProfileDTO;
}

export interface SignUpResponse {
  accessToken: string;
  refreshToken: string;
  message: string;
}

export interface UpdateUserPayload extends Partial<UserProfileDTO> {
  // Có thể thêm fields validation hoặc metadata
}

export type AuthError = string | null;
export type AuthLoadingState = boolean;
