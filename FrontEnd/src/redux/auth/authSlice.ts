import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit";
import type { SignInDTO, SignUpDTO } from "../../api/types/auth/AuthDTO";
import { SignIn, SignUp } from "../../api/services/SignInOut";
import { getProfile } from "../../api/services/getProfile.ts";
import type { UserProfileDTO } from "../../api/types/auth/UserProfileDTO";
import type { ApiResponse } from "../../api/types/apiResponse";

interface AuthState {
  user: UserProfileDTO | null;
  accessToken: string | null;
  refreshToken: string | null;
  loading: boolean;
  error: string | null;
  isAuthenticated: boolean;
}

// 🔥 Khôi phục state từ localStorage khi khởi động app
const getInitialState = (): AuthState => {
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
    // Nếu localStorage bị lỗi, clear và return initial state
    localStorage.removeItem("user");
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
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

const initialState: AuthState = getInitialState();

// 🔥 Đăng nhập và lấy profile trong 1 thunk
export const signInThunk = createAsyncThunk(
  "auth/signIn",
  async (payload: SignInDTO, { rejectWithValue }) => {
    try {
      const loginResponse = await SignIn(payload);

      if (!loginResponse.isSuccess || !loginResponse.data) {
        return rejectWithValue(loginResponse.message || "Đăng nhập thất bại");
      }

      const { accessToken, refreshToken } = loginResponse.data;

      localStorage.setItem("accessToken", accessToken);
      localStorage.setItem("refreshToken", refreshToken);

      const profileResponse = await getProfile();

      if (!profileResponse.isSuccess || !profileResponse.data) {
        return rejectWithValue("Không thể lấy thông tin người dùng");
      }

      localStorage.setItem("user", JSON.stringify(profileResponse.data));

      return {
        accessToken: accessToken,
        refreshToken: refreshToken,
        user: profileResponse.data,
      };
    } catch (error: any) {
      return rejectWithValue(error.message || "Có lỗi xảy ra khi đăng nhập");
    }
  }
);

// 🔥 Cũng fix SignUp nếu có cùng format
export const signUpThunk = createAsyncThunk(
  "auth/signUp",
  async (payload: SignUpDTO, { rejectWithValue }) => {
    try {
      const response = await SignUp(payload);

      if (!response.isSuccess || !response.data) {
        return rejectWithValue(response.message || "Đăng ký thất bại");
      }

      // 🔥 Fix: Dùng lowercase
      const { accessToken, refreshToken } = response.data;

      // Lưu token vào localStorage
      localStorage.setItem("accessToken", accessToken);
      localStorage.setItem("refreshToken", refreshToken);

      return {
        accessToken: accessToken,
        refreshToken: refreshToken,
        message: response.message || "Đăng ký thành công",
      };
    } catch (error: any) {
      return rejectWithValue(error.message || "Có lỗi xảy ra khi đăng ký");
    }
  }
);

// 🔥 Lấy profile user (dùng khi cần refresh thông tin)
export const fetchProfileThunk = createAsyncThunk(
  "auth/fetchProfile",
  async (_, { rejectWithValue }) => {
    try {
      const response = await getProfile();

      if (!response.isSuccess || !response.data) {
        return rejectWithValue(
          response.message || "Lấy thông tin người dùng thất bại"
        );
      }

      // Cập nhật user trong localStorage
      localStorage.setItem("user", JSON.stringify(response.data));

      return response.data;
    } catch (error: any) {
      return rejectWithValue(
        error.message || "Lỗi khi lấy thông tin người dùng"
      );
    }
  }
);

// 🔥 Logout (có thể gọi API logout nếu cần)
export const logoutThunk = createAsyncThunk(
  "auth/logout",
  async (_, { getState, rejectWithValue }) => {
    try {
      const state = getState() as { auth: AuthState };
      const refreshToken = state.auth.refreshToken;

      // Nếu có refreshToken, gọi API logout để revoke token
      if (refreshToken) {
        // Uncomment nếu có API logout
        // await logoutApi({ refreshToken });
      }

      // Clear localStorage
      localStorage.removeItem("user");
      localStorage.removeItem("accessToken");
      localStorage.removeItem("refreshToken");

      return null;
    } catch (error: any) {
      // Dù có lỗi cũng vẫn logout local
      localStorage.removeItem("user");
      localStorage.removeItem("accessToken");
      localStorage.removeItem("refreshToken");
      return null;
    }
  }
);

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    // 🔥 Logout nhanh (không gọi API)
    logout(state) {
      state.user = null;
      state.accessToken = null;
      state.refreshToken = null;
      state.error = null;
      state.isAuthenticated = false;
      state.loading = false;

      // Clear localStorage
      localStorage.removeItem("user");
      localStorage.removeItem("accessToken");
      localStorage.removeItem("refreshToken");
    },

    // 🔥 Clear error
    clearError(state) {
      state.error = null;
    },

    // 🔥 Cập nhật user info
    updateUser(state, action: PayloadAction<Partial<UserProfileDTO>>) {
      if (state.user) {
        state.user = { ...state.user, ...action.payload };
        localStorage.setItem("user", JSON.stringify(state.user));
      }
    },

    // 🔥 Set loading state
    setLoading(state, action: PayloadAction<boolean>) {
      state.loading = action.payload;
    },

    // 🔥 Restore auth từ localStorage (gọi khi app khởi động)
    restoreAuth(state) {
      try {
        const savedUser = localStorage.getItem("user");
        const savedAccessToken = localStorage.getItem("accessToken");
        const savedRefreshToken = localStorage.getItem("refreshToken");

        if (savedAccessToken) {
          state.accessToken = savedAccessToken;
          state.refreshToken = savedRefreshToken;
          state.user = savedUser ? JSON.parse(savedUser) : null;
          state.isAuthenticated = true;
        }
      } catch (error) {
        // Nếu localStorage bị lỗi, logout
        state.user = null;
        state.accessToken = null;
        state.refreshToken = null;
        state.isAuthenticated = false;
      }
    },
  },
  extraReducers: (builder) => {
    builder
      // 🔥 SignIn
      .addCase(signInThunk.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(signInThunk.fulfilled, (state, action) => {
        state.loading = false;
        state.isAuthenticated = true;
        state.accessToken = action.payload.accessToken;
        state.refreshToken = action.payload.refreshToken;
        state.user = action.payload.user;
        state.error = null;
      })
      .addCase(signInThunk.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
        state.isAuthenticated = false;
        state.user = null;
        state.accessToken = null;
        state.refreshToken = null;
      })

      // 🔥 SignUp
      .addCase(signUpThunk.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(signUpThunk.fulfilled, (state, action) => {
        state.loading = false;
        state.isAuthenticated = true;
        state.accessToken = action.payload.accessToken;
        state.refreshToken = action.payload.refreshToken;
        state.error = null;
        // Chưa có user info, sẽ fetch sau
      })
      .addCase(signUpThunk.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
        state.isAuthenticated = false;
      })

      // 🔥 FetchProfile
      .addCase(fetchProfileThunk.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchProfileThunk.fulfilled, (state, action) => {
        state.loading = false;
        state.user = action.payload;
        state.error = null;
      })
      .addCase(fetchProfileThunk.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })

      // 🔥 Logout
      .addCase(logoutThunk.pending, (state) => {
        state.loading = true;
      })
      .addCase(logoutThunk.fulfilled, (state) => {
        state.user = null;
        state.accessToken = null;
        state.refreshToken = null;
        state.error = null;
        state.isAuthenticated = false;
        state.loading = false;
      })
      .addCase(logoutThunk.rejected, (state) => {
        // Dù có lỗi cũng vẫn logout
        state.user = null;
        state.accessToken = null;
        state.refreshToken = null;
        state.error = null;
        state.isAuthenticated = false;
        state.loading = false;
      });
  },
});

export const { logout, clearError, updateUser, setLoading, restoreAuth } =
  authSlice.actions;
export default authSlice.reducer;
