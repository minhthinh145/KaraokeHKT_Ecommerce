import { createAsyncThunk } from "@reduxjs/toolkit";
import type { SignInDTO, SignUpDTO } from "../../api/types/auth/AuthDTO";
import type { UserProfileDTO } from "../../api/types/auth/UserProfileDTO";
import {
  SignIn,
  SignUp,
  getProfile,
  updateProfile,
} from "../../api/services/index";
import type { AuthState, SignInResponse, SignUpResponse } from "./types";
import { saveAuthData, clearAuthData } from "./utils";

// 🔥 Sign In Thunk
export const signInThunk = createAsyncThunk<SignInResponse, SignInDTO>(
  "auth/signIn",
  async (payload, { rejectWithValue }) => {
    try {
      const loginResponse = await SignIn(payload);
      if (!loginResponse.isSuccess || !loginResponse.data) {
        return rejectWithValue(loginResponse);
      }

      const { accessToken, refreshToken } = loginResponse.data;
      saveAuthData(undefined, accessToken, refreshToken);

      const profileResponse = await getProfile();

      if (!profileResponse.isSuccess || !profileResponse.data) {
        return rejectWithValue({
          isSuccess: false,
          message: "Không thể lấy thông tin người dùng",
          data: null,
        });
      }

      saveAuthData(profileResponse.data);

      return {
        accessToken,
        refreshToken,
        user: profileResponse.data,
      };
    } catch (error: any) {
      if (error.response?.data) {
        return rejectWithValue(error.response.data);
      }

      return rejectWithValue({
        isSuccess: false,
        message: error.message || "Có lỗi xảy ra khi đăng nhập",
        data: null,
      });
    }
  }
);

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

// 🔥 Fetch Profile Thunk
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

      saveAuthData(response.data);
      return response.data;
    } catch (error: any) {
      return rejectWithValue(
        error.message || "Lỗi khi lấy thông tin người dùng"
      );
    }
  }
);

// 🔥 Update User Thunk
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

      saveAuthData(response.data);
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
  async (_, { getState }) => {
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
  UserProfileDTO | null,
  void
>("auth/checkStatus", async (_, { rejectWithValue }) => {
  try {
    const accessToken = localStorage.getItem("accessToken");

    if (!accessToken) {
      return null;
    }

    const response = await getProfile();

    if (!response.isSuccess || !response.data) {
      clearAuthData();
      return null;
    }

    return response.data;
  } catch (error: any) {
    clearAuthData();
    return null;
  }
});
