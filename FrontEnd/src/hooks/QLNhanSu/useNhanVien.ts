import { useCallback, useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../redux/hooks";
import {
  fetchAllNhanVienQLNhanSu,
  createNhanVienQLNhanSu,
  updateNhanVienQLNhanSu,
  removeNhanVienFromList,
  clearNhanVienErrorQLNhanSu,
  selectNhanVienStateQLNhanSu,
  selectNhanVienStatsQLNhanSu,
} from "../../redux/admin";
import { useToast } from "../useToast";
import type { AddNhanVienDTO, NhanVienDTO } from "../../api/services/shared";

export const useNhanVien = ({ autoLoad = true } = {}) => {
  const dispatch = useAppDispatch();
  const { showSuccess, showError } = useToast();

  const nhanVienState = useAppSelector(selectNhanVienStateQLNhanSu);
  const nhanVienStats = useAppSelector(selectNhanVienStatsQLNhanSu);
  const loading = nhanVienState.loading;
  const error = nhanVienState.error;

  // Load data
  const refreshNhanVienData = useCallback(() => {
    dispatch(fetchAllNhanVienQLNhanSu());
  }, [dispatch]);

  useEffect(() => {
    if (autoLoad) {
      refreshNhanVienData();
    }
  }, [autoLoad, refreshNhanVienData]);

  // Add nhân viên
  const addNhanVien = useCallback(
    async (payload: AddNhanVienDTO) => {
      try {
        const res = await dispatch(createNhanVienQLNhanSu(payload)).unwrap();
        showSuccess("Tạo nhân viên thành công");
        return { success: true, data: res };
      } catch (error: any) {
        showError("Tạo nhân viên thất bại");
        return { success: false, error: error.message };
      }
    },
    [dispatch, showSuccess, showError]
  );

  // Update nhân viên
  const updateNhanVien = useCallback(
    async (payload: NhanVienDTO) => {
      try {
        const res = await dispatch(updateNhanVienQLNhanSu(payload)).unwrap();
        showSuccess("Cập nhật nhân viên thành công");
        return { success: true, data: res };
      } catch (error: any) {
        showError("Cập nhật nhân viên thất bại");
        return { success: false, error: error.message };
      }
    },
    [dispatch, showSuccess, showError]
  );

  // Delete nhân viên (sử dụng removeNhanVienFromList có sẵn)
  const deleteNhanVien = useCallback(
    async (maNhanVien: string) => {
      try {
        // Nếu có API delete thì gọi trước, sau đó remove khỏi list
        dispatch(removeNhanVienFromList(maNhanVien));
        showSuccess("Xóa nhân viên thành công");
        return { success: true };
      } catch (error: any) {
        showError("Xóa nhân viên thất bại");
        return { success: false, error: error.message };
      }
    },
    [dispatch, showSuccess, showError]
  );

  // Clear error
  const clearError = useCallback(() => {
    dispatch(clearNhanVienErrorQLNhanSu());
  }, [dispatch]);

  return {
    nhanVienData: nhanVienState.data,
    nhanVienStats,
    loading,
    error,
    addNhanVien,
    updateNhanVien,
    deleteNhanVien,
    refreshNhanVienData,
    clearError,
  };
};
