import { useCallback, useMemo } from "react";
import { useAppDispatch, useAppSelector } from "../../redux/hooks";
import {
  fetchLoaiTaiKhoan,
  clearLoaiTaiKhoanError,
  selectLoaiTaiKhoanState,
} from "../../redux/admin";

export const useLoaiTaiKhoan = ({ autoLoad = true } = {}) => {
  const dispatch = useAppDispatch();

  // Selectors
  const loaiTaiKhoanState = useAppSelector(selectLoaiTaiKhoanState);

  // Refresh callback
  const refreshLoaiTaiKhoanData = useCallback(() => {
    dispatch(fetchLoaiTaiKhoan());
  }, [dispatch]);

  // Actions
  const actions = useMemo(
    () => ({
      loadLoaiTaiKhoan: () => dispatch(fetchLoaiTaiKhoan()),
      clearLoaiTaiKhoanError: () => dispatch(clearLoaiTaiKhoanError()),
    }),
    [dispatch]
  );

  // Thunks
  const thunks = {
    fetchLoaiTaiKhoan,
  };

  // Loading & error
  const loading = loaiTaiKhoanState.loading;
  const error = loaiTaiKhoanState.error;

  return {
    loaiTaiKhoanData: loaiTaiKhoanState.data,
    loading,
    error,
    actions,
    thunks,
    refreshLoaiTaiKhoanData, // ✅ Để consistency với các hook khác
  };
};
