import { useCallback, useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../redux";
import { useToast } from "../../hooks/useToast";
import {
  selectLichLamViecData,
  selectFilteredLichLamViec,
  selectLichLamViecLoading,
  selectLichLamViecError,
  selectLichLamViecUI,
  selectLichLamViecStats,
  selectLichLamViecCurrent,
  selectLichLamViecShowEditModal,
  selectLichLamViecSlice,
} from "../../redux/admin/QLNhanSu/lichLamViec/selectors";
import {
  fetchAllLichLamViec,
  fetchLichLamViecByNhanVienThunk,
  fetchLichLamViecByRangeThunk,
  createLichLamViecThunk,
  updateLichLamViecThunk,
  deleteLichLamViecThunk,
  sendNotiWorkSchedulesThunk,
} from "../../redux/admin/QLNhanSu/lichLamViec/thunks";
import {
  setSearchQuery,
  setSelectedNhanVien,
  setDateRange,
  clearAllFilters,
  openEditModal,
  closeEditModal,
  setCurrentLichLamViec,
  clearLichLamViecError,
} from "../../redux/admin/QLNhanSu/lichLamViec/slice";
import type { AddLichLamViecDTO, LichLamViecDTO } from "../../api/types";

export const useLichLamViec = ({ autoLoad = true } = {}) => {
  const dispatch = useAppDispatch();
  const { showSuccess, showError } = useToast();

  // State/selectors
  const data = useAppSelector(selectLichLamViecData);
  const filtered = useAppSelector(selectFilteredLichLamViec);
  const loading = useAppSelector(selectLichLamViecLoading);
  const error = useAppSelector(selectLichLamViecError);
  const ui = useAppSelector(selectLichLamViecUI);
  const stats = useAppSelector(selectLichLamViecStats);
  const current = useAppSelector(selectLichLamViecCurrent);
  const showEdit = useAppSelector(selectLichLamViecShowEditModal);
  const slice = useAppSelector(selectLichLamViecSlice);
  const sendingNoti = slice.ui.sendingNoti;

  // Load all once (hoặc bạn tự quyết định dùng filter để load theo range)
  useEffect(() => {
    if (!autoLoad) return;
    dispatch(fetchAllLichLamViec());
  }, [autoLoad, dispatch]);

  // Actions (UI/filters)
  const setSearch = useCallback(
    (q: string) => dispatch(setSearchQuery(q)),
    [dispatch]
  );

  const setNhanVienFilter = useCallback(
    (ma: string | "ALL") => dispatch(setSelectedNhanVien(ma)),
    [dispatch]
  );

  const setDateRangeFilter = useCallback(
    (range: [string | null, string | null]) => dispatch(setDateRange(range)),
    [dispatch]
  );

  const clearFiltersAction = useCallback(
    () => dispatch(clearAllFilters()),
    [dispatch]
  );

  // Fetch theo tiêu chí
  const refresh = useCallback(() => {
    dispatch(fetchAllLichLamViec());
  }, [dispatch]);

  const loadByNhanVien = useCallback(
    (maNhanVien: string) => {
      dispatch(fetchLichLamViecByNhanVienThunk(maNhanVien));
    },
    [dispatch]
  );

  const loadByRange = useCallback(
    (start: string, end: string) => {
      dispatch(fetchLichLamViecByRangeThunk({ start, end }));
    },
    [dispatch]
  );

  // CRUD handlers
  const add = useCallback(
    async (payload: AddLichLamViecDTO) => {
      try {
        const dto = await dispatch(createLichLamViecThunk(payload)).unwrap();
        showSuccess("Thêm lịch làm việc thành công");
        return { success: true, data: dto };
      } catch (e: any) {
        showError(e?.toString?.() || "Thêm lịch làm việc thất bại");
        return { success: false, message: e?.toString?.() };
      }
    },
    [dispatch, showSuccess, showError]
  );

  const update = useCallback(
    async (payload: LichLamViecDTO) => {
      try {
        const dto = await dispatch(updateLichLamViecThunk(payload)).unwrap();
        showSuccess("Cập nhật lịch làm việc thành công");
        return { success: true, data: dto };
      } catch (e: any) {
        showError(e?.toString?.() || "Cập nhật lịch làm việc thất bại");
        return { success: false, message: e?.toString?.() };
      }
    },
    [dispatch, showSuccess, showError]
  );

  const remove = useCallback(
    async (maLichLamViec: number) => {
      try {
        await dispatch(deleteLichLamViecThunk(maLichLamViec)).unwrap();
        showSuccess("Xóa lịch làm việc thành công");
        return { success: true };
      } catch (e: any) {
        showError(e?.toString?.() || "Xóa lịch làm việc thất bại");
        return { success: false, message: e?.toString?.() };
      }
    },
    [dispatch, showSuccess, showError]
  );

  // Edit modal controls (đúng phần LichLamViec)
  const openEditModalAction = useCallback(
    (lichLamViec: LichLamViecDTO) => {
      dispatch(openEditModal(lichLamViec)); // FIX: truyền payload
    },
    [dispatch]
  );

  const closeEditModalAction = useCallback(() => {
    dispatch(closeEditModal());
    dispatch(clearLichLamViecError());
  }, [dispatch]);

  const sendNotiRange = useCallback(
    async (start: string, end: string) => {
      try {
        await dispatch(sendNotiWorkSchedulesThunk({ start, end })).unwrap();
        showSuccess("Đã gửi thông báo lịch làm việc");
        return { success: true };
      } catch (e: any) {
        showError(e?.toString?.() || "Gửi thông báo thất bại");
        return { success: false, message: e?.toString?.() };
      }
    },
    [dispatch, showSuccess, showError]
  );

  return {
    data,
    filtered,
    stats,
    ui,
    loading,
    error,
    refresh,
    loadByNhanVien,
    loadByRange,
    setDateRangeFilter,
    add,
    update,
    remove,
    setSearch,
    setNhanVienFilter,
    clearFiltersAction,
    // Edit modal
    showEdit,
    openEditModalAction,
    closeEditModalAction,
    current,
    sendingNoti,
    sendNotiRange,
  };
};
