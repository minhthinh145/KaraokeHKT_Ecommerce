import type { AuthState } from "./types";

// 🔥 Safe localStorage operations
export const safeLocalStorageOperation = (operation: () => void) => {
  try {
    operation();
  } catch (error) {
    console.warn("localStorage operation failed:", error);
  }
};

// 🔥 Clear all auth data
export const clearAuthData = () => {
  safeLocalStorageOperation(() => {
    localStorage.removeItem("user");
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
  });
};

// 🔥 Save auth data
export const saveAuthData = (
  user?: any,
  accessToken?: string,
  refreshToken?: string
) => {
  safeLocalStorageOperation(() => {
    if (user) localStorage.setItem("user", JSON.stringify(user));
    if (accessToken) localStorage.setItem("accessToken", accessToken);
    if (refreshToken) localStorage.setItem("refreshToken", refreshToken);
  });
};

// 🔥 Get initial state from localStorage
export const getInitialState = (): AuthState => {
  try {
    const savedUser = localStorage.getItem("user");
    const savedAccessToken = localStorage.getItem("accessToken");
    const savedRefreshToken = localStorage.getItem("refreshToken");

    return {
      user: savedUser ? JSON.parse(savedUser) : null,
      accessToken: savedAccessToken,
      refreshToken: savedRefreshToken,
      loading: false,
      error: null,
      isAuthenticated: !!savedAccessToken,
    };
  } catch (error) {
    console.warn("Failed to restore auth state:", error);
    clearAuthData();
    return {
      user: null,
      accessToken: null,
      refreshToken: null,
      loading: false,
      error: null,
      isAuthenticated: false,
    };
  }
};

// 🔥 Validate token format (basic check)
export const isValidTokenFormat = (token: string): boolean => {
  try {
    // JWT has 3 parts separated by dots
    const parts = token.split(".");
    return parts.length === 3;
  } catch {
    return false;
  }
};

// 🔥 Check if token is expired (basic check without decoding)
export const isTokenExpired = (token: string): boolean => {
  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    const currentTime = Math.floor(Date.now() / 1000);
    return payload.exp < currentTime;
  } catch {
    return true; // Assume expired if can't parse
  }
};

// 🔥 Format user display name
export const formatUserDisplayName = (user: any): string => {
  return user?.userName || "User";
};

// 🔥 Validate user data completeness
export const validateUserProfile = (user: any) => {
  const errors: string[] = [];

  if (!user?.userName || user.userName.trim() === "") {
    errors.push("Tên người dùng không được để trống");
  }

  if (!user?.email || user.email.trim() === "") {
    errors.push("Email không được để trống");
  }

  if (user?.email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(user.email)) {
    errors.push("Email không đúng định dạng");
  }

  if (user?.phone && !/^[0-9]{10,11}$/.test(user.phone.replace(/\s/g, ""))) {
    errors.push("Số điện thoại không đúng định dạng");
  }

  return {
    isValid: errors.length === 0,
    errors,
  };
};
