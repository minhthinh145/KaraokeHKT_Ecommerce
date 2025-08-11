import type { RootState } from "../../../store";
import { createSelector } from "@reduxjs/toolkit";

export const selectCaLamViecSlice = (state: RootState) =>
  state.qlNhanSu.caLamViec;

export const selectCaLamViecData = createSelector(
  [selectCaLamViecSlice],
  (caLamViec) => caLamViec.data
);

export const selectCaLamViecLoading = createSelector(
  [selectCaLamViecSlice],
  (caLamViec) => caLamViec.loading
);

export const selectCaLamViecError = createSelector(
  [selectCaLamViecSlice],
  (caLamViec) => caLamViec.error
);
export const selectCaLamViecCurrent = createSelector(
  [selectCaLamViecSlice],
  (caLamViec) => caLamViec.current
);

// Utility selectors
export const selectCaLamViecMap = createSelector(
  [selectCaLamViecData],
  (data) =>
    data.reduce<Record<number, string>>((acc, ca) => {
      acc[ca.maCa] = ca.tenCa;
      return acc;
    }, {})
);

export const selectCaLamViecOptions = createSelector(
  [selectCaLamViecData],
  (data) =>
    data.map((ca) => ({
      label: `${ca.tenCa} (${ca.gioBatDauCa} - ${ca.gioKetThucCa})`,
      value: ca.maCa,
    }))
);

export const selectCaLamViecById = (maCa: number) =>
  createSelector([selectCaLamViecData], (data) =>
    data.find((ca) => ca.maCa === maCa)
  );
