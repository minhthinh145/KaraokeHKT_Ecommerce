import { useCallback, useMemo, useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../redux/hooks";
import {
  fetchAllNhanVienQLHeThong,
  createNhanVienQLHeThong,
  clearNhanVienErrorQLHeThong,
  selectNhanVienStateQLHeThong,
  selectFilteredNhanVienQLHeThong,
  selectNhanVienStatsQLHeThong,
} from "../../redux/admin";
import { useAccountLockToggle } from "../shared/useLockableAccount";
import { message } from "antd";
import type { AddTaiKhoanForNhanVienDTO } from "../../api/types";
import { useToast } from "../useToast";

export const useNhanVienAccount = ({ autoLoad = true } = {}) => {
  const dispatch = useAppDispatch();

  // Selectors
  const nhanVienState = useAppSelector(selectNhanVienStateQLHeThong);
  const filteredNhanVien = useAppSelector(selectFilteredNhanVienQLHeThong);
  const nhanVienStats = useAppSelector(selectNhanVienStatsQLHeThong);
  const { showSuccess, showError } = useToast();
  // Refresh callback
  const refreshNhanVienAccountData = useCallback(() => {
    dispatch(fetchAllNhanVienQLHeThong());
  }, [dispatch]);

  // Lock/unlock handlers
  const nhanVienLockHandlers = useAccountLockToggle();

  // Add nhân viên account logic
  const handleAddNhanVienAccount = useCallback(
    async (data: AddTaiKhoanForNhanVienDTO) => {
      try {
        const result = await dispatch(createNhanVienQLHeThong(data));
        if (result.meta.requestStatus === "fulfilled") {
          showSuccess("Tạo tài khoản nhân viên thành công!");
          return { success: true, data: result.payload };
        } else {
          showError("Tạo tài khoản thất bại!");
          return { success: false, error: result.payload as string };
        }
      } catch (error: any) {
        showError(error.message || "Có lỗi xảy ra!");
        return { success: false, error: error.message };
      }
    },
    [dispatch]
  );

  // Actions
  const actions = useMemo(
    () => ({
      loadNhanVienAccount: () => dispatch(fetchAllNhanVienQLHeThong()),
      clearNhanVienError: () => dispatch(clearNhanVienErrorQLHeThong()),
    }),
    [dispatch]
  );
  //thunks
  const thunks = {
    fetchAllNhanVienQLHeThong,
  };
  // Loading & error
  const loading = nhanVienState.loading;
  const error = nhanVienState.error;

  return {
    nhanVienData: nhanVienState.data,
    filteredNhanVien,
    nhanVienStats,
    loading,
    error,
    actions,
    lockHandlers: nhanVienLockHandlers,
    addNhanVienAccount: handleAddNhanVienAccount,
    thunks,
  };
};
