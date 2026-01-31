import { createSelector } from "@reduxjs/toolkit";
import dayjs from "dayjs";
import type { RootState } from "../../../store";
import type { LichLamViecDTO } from "../../../../api";

export const selectLichLamViecSlice = (s: RootState) => s.qlNhanSu.lichLamViec;
export const selectLichLamViecData = createSelector(
  [selectLichLamViecSlice],
  (sl) => sl.data
);
export const selectLichLamViecLoading = createSelector(
  [selectLichLamViecSlice],
  (sl) => sl.loading
);
export const selectLichLamViecError = createSelector(
  [selectLichLamViecSlice],
  (sl) => sl.error
);
export const selectLichLamViecUI = createSelector(
  [selectLichLamViecSlice],
  (sl) => sl.ui
);
export const selectFilteredLichLamViec = createSelector(
  [selectLichLamViecData, selectLichLamViecUI],
  (data, ui) => {
    const q = ui.searchQuery.trim().toLowerCase();
    const [start, end] = ui.filters.dateRange;
    const selected = ui.filters.selectedNhanVien;

    return data.filter((d: LichLamViecDTO) => {
      // search theo tên nhân viên (nếu có)
      if (q) {
        const ok =
          (d.tenNhanVien || "").toLowerCase().includes(q) ||
          (d.loaiNhanVien || "").toLowerCase().includes(q);
        if (!ok) return false;
      }

      // filter theo nhân viên
      if (selected !== "ALL" && d.maNhanVien !== selected) return false;

      // filter theo ngày
      if (start || end) {
        const date = dayjs(d.ngayLamViec);
        const startD = start ? dayjs(start) : null;
        const endD = end ? dayjs(end) : null;

        if (startD && date.isBefore(startD, "day")) return false;
        if (endD && date.isAfter(endD, "day")) return false;
      }

      return true;
    });
  }
);
export const selectLichLamViecStats = createSelector(
  [selectLichLamViecData],
  (data) => ({ total: data.length })
);

export const selectLichLamViecCurrent = createSelector(
  [selectLichLamViecSlice],
  (sl) => sl.current
);

export const selectLichLamViecShowEditModal = createSelector(
  [selectLichLamViecSlice],
  (sl) => sl.ui.showEditModal
);