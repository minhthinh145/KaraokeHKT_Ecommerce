import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit";
import type { SignInDTO } from "../../api/types/auth/AuthDTO";
import { SignIn } from "../../api/index";

interface AuthState {
  user: any;
  token: string | null;
  loading: boolean;
  error: string | null;
  isAuthenticated: boolean;
}

const initialState: AuthState = {
  user: null,
  token: null,
  loading: false,
  error: null,
  isAuthenticated: false,
};

export const signInThunk = createAsyncThunk(
  "auth/signIn",
  async (payload: SignInDTO, { rejectWithValue }) => {
    try {
      const response = await SignIn(payload);

      // Kiểm tra response structure
      console.log("API Response:", response); // Debug

      if (response.isSuccess && response.data) {
        // Lưu token
        if (response.data.AccessToken) {
          localStorage.setItem("accessToken", response.data.AccessToken);
        }

        // Return data thành công
        return {
          token: response.data.AccessToken,
          message: response.message, // "Login successful."
        };
      } else {
        // Nếu isSuccess = false
        return rejectWithValue(response.message || "Đăng nhập thất bại");
      }
    } catch (error: any) {
      return rejectWithValue(error.message || "Có lỗi xảy ra khi đăng nhập");
    }
  }
);

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    logout(state) {
      state.user = null;
      state.token = null;
      state.error = null;
      state.isAuthenticated = false;
      // Xóa token khỏi localStorage
      localStorage.removeItem("accessToken");
    },
    clearError(state) {
      state.error = null;
    },
    // Action để restore auth state từ localStorage khi app khởi động
    restoreAuth(state) {
      const token = localStorage.getItem("accessToken");
      if (token) {
        state.token = token;
        state.isAuthenticated = true;
      }
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(signInThunk.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(signInThunk.fulfilled, (state, action: PayloadAction<any>) => {
        state.loading = false;
        state.isAuthenticated = true;
        state.user = action.payload?.user || action.payload;
        state.token = action.payload?.token || action.payload?.accessToken;
        state.error = null;
      })
      .addCase(signInThunk.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
        state.isAuthenticated = false;
        state.user = null;
        state.token = null;
      });
  },
});

export const { logout, clearError, restoreAuth } = authSlice.actions;
export default authSlice.reducer;
