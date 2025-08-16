import type { RootState } from "../../../store";

export const selectVatLieuState = (s: RootState) => s.qlKho.vatLieu;
export const selectVatLieuItems = (s: RootState) => s.qlKho.vatLieu.items;
export const selectVatLieuLoading = (s: RootState) => s.qlKho.vatLieu.loading;
export const selectVatLieuError = (s: RootState) => s.qlKho.vatLieu.error;
export const selectVatLieuUI = (s: RootState) => s.qlKho.vatLieu.ui;