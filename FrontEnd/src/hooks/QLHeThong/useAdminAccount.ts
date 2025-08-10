import { useCallback, useMemo } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useAppDispatch, useAppSelector } from "../../redux/hooks"; // ✅ Đồng nhất
import {
  clearAdminAccountError,
  createAdminAccount,
  fetchAllAdminAccount,
  selectAdminAccountState,
  selectAdminAccountStats,
  selectFilteredAdminAccount,
} from "../../redux/admin";
import { useAccountLockToggle } from "../shared/useLockableAccount";
import type { AddAdminAccountDTO } from "../../api";
import { message } from "antd";
import {
  deleteAccountThunk,
  updateAdminAccountThunk,
} from "../../redux/admin/QLHeThong";
import type { UpdateAccountDTO } from "../../api/types/admins/QLHeThongtypes";
import type { AppDispatch, RootState } from "../../redux/store";
import { useToast } from "../useToast";

export const useAdminAccount = ({ autoLoad = true } = {}) => {
  const dispatch = useAppDispatch();
  const adminAccount = useSelector((s: RootState) => s.qlHeThong.adminAccount);

  // Selectors
  const adminAccountState = useAppSelector(selectAdminAccountState); // ✅ useAppSelector
  const filteredAdminAccount = useAppSelector(selectFilteredAdminAccount);
  const adminAccountStats = useAppSelector(selectAdminAccountStats);
  const { showSuccess, showError } = useToast();
  // Refresh callback
  const refreshAdminAccountData = useCallback(() => {
    dispatch(fetchAllAdminAccount());
  }, [dispatch]);

  // Lock/unlock handlers
  const adminAccountLockHandlers = useAccountLockToggle();

  // Add admin account
  const handleAddAdminAccount = useCallback(
    async (data: AddAdminAccountDTO) => {
      try {
        const response = await dispatch(createAdminAccount(data));
        if (response.meta.requestStatus === "fulfilled") {
          showSuccess("Thêm tài khoản quản trị thành công");
          refreshAdminAccountData();
          return { success: true };
        } else {
          showError("Tạo tài khoản thất bại!");
          return { success: false, error: response.payload as string };
        }
      } catch (error: any) {
        showError("Không thể thêm tài khoản quản trị");
        return { success: false, error: error.message };
      }
    },
    [refreshAdminAccountData]
  );

  //delete admin account
  const handleDeleteAdminAccount = useCallback(
    async (maTaiKhoan: string) => {
      try {
        const response = await dispatch(deleteAccountThunk(maTaiKhoan));
        if (response.meta.requestStatus === "fulfilled") {
          showSuccess("Xóa tài khoản thành công");
          refreshAdminAccountData();
          return { success: true };
        } else {
          showError("Xóa tài khoản thất bại");
          return { success: false, error: response.payload as string };
        }
      } catch (error: any) {
        showError("Xóa tài khoản thất bại");
        return { success: false, error: error.message };
      }
    },
    [refreshAdminAccountData]
  );

  // Update admin account
  const updateAccount = useCallback(
    async (payload: UpdateAccountDTO) => {
      try {
        const res = await dispatch(updateAdminAccountThunk(payload));
        if (res.meta.requestStatus === "fulfilled") {
          showSuccess("Cập nhật tài khoản thành công");
          refreshAdminAccountData();
          return { success: true };
        } else {
          showError("Cập nhật tài khoản thất bại");
          return { success: false };
        }
      } catch (error: any) {
        showError("Cập nhật tài khoản thất bại");
        return { success: false, error: error.message };
      }
    },
    [refreshAdminAccountData]
  );

  // Actions
  const actions = useMemo(
    () => ({
      loadAdminAccount: () => dispatch(fetchAllAdminAccount()),
      clearAdminAccountError: () => dispatch(clearAdminAccountError()),
    }),
    [dispatch]
  );

  // Thunks
  const thunks = {
    fetchAllAdminAccount,
  };

  // Loading & error
  const loading = adminAccountState.loading;
  const error = adminAccountState.error;

  return {
    adminAccountData: adminAccountState.data,
    filteredAdminAccount,
    adminAccountStats,
    loading,
    error,
    actions,
    lockHandlers: adminAccountLockHandlers,
    addAdminAccount: handleAddAdminAccount,
    deleteAdminAccount: handleDeleteAdminAccount,
    updateAdminAccount: updateAccount,
    thunks,
  };
};
