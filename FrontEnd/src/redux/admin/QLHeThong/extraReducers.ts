import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { QLHeThongState } from "./types";
import {
  fetchAllNhanVien,
  fetchAllKhachHang,
  createNhanVienAccount,
  createAdminAccount,
  fetchAllAdminAccount,
  fetchLoaiTaiKhoan,
  lockAccountThunk,
  unlockAccountThunk,
  deleteAccountThunk,
  updateAdminAccountThunk,
} from "./thunks";

// 🔥 Extra Reducers for Async Thunks
export const qlHeThongExtraReducers = (
  builder: ActionReducerMapBuilder<QLHeThongState>
) => {
  // 📋 Fetch All Nhân viên
  builder
    .addCase(fetchAllNhanVien.pending, (state) => {
      state.nhanVien.loading = true;
      state.nhanVien.error = null;
    })
    .addCase(fetchAllNhanVien.fulfilled, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.data = action.payload;
      state.nhanVien.total = action.payload.length;
      state.nhanVien.error = null;
    })
    .addCase(fetchAllNhanVien.rejected, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.error = action.payload as string;
    });

  // 👤 Fetch All Khách hàng
  builder
    .addCase(fetchAllKhachHang.pending, (state) => {
      state.khachHang.loading = true;
      state.khachHang.error = null;
    })
    .addCase(fetchAllKhachHang.fulfilled, (state, action) => {
      state.khachHang.loading = false;
      state.khachHang.data = action.payload;
      state.khachHang.total = action.payload.length;
      state.khachHang.error = null;
    })
    .addCase(fetchAllKhachHang.rejected, (state, action) => {
      state.khachHang.loading = false;
      state.khachHang.error = action.payload as string;
    });

  // 🏷️ Fetch Loại tài khoản
  builder
    .addCase(fetchLoaiTaiKhoan.pending, (state) => {
      state.loaiTaiKhoan.loading = true;
      state.loaiTaiKhoan.error = null;
    })
    .addCase(fetchLoaiTaiKhoan.fulfilled, (state, action) => {
      state.loaiTaiKhoan.loading = false;
      state.loaiTaiKhoan.data = action.payload;
      state.loaiTaiKhoan.error = null;
    })
    .addCase(fetchLoaiTaiKhoan.rejected, (state, action) => {
      state.loaiTaiKhoan.loading = false;
      state.loaiTaiKhoan.error = action.payload as string;
    });

  // ➕ Create Nhân viên
  builder
    .addCase(createNhanVienAccount.pending, (state) => {
      state.nhanVien.loading = true;
      state.nhanVien.error = null;
    })
    .addCase(createNhanVienAccount.fulfilled, (state, action) => {
      state.nhanVien.loading = false;
      if (action.payload) {
        state.nhanVien.data.push(action.payload);
        state.nhanVien.total += 1;
      }
      state.nhanVien.error = null;
    })
    .addCase(createNhanVienAccount.rejected, (state, action) => {
      state.nhanVien.loading = false;
      state.nhanVien.error = action.payload as string;
    });

  // 📋 Fetch All Admin Account
  builder
    .addCase(fetchAllAdminAccount.pending, (state) => {
      state.adminAccount.loading = true;
      state.adminAccount.error = null;
    })
    .addCase(fetchAllAdminAccount.fulfilled, (state, action) => {
      state.adminAccount.loading = false;
      state.adminAccount.data = action.payload || [];
      state.adminAccount.total = action.payload?.length || 0;
      state.adminAccount.error = null;
    })
    .addCase(fetchAllAdminAccount.rejected, (state, action) => {
      state.adminAccount.loading = false;
      state.adminAccount.error = action.payload as string;
    });

  // ➕ Create Admin Account
  builder
    .addCase(createAdminAccount.pending, (state) => {
      state.adminAccount.loading = true;
      state.adminAccount.error = null;
    })
    .addCase(createAdminAccount.fulfilled, (state, action) => {
      state.adminAccount.loading = false;
      if (action.payload) {
        state.adminAccount.data.push(action.payload);
        state.adminAccount.total += 1;
      }
      state.adminAccount.error = null;
    })
    .addCase(createAdminAccount.rejected, (state, action) => {
      state.adminAccount.loading = false;
      state.adminAccount.error = action.payload as string;
    });

  //delete admin account
  builder
    .addCase(deleteAccountThunk.pending, (state) => {
      state.adminAccount.loading = true;
      state.adminAccount.error = null;
    })
    .addCase(deleteAccountThunk.fulfilled, (state, action) => {
      state.adminAccount.loading = false;
      state.adminAccount.data = state.adminAccount.data.filter(
        (acc) => acc.maTaiKhoan !== action.meta.arg
      );
      state.adminAccount.total -= 1;
      state.adminAccount.error = null;
    })
    .addCase(deleteAccountThunk.rejected, (state, action) => {
      state.adminAccount.loading = false;
      state.adminAccount.error = action.payload as string;
    });

  // 🔒 Lock Account
  function updateLockStatus(
    state: QLHeThongState,
    maTaiKhoan: string,
    locked: boolean
  ) {
    [
      state.nhanVien.data,
      state.khachHang.data,
      state.adminAccount.data,
    ].forEach((list) => {
      const idx = list.findIndex((acc) => acc.maTaiKhoan === maTaiKhoan);
      if (idx !== -1) list[idx].daBiKhoa = locked;
    });
  }

  builder
    .addCase(lockAccountThunk.pending, (state) => {
      state.nhanVien.loading =
        state.khachHang.loading =
        state.adminAccount.loading =
          true;
      state.nhanVien.error =
        state.khachHang.error =
        state.adminAccount.error =
          null;
    })
    .addCase(lockAccountThunk.fulfilled, (state, action) => {
      updateLockStatus(state, action.meta.arg, true);
      state.nhanVien.loading =
        state.khachHang.loading =
        state.adminAccount.loading =
          false;
    })
    .addCase(lockAccountThunk.rejected, (state, action) => {
      state.nhanVien.loading =
        state.khachHang.loading =
        state.adminAccount.loading =
          false;
      state.nhanVien.error =
        state.khachHang.error =
        state.adminAccount.error =
          action.payload as string;
    })
    // 🔓 Unlock Account
    .addCase(unlockAccountThunk.pending, (state) => {
      state.nhanVien.loading =
        state.khachHang.loading =
        state.adminAccount.loading =
          true;
      state.nhanVien.error =
        state.khachHang.error =
        state.adminAccount.error =
          null;
    })
    .addCase(unlockAccountThunk.fulfilled, (state, action) => {
      updateLockStatus(state, action.meta.arg, false);
      state.nhanVien.loading =
        state.khachHang.loading =
        state.adminAccount.loading =
          false;
    })
    .addCase(unlockAccountThunk.rejected, (state, action) => {
      state.nhanVien.loading =
        state.khachHang.loading =
        state.adminAccount.loading =
          false;
      state.nhanVien.error =
        state.khachHang.error =
        state.adminAccount.error =
          action.payload as string;
    });

  builder
    .addCase(updateAdminAccountThunk.pending, (state) => {
      state.adminAccount.loading = true;
      state.adminAccount.error = null;
    })
    .addCase(updateAdminAccountThunk.fulfilled, (state, action) => {
      state.adminAccount.loading = false;

      // Thunk (đề xuất) trả về: { request: UpdateAccountDTO, apiData?: UpdatedAccount }
      const { request, apiData } = action.payload as {
        request: {
          maTaiKhoan: string;
          newUserName: string;
          newPassword: string;
          newLoaiTaiKhoan: string;
        };
        apiData?: any;
      };

      const idx = state.adminAccount.data.findIndex(
        (acc) => acc.maTaiKhoan === request.maTaiKhoan
      );
      if (idx !== -1) {
        const current = state.adminAccount.data[idx];
        state.adminAccount.data[idx] = {
          ...current,
          // ưu tiên dữ liệu server trả về nếu có
          ...(apiData || {}),
          userName: request.newUserName || apiData?.userName || current.email,
          loaiTaiKhoan:
            request.newLoaiTaiKhoan ||
            apiData?.loaiTaiKhoan ||
            current.loaiTaiKhoan,
          // Không đổi password ở client list (thường không hiển thị)
        };
      }

      state.adminAccount.error = null;

      // Đóng modal nếu bạn có các flag này trong UI state
      if (state.ui) {
        // đổi tên theo đúng state thực tế nếu khác
        (state.ui as any).showUpdateAdminModal = false;
        (state.ui as any).selectedAdminAccount = null;
      }
    })
    .addCase(updateAdminAccountThunk.rejected, (state, action) => {
      state.adminAccount.loading = false;
      state.adminAccount.error =
        (action.payload as string) || "Không thể cập nhật tài khoản";
    });
};
