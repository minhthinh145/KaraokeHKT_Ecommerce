import type { UserProfileDTO } from "../api/types/auth/UserProfileDTO";
import { ApplicationRole } from "../api/types/admins/QLHeThongtypes";
import type { SignInDTO } from "../api";

// 🔥 SỬA: UserRole type
export type UserRole = keyof typeof ApplicationRole;

// 🔥 EXPORT UserRole để dùng ở nơi khác

// 🔥 Auth state types - DUY NHẤT ở đây
export interface AuthState {
  isAuthenticated: boolean;
  accessToken: string | null;
  refreshToken: string | null;
  user: AuthUser | null;
  loading: boolean;
  error: string | null;
}

// AuthUser chỉ nên chứa loaiTaiKhoan và ánh xạ UserProfileDTO (nếu có)
export interface AuthUser {
  loaiTaiKhoan: UserRole; // luôn có từ login
  profile?: UserProfileDTO; // sẽ được gán sau khi gọi getProfile
  profileLoaded?: boolean; // flag đã load profile
}

// Login response từ backend (chỉ 3 trường)
export interface LoginResponse {
  loaiTaiKhoan: UserRole;
  accessToken: string;
  refreshToken: string;
}

// Login request

// Auth action payloads
export interface LoginSuccessPayload {
  accessToken: string;
  refreshToken: string;
  loaiTaiKhoan: UserRole;
}

export interface ProfileLoadedPayload extends UserProfileDTO {
  // Profile data từ API getProfile
}

// Auth thunk responses
export interface LoginThunkResponse extends LoginSuccessPayload {
  // Response từ login thunk
}

export interface ProfileThunkResponse extends UserProfileDTO {
  // Response từ profile thunk
}

// Local storage keys
export const AUTH_STORAGE_KEYS = {
  ACCESS_TOKEN: "accessToken",
  REFRESH_TOKEN: "refreshToken",
  USER_ROLE: "userRole",
  USER_DATA: "userData",
} as const;

// Auth error types
export type AuthErrorType =
  | "INVALID_CREDENTIALS"
  | "TOKEN_EXPIRED"
  | "NETWORK_ERROR"
  | "PROFILE_LOAD_FAILED"
  | "UNKNOWN_ERROR";

export interface AuthError {
  type: AuthErrorType;
  message: string;
  details?: any;
}

// Route protection types
export interface ProtectedRouteProps {
  children: React.ReactNode;
  requiredRole?: UserRole;
  fallbackRoute?: string;
}

export interface RoleBasedRouteProps {
  children: React.ReactNode;
  allowedRoles: string[]; // 🔥 SỬA: từ UserRole[] thành string[]
  fallbackRoute?: string;
}

// Auth context/hook return type
export interface UseAuthReturn extends Omit<AuthState, "user"> {
  user: AuthUser | null;
  userRole: UserRole | null;
  defaultRoute: string;

  // Actions
  login: (credentials: SignInDTO) => Promise<any>;
  logout: () => void;
  loadProfile: () => Promise<UserProfileDTO>;
  clearError: () => void;

  // Role checks
  hasRole: (role: UserRole) => boolean;
  isInRoleGroup: (
    group: keyof typeof import("../constants/auth").ROLE_GROUPS
  ) => boolean;
  canAccessRoute: (route: string) => boolean;

  // Specific role checks
  isQuanTriHeThong: () => boolean;
  isQuanLy: () => boolean;
  isNhanVien: () => boolean;
  isAdmin: () => boolean;

  // 🔥 THÊM: Navigation helpers
  navigateToDefaultRoute: () => void;
  navigateToRole: (role: UserRole) => void;

  // 🔥 THÊM: Profile helpers
  hasProfile: boolean;
  profileLoaded: boolean;

  // 🔥 THÊM: User info shortcuts
  userName: string;
  userEmail: string;
  userPhone: string;
}

// Redux selectors return types
export interface AuthSelectors {
  auth: AuthState;
  user: AuthUser | null;
  userRole: UserRole | null;
  isAuthenticated: boolean;
  defaultRoute: string;
  loading: boolean;
  error: string | null;
}
