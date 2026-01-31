import { createAsyncThunk } from "@reduxjs/toolkit";
import type { SignInDTO, SignUpDTO } from "../../api/types/auth/AuthDTO";
import type { UserProfileDTO } from "../../api/types/auth/UserProfileDTO";
import {
  SignIn,
  SignUp,
  getProfile,
  updateProfile,
} from "../../api/services/index";
import type { AuthState, SignUpResponse } from "./types";
import { saveAuthData, clearAuthData } from "./utils";
import type { AuthUser } from "../../types/auth";
import type { UserRole } from "../../constants/auth";
import { fetchNhanVienProfile } from "../../redux/nhanVien/thunks";

// 🔥 Sign In Thunk - return đúng type
export const signInThunk = createAsyncThunk<
  { accessToken: string; refreshToken: string; user: AuthUser },
  SignInDTO
>("auth/signIn", async (payload, { rejectWithValue, dispatch }) => {
  try {
    const loginResponse = await SignIn(payload);
    if (!loginResponse.isSuccess || !loginResponse.data) {
      return rejectWithValue(loginResponse);
    }

    const { accessToken, refreshToken, loaiTaiKhoan } = loginResponse.data;

    // 🔥 Tạo AuthUser object đúng type
    const user: AuthUser = {
      loaiTaiKhoan: loaiTaiKhoan as UserRole,
      profileLoaded: false,
    };

    saveAuthData(user, accessToken, refreshToken);

    dispatch(fetchNhanVienProfile()); // lấy profile ngay sau khi login thành công

    return {
      accessToken,
      refreshToken,
      user, // 🔥 AuthUser type
    };
  } catch (error: any) {
    return rejectWithValue({
      isSuccess: false,
      message: error.message || "Có lỗi xảy ra khi đăng nhập",
      data: null,
    });
  }
});

// 🔥 Sign Up Thunk
export const signUpThunk = createAsyncThunk<SignUpResponse, SignUpDTO>(
  "auth/signUp",
  async (payload, { rejectWithValue }) => {
    try {
      const response = await SignUp(payload);

      if (!response.isSuccess || !response.data) {
        return rejectWithValue(response);
      }

      const { accessToken, refreshToken } = response.data;
      saveAuthData(undefined, accessToken, refreshToken);

      return {
        accessToken,
        refreshToken,
        message: response.message || "Đăng ký thành công",
      };
    } catch (error: any) {
      if (error.response?.data) {
        return rejectWithValue(error.response.data);
      }

      return rejectWithValue({
        isSuccess: false,
        message: error.message || "Có lỗi xảy ra khi đăng ký",
        data: null,
      });
    }
  }
);

// 🔥 Fetch Profile Thunk - chỉ return UserProfileDTO
export const fetchProfileThunk = createAsyncThunk<UserProfileDTO, void>(
  "auth/fetchProfile",
  async (_, { rejectWithValue }) => {
    try {
      const response = await getProfile();

      if (!response.isSuccess || !response.data) {
        return rejectWithValue(
          response.message || "Lấy thông tin người dùng thất bại"
        );
      }

      // 🔥 Chỉ return UserProfileDTO, việc update AuthUser sẽ xử lý trong reducer
      return response.data;
    } catch (error: any) {
      return rejectWithValue(
        error.message || "Lỗi khi lấy thông tin người dùng"
      );
    }
  }
);

// 🔥 Update User Thunk - chỉ return UserProfileDTO
export const updateUserThunk = createAsyncThunk<UserProfileDTO, UserProfileDTO>(
  "auth/updateUser",
  async (payload, { rejectWithValue, getState }) => {
    try {
      const state = getState() as { auth: AuthState };
      if (!state.auth.user) {
        return rejectWithValue("Người dùng chưa đăng nhập");
      }

      const response = await updateProfile(payload);

      if (!response.isSuccess || !response.data) {
        return rejectWithValue(response.message || "Cập nhật thất bại");
      }

      // 🔥 Chỉ return UserProfileDTO mới
      return response.data;
    } catch (error: any) {
      const errorMessage =
        error?.response?.data?.message ||
        error?.message ||
        "Có lỗi xảy ra khi cập nhật";
      return rejectWithValue(errorMessage);
    }
  }
);

// 🔥 Logout Thunk
export const logoutThunk = createAsyncThunk<null, void>(
  "auth/logout",
  async () => {
    try {
      clearAuthData();
      return null;
    } catch (error: any) {
      clearAuthData();
      return null;
    }
  }
);

// 🔥 Check Auth Status Thunk
export const checkAuthStatusThunk = createAsyncThunk<
  { user: AuthUser; profile: UserProfileDTO } | null,
  void
>("auth/checkStatus", async () => {
  try {
    const accessToken = localStorage.getItem("accessToken");
    const savedUser = localStorage.getItem("user");

    if (!accessToken || !savedUser) {
      clearAuthData();
      return null;
    }

    // 🔥 Parse AuthUser từ localStorage
    const user: AuthUser = JSON.parse(savedUser);

    // 🔥 Nếu chưa có profile, fetch từ API
    if (!user.profileLoaded || !user.profile) {
      const response = await getProfile();

      if (response.isSuccess && response.data) {
        return {
          user: {
            loaiTaiKhoan: user.loaiTaiKhoan,
            profileLoaded: true,
          },
          profile: response.data,
        };
      }
    }

    // 🔥 Nếu đã có profile, return luôn
    return {
      user,
      profile: user.profile!,
    };
  } catch (error: any) {
    clearAuthData();
    return null;
  }
});
