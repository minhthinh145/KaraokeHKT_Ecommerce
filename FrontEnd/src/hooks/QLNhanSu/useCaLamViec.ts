import { useCallback } from "react";
import { useAppDispatch, useAppSelector } from "../../redux/hooks";
import {
  fetchAllCaLamViec,
  createCaLamViecThunk,
  setCurrentCaLamViec,
  clearCaLamViecError,
  clearCaLamViecCurrent,
  resetCaLamViecState,
} from "../../redux/admin/QLNhanSu";
import type { AddCaLamViecDTO, CaLamViecDTO } from "../../api";
import {
  selectCaLamViecData,
  selectCaLamViecLoading,
  selectCaLamViecError,
  selectCaLamViecCurrent,
  selectCaLamViecMap,
  selectCaLamViecOptions,
} from "../../redux/admin/QLNhanSu";

export const useCaLamViec = () => {
  const dispatch = useAppDispatch();

  // Selectors
  const caLamViecList = useAppSelector(selectCaLamViecData);
  const loading = useAppSelector(selectCaLamViecLoading);
  const error = useAppSelector(selectCaLamViecError);
  const current = useAppSelector(selectCaLamViecCurrent);
  const caMap = useAppSelector(selectCaLamViecMap);
  const caOptions = useAppSelector(selectCaLamViecOptions);

  // Actions
  const refreshCaLamViec = useCallback(() => {
    dispatch(fetchAllCaLamViec());
  }, [dispatch]);

  const addCaLamViec = useCallback(
    async (payload: AddCaLamViecDTO) => {
      const res = await dispatch(createCaLamViecThunk(payload));
      return res;
    },
    [dispatch]
  );

  const setCurrent = useCallback(
    (ca: CaLamViecDTO) => {
      dispatch(setCurrentCaLamViec(ca));
    },
    [dispatch]
  );

  const clearError = useCallback(() => {
    dispatch(clearCaLamViecError());
  }, [dispatch]);

  const clearCurrent = useCallback(() => {
    dispatch(clearCaLamViecCurrent());
  }, [dispatch]);

  const resetState = useCallback(() => {
    dispatch(resetCaLamViecState());
  }, [dispatch]);

  return {
    caLamViecList,
    loading,
    error,
    current,
    caMap,
    caOptions,
    refreshCaLamViec,
    addCaLamViec,
    setCurrent,
    clearError,
    clearCurrent,
    resetState,
  };
};
