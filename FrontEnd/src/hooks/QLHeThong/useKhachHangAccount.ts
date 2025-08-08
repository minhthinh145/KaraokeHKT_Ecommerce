import { useCallback, useEffect, useMemo } from "react";
import { useAppDispatch, useAppSelector } from "../../redux/hooks";
import {
  fetchAllKhachHang,
  clearKhachHangError,
  selectKhachHangState,
  selectFilteredKhachHang,
  selectKhachHangStats,
} from "../../redux/admin";
import { useAccountLockToggle } from "../shared/useLockableAccount";
import { useToast } from "../useToast";

export const useKhachHangAccount = ({ autoLoad = true } = {}) => {
  const dispatch = useAppDispatch();

  // Selectors
  const khachHangState = useAppSelector(selectKhachHangState);
  const filteredKhachHang = useAppSelector(selectFilteredKhachHang);
  const khachHangStats = useAppSelector(selectKhachHangStats);
  // Refresh callback
  const refreshKhachHangData = useCallback(() => {
    dispatch(fetchAllKhachHang());
  }, [dispatch]);

  // Lock/unlock handlers
  const khachHangLockHandlers = useAccountLockToggle();

  // Actions
  const actions = useMemo(
    () => ({
      loadKhachHang: () => dispatch(fetchAllKhachHang()),
      clearKhachHangError: () => dispatch(clearKhachHangError()),
    }),
    [dispatch]
  );

  // Loading & error
  const loading = khachHangState.loading;
  const error = khachHangState.error;
  //thunks
  const thunks = {
    fetchAllKhachHang,
  };
  return {
    khachHangData: khachHangState.data,
    filteredKhachHang,
    khachHangStats,
    loading,
    error,
    actions,
    lockHandlers: khachHangLockHandlers,
    thunks,
  };
};
