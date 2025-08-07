import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { UserProfileDTO } from "../../api/types/auth/UserProfileDTO";
import type { AuthState } from "./types";
import {
  getInitialState,
  safeLocalStorageOperation,
  clearAuthData,
} from "./utils";
import {
  signInThunk,
  signUpThunk,
  fetchProfileThunk,
  updateUserThunk,
  logoutThunk,
  checkAuthStatusThunk,
} from "./Thunks";

const initialState: AuthState = getInitialState();

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    // 🔥 Sync logout
    logout(state) {
      state.user = null;
      state.accessToken = null;
      state.refreshToken = null;
      state.error = null;
      state.isAuthenticated = false;
      state.loading = false;
      clearAuthData();
    },

    // 🔥 Clear error
    clearError(state) {
      state.error = null;
    },

    updateUserLocal(state, action: PayloadAction<Partial<UserProfileDTO>>) {
      if (state.user) {
        state.user = { ...state.user, ...action.payload };
        safeLocalStorageOperation(() => {
          localStorage.setItem("user", JSON.stringify(state.user));
        });
      }
    },

    setLoading(state, action: PayloadAction<boolean>) {
      state.loading = action.payload;
    },

    setError(state, action: PayloadAction<string>) {
      state.error = action.payload;
    },

    restoreAuth(state) {
      const restoredState = getInitialState();
      Object.assign(state, restoredState);
    },
  },

  extraReducers: (builder) => {
    builder
      // 🔥 Sign In
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
      })

      // 🔥 Sign Up
      .addCase(signUpThunk.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(signUpThunk.fulfilled, (state, action) => {
        state.loading = false;

        state.error = null;
      })
      .addCase(signUpThunk.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
        state.isAuthenticated = false;
      })

      // 🔥 Fetch Profile
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

      // 🔥 Update User
      .addCase(updateUserThunk.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateUserThunk.fulfilled, (state, action) => {
        state.loading = false;
        state.user = action.payload;
        state.error = null;
      })
      .addCase(updateUserThunk.rejected, (state, action) => {
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
        state.user = null;
        state.accessToken = null;
        state.refreshToken = null;
        state.error = null;
        state.isAuthenticated = false;
        state.loading = false;
      })

      // 🔥 Check Auth Status
      .addCase(checkAuthStatusThunk.pending, (state) => {
        state.loading = true;
      })
      .addCase(checkAuthStatusThunk.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload) {
          state.user = action.payload;
          state.isAuthenticated = true;
        } else {
          state.isAuthenticated = false;
          state.user = null;
        }
      })
      .addCase(checkAuthStatusThunk.rejected, (state) => {
        state.loading = false;
        state.isAuthenticated = false;
        state.user = null;
      });
  },
});

export const {
  logout,
  clearError,
  updateUserLocal,
  setLoading,
  setError,
  restoreAuth,
} = authSlice.actions;

export default authSlice.reducer;
