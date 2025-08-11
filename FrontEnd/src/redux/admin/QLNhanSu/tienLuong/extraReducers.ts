import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { TienLuongSliceState } from "../types";
import {
  fetchAllTienLuong,
  fetchTienLuongById,
  createTienLuong,
  deleteTienLuong,
} from "./thunks";

export const tienLuongExtraReducers = (
  b: ActionReducerMapBuilder<TienLuongSliceState>
) => {
  b.addCase(fetchAllTienLuong.pending, (s) => {
    s.loading = true;
    s.error = null;
  })
    .addCase(fetchAllTienLuong.fulfilled, (s, a) => {
      s.loading = false;
      s.data = a.payload;
      s.total = a.payload.length;
    })
    .addCase(fetchAllTienLuong.rejected, (s, a) => {
      s.loading = false;
      s.error = (a.payload as string) || a.error.message || "Lỗi tải dữ liệu";
    })
    .addCase(fetchTienLuongById.pending, (s) => {
      s.loading = true;
    })
    .addCase(fetchTienLuongById.fulfilled, (s, a) => {
      s.loading = false;
      s.current = a.payload;
    })
    .addCase(fetchTienLuongById.rejected, (s, a) => {
      s.loading = false;
      s.error = (a.payload as string) || "Không lấy được bản ghi";
    })
    .addCase(createTienLuong.fulfilled, (s, a) => {
      s.data.unshift(a.payload);
      s.total = s.data.length;
      s.current = a.payload;
    })
    .addCase(deleteTienLuong.fulfilled, (s, a) => {
      s.data = s.data.filter((x) => x.maLuongCaLamViec !== a.payload);
      s.total = s.data.length;
      if (s.current?.maLuongCaLamViec === a.payload) s.current = null;
    });
};
