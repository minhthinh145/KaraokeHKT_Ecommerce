import type { RootState } from "../../../store";
import { createSelector } from "@reduxjs/toolkit";
import dayjs from "dayjs";
import isBetween from "dayjs/plugin/isBetween";
import isSameOrBefore from "dayjs/plugin/isSameOrBefore";
import isSameOrAfter from "dayjs/plugin/isSameOrAfter";

dayjs.extend(isBetween);
dayjs.extend(isSameOrBefore);
dayjs.extend(isSameOrAfter);

export const selectTienLuongSlice = (state: RootState) =>
  state.qlNhanSu.tienLuong;

export const selectTienLuongData = createSelector(
  [selectTienLuongSlice],
  (tienLuong) => tienLuong.data
);

export const selectTienLuongLoading = createSelector(
  [selectTienLuongSlice],
  (tienLuong) => tienLuong.loading
);

export const selectTienLuongError = createSelector(
  [selectTienLuongSlice],
  (tienLuong) => tienLuong.error
);

export const selectCurrentTienLuong = createSelector(
  [selectTienLuongSlice],
  (tienLuong) => tienLuong.current
);

export const selectTienLuongUI = createSelector(
  [selectTienLuongSlice],
  (tienLuong) => tienLuong.ui
);

// ✨ Filtered data selector
export const selectFilteredTienLuong = createSelector(
  [selectTienLuongData, selectTienLuongUI],
  (data, ui) => {
    let filtered = data;

    // Lọc theo search
    if (ui.searchQuery) {
      const query = ui.searchQuery.toLowerCase();
      filtered = filtered.filter(
        (item) =>
          item.tenCaLamViec?.toLowerCase().includes(query) ||
          item.giaCa?.toString().includes(query)
      );
    }

    // Lọc theo ca
    if (ui.filters.selectedCa !== "ALL") {
      filtered = filtered.filter((item) => item.maCa === ui.filters.selectedCa);
    }

    // ✨ LOGIC LỌC THEO NGÀY - OVERLAP
    const [startDate, endDate] = ui.filters.dateRange;
    if (startDate && endDate) {
      const filterStart = dayjs(startDate);
      const filterEnd = dayjs(endDate);

      filtered = filtered.filter((item) => {
        // Nếu không có ngày áp dụng thì bỏ qua
        if (!item.ngayApDung) return false;

        const itemStart = dayjs(item.ngayApDung);

        // Nếu không có ngày kết thúc, chỉ kiểm tra ngày áp dụng
        if (!item.ngayKetThuc) {
          return itemStart.isBetween(filterStart, filterEnd, "day", "[]");
        }

        const itemEnd = dayjs(item.ngayKetThuc);

        // ✅ LOGIC OVERLAP: Có bất kỳ ngày nào trong khoảng item overlap với filter
        // Item: [itemStart ---- itemEnd]
        // Filter: [filterStart ---- filterEnd]
        // Overlap khi: itemStart <= filterEnd && itemEnd >= filterStart
        return (
          itemStart.isSameOrBefore(filterEnd, "day") &&
          itemEnd.isSameOrAfter(filterStart, "day")
        );
      });
    }

    return filtered;
  }
);

// ✨ Default luong cards data
export const selectDefaultLuongCardsData = createSelector(
  [selectTienLuongData],
  (data) => data.filter((d) => d.isDefault)
);

// ✨ Stats selector
export const selectTienLuongStats = createSelector(
  [selectTienLuongData],
  (data) => ({
    total: data.length,
    defaultCount: data.filter((d) => d.isDefault).length,
    specialCount: data.filter((d) => !d.isDefault).length,
    totalValue: data.reduce((sum, item) => sum + (item.giaCa || 0), 0),
  })
);
