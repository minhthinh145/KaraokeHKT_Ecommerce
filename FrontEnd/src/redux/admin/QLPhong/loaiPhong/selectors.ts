import type { RootState } from "../../../store";

export const selectQLPhongDomain = (s: RootState) => s.qlPhong;
export const selectLoaiPhongSlice = (s: RootState) => selectQLPhongDomain(s).loaiPhong;

export const selectLoaiPhongData = (s: RootState) => selectLoaiPhongSlice(s).data;
export const selectLoaiPhongLoading = (s: RootState) => selectLoaiPhongSlice(s).loading;
export const selectLoaiPhongUI = (s: RootState) => selectLoaiPhongSlice(s).ui;