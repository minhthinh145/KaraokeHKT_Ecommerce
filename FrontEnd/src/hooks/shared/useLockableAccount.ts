import { useCallback } from "react";
import { useAppDispatch } from "../../redux";
import {
  lockAccountThunk,
  unlockAccountThunk,
} from "../../redux/admin/QLHeThong/thunks";
import { useToast } from "../useToast";

export const useAccountLockToggle = () => {
  const dispatch = useAppDispatch();
  const { showSuccess, showError } = useToast();

  const handleLockAccount = useCallback(
    async (maTaiKhoan: string) => {
      try {
        await dispatch(lockAccountThunk(maTaiKhoan)).unwrap();
        showSuccess("Đã khóa tài khoản");
        return { success: true };
      } catch (error: any) {
        showError(error?.message || "Có lỗi xảy ra khi khóa tài khoản");
        return { success: false, error };
      }
    },
    [dispatch, showSuccess, showError]
  );

  const handleUnlockAccount = useCallback(
    async (maTaiKhoan: string) => {
      try {
        await dispatch(unlockAccountThunk(maTaiKhoan)).unwrap();
        showSuccess("Đã mở khóa tài khoản");
        return { success: true };
      } catch (error: any) {
        showError(error?.message || "Có lỗi xảy ra khi mở khóa tài khoản");
        return { success: false, error };
      }
    },
    [dispatch, showSuccess, showError]
  );

  const handleLockToggle = useCallback(
    async (maTaiKhoan: string, isCurrentlyLocked: boolean) => {
      return isCurrentlyLocked
        ? await handleUnlockAccount(maTaiKhoan)
        : await handleLockAccount(maTaiKhoan);
    },
    [handleLockAccount, handleUnlockAccount]
  );

  return {
    lockAccount: handleLockAccount,
    unlockAccount: handleUnlockAccount,
    lockToggle: handleLockToggle,
  };
};
