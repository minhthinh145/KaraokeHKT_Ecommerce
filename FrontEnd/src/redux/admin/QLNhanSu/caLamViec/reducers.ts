import type { CaLamViecDTO } from "../../../../api";
import type { CaLamViecSliceState } from "../types";
import type { PayloadAction } from "@reduxjs/toolkit";

export const caLamViecReducers = {
  setCurrentCaLamViec: (
    state: CaLamViecSliceState,
    action: PayloadAction<CaLamViecDTO>
  ) => {
    state.current = action.payload;
  },
  clearCaLamViecError: (state: CaLamViecSliceState) => {
    state.error = null;
  },
  clearCaLamViecCurrent: (state: CaLamViecSliceState) => {
    state.current = null;
  },
  resetCaLamViecState(): CaLamViecSliceState {
    return {
      data: [],
      current: null,
      loading: false,
      error: null,
      lastFetch: null,
    };
  },
};
