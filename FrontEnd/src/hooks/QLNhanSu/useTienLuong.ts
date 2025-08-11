import { useCallback, useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../redux/hooks";
import {
  fetchAllTienLuong,
  createTienLuong,
  deleteTienLuong,
  setCurrentTienLuong,
  clearTienLuongError,
  clearTienLuongCurrent,
  resetTienLuongState,
  // UI Actions
  setSearchQueryTienLuong,
  setSelectedCaTienLuong,
  setDateRangeTienLuong,
  openAddModalTienLuong,
  closeAddModalTienLuong,
  openEditModalTienLuong,
  closeEditModalTienLuong,
  clearAllFiltersTienLuong,
} from "../../redux/admin/QLNhanSu";
import {
  selectTienLuongData,
  selectTienLuongLoading,
  selectTienLuongError,
  selectCurrentTienLuong,
  selectTienLuongUI,
  selectFilteredTienLuong,
  selectDefaultLuongCardsData,
  selectTienLuongStats,
} from "../../redux/admin/QLNhanSu/tienLuong/selectors";
import type { AddLuongCaLamViecDTO, LuongCaLamViecDTO } from "../../api/types";
import { useToast } from "../useToast";

interface UseTienLuongOptions {
  autoLoad?: boolean;
}

export const useTienLuong = ({
  autoLoad = false,
}: UseTienLuongOptions = {}) => {
  const dispatch = useAppDispatch();

  // Selectors
  const tienLuongData = useAppSelector(selectTienLuongData);
  const loading = useAppSelector(selectTienLuongLoading);
  const error = useAppSelector(selectTienLuongError);
  const current = useAppSelector(selectCurrentTienLuong);
  const ui = useAppSelector(selectTienLuongUI);
  const filteredTienLuong = useAppSelector(selectFilteredTienLuong);
  const defaultLuongCardsData = useAppSelector(selectDefaultLuongCardsData);
  const tienLuongStats = useAppSelector(selectTienLuongStats);
  const { showSuccess, showError } = useToast();
  // Data Actions
  const refreshTienLuongData = useCallback(() => {
    dispatch(fetchAllTienLuong());
  }, [dispatch]);

  const addTienLuong = useCallback(
    async (payload: AddLuongCaLamViecDTO) => {
      try {
        const res = await dispatch(createTienLuong(payload));
        showSuccess("Thêm lương ca làm việc thành công");
        return { success: true, data: res };
      } catch (error) {
        showError("Thêm lương ca làm việc thất bại");
        return { success: false };
      }
    },
    [dispatch, showSuccess, showError]
  );

  const removeTienLuong = useCallback(
    async (maLuongCaLamViec: number) => {
      try {
        const res = await dispatch(deleteTienLuong(maLuongCaLamViec));
        showSuccess("Xóa lương ca làm việc thành công");
        return { success: true, data: res };
      } catch (error) {
        showError("Xóa lương ca làm việc thất bại");
        return { success: false };
      }
    },
    [dispatch, showSuccess, showError]
  );

  // State Actions
  const setCurrent = useCallback(
    (tienLuong: LuongCaLamViecDTO) => {
      dispatch(setCurrentTienLuong(tienLuong));
    },
    [dispatch]
  );

  const clearError = useCallback(() => {
    dispatch(clearTienLuongError());
  }, [dispatch]);

  const clearCurrent = useCallback(() => {
    dispatch(clearTienLuongCurrent());
  }, [dispatch]);

  const resetState = useCallback(() => {
    dispatch(resetTienLuongState());
  }, [dispatch]);

  // UI Actions
  const setSearch = useCallback(
    (query: string) => {
      dispatch(setSearchQueryTienLuong(query));
    },
    [dispatch]
  );

  const setCaFilter = useCallback(
    (ca: number | "ALL") => {
      dispatch(setSelectedCaTienLuong(ca));
    },
    [dispatch]
  );

  const setDateRangeFilter = useCallback(
    (range: [string | null, string | null]) => {
      dispatch(setDateRangeTienLuong(range));
    },
    [dispatch]
  );

  const openAdd = useCallback(() => {
    dispatch(openAddModalTienLuong());
  }, [dispatch]);

  const closeAdd = useCallback(() => {
    dispatch(closeAddModalTienLuong());
  }, [dispatch]);

  const openEdit = useCallback(
    (tienLuong: LuongCaLamViecDTO) => {
      dispatch(openEditModalTienLuong(tienLuong));
    },
    [dispatch]
  );

  const closeEdit = useCallback(() => {
    dispatch(closeEditModalTienLuong());
  }, [dispatch]);

  const clearFilters = useCallback(() => {
    dispatch(clearAllFiltersTienLuong());
  }, [dispatch]);

  // Auto load
  useEffect(() => {
    if (autoLoad) {
      refreshTienLuongData();
    }
  }, [autoLoad, refreshTienLuongData]);

  return {
    // Data
    tienLuongData,
    filteredTienLuong,
    defaultLuongCardsData,
    tienLuongStats,
    current,
    loading,
    error,

    // UI State
    ui,

    // Data Actions
    refreshTienLuongData,
    addTienLuong,
    removeTienLuong,
    setCurrent,
    clearError,
    clearCurrent,
    resetState,

    // UI Actions
    setSearch,
    setCaFilter,
    setDateRangeFilter,
    openAdd,
    closeAdd,
    openEdit,
    closeEdit,
    clearFilters,
  };
};
