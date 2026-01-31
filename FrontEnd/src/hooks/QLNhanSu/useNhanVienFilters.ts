import { useCallback } from "react";
import { useAppDispatch, useAppSelector } from "../../redux/hooks";
import {
  setSearchQueryQLNhanSu,
  setLoaiNhanVienFilter,
  setTrangThaiFilterQLNhanSu,
  clearFiltersQLNhanSu,
  selectFilteredNhanVienQLNhanSu,
  selectUIStateQLNhanSu,
} from "../../redux/admin";

export const useNhanVienFilters = () => {
  const dispatch = useAppDispatch();

  const uiState = useAppSelector(selectUIStateQLNhanSu);
  const filteredNhanVien = useAppSelector(selectFilteredNhanVienQLNhanSu);

  const setSearchQuery = useCallback(
    (query: string) => {
      dispatch(setSearchQueryQLNhanSu(query));
    },
    [dispatch]
  );

  const setRoleFilter = useCallback(
    (role: string) => {
      dispatch(setLoaiNhanVienFilter(role));
    },
    [dispatch]
  );

  const setStatusFilter = useCallback(
    (status: string) => {
      dispatch(setTrangThaiFilterQLNhanSu(status));
    },
    [dispatch]
  );

  const clearAllFilters = useCallback(() => {
    dispatch(clearFiltersQLNhanSu());
  }, [dispatch]);

  return {
    filteredNhanVien,
    searchQuery: uiState.searchQuery,
    roleFilter: uiState.filters.loaiNhanVien,
    statusFilter: uiState.filters.trangThai,
    setSearchQuery,
    setRoleFilter,
    setStatusFilter,
    clearAllFilters,
  };
};
