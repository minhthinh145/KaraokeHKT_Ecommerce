import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { LichLamViecSliceState } from "../types";
import {
  fetchAllLichLamViec,
  fetchLichLamViecByNhanVienThunk,
  createLichLamViecThunk,
  fetchLichLamViecByRangeThunk,
  updateLichLamViecThunk,
  deleteLichLamViecThunk,
  sendNotiWorkSchedulesThunk,
} from "./thunks";

export const lichLamViecExtraReducers = (
  b: ActionReducerMapBuilder<LichLamViecSliceState>
) => {
  b.addCase(fetchAllLichLamViec.pending, (s) => {
    s.loading = true;
    s.error = null;
  })
    .addCase(fetchAllLichLamViec.fulfilled, (s, a) => {
      s.loading = false;
      s.data = a.payload;
      s.total = a.payload.length;
    })
    .addCase(fetchAllLichLamViec.rejected, (s, a) => {
      s.loading = false;
      s.error = (a.payload as string) || a.error.message || "Lỗi tải dữ liệu";
    })
    .addCase(fetchLichLamViecByNhanVienThunk.pending, (s) => {
      s.loading = true;
      s.error = null;
    })
    .addCase(fetchLichLamViecByNhanVienThunk.fulfilled, (s, a) => {
      s.loading = false;
      s.data = a.payload;
      s.total = a.payload.length;
    })
    .addCase(fetchLichLamViecByNhanVienThunk.rejected, (s, a) => {
      s.loading = false;
      s.error =
        (a.payload as string) || a.error.message || "Không lấy được lịch";
    })

    .addCase(fetchLichLamViecByRangeThunk.fulfilled, (s, a) => {
      s.loading = false;
      s.data = a.payload;
      s.total = a.payload.length;
    })
    .addCase(fetchLichLamViecByRangeThunk.rejected, (s, a) => {
      s.loading = false;
      s.error =
        (a.payload as string) ||
        a.error.message ||
        "Không lấy được lịch làm việc theo khoảng ngày";
    })
    .addCase(createLichLamViecThunk.pending, (s) => {
      s.loading = true;
      s.error = null;
    })
    .addCase(createLichLamViecThunk.fulfilled, (s, a) => {
      s.loading = false;
      s.data.unshift(a.payload);
      s.total = s.data.length;
    })
    .addCase(createLichLamViecThunk.rejected, (s, a) => {
      s.loading = false;
      s.error = (a.payload as string) || a.error.message || "Tạo lịch thất bại";
    });
  // Delete: payload là id đã xóa
  // Xóa
  b.addCase(deleteLichLamViecThunk.fulfilled, (s, a) => {
    const id = a.payload;
    s.data = s.data.filter((x) => x.maLichLamViec !== id);
    s.total = s.data.length;
    if (s.current?.maLichLamViec === id) s.current = null;
  });

  // Cập nhật
  b.addCase(updateLichLamViecThunk.fulfilled, (s, a) => {
    const updated = a.payload;
    const idx = s.data.findIndex((d) => d.maLichLamViec === updated.maLichLamViec);
    if (idx !== -1) s.data[idx] = updated;
    if (s.current?.maLichLamViec === updated.maLichLamViec) s.current = updated;
  });

  // Gửi thông báo lịch làm việc theo range
  b.addCase(sendNotiWorkSchedulesThunk.pending, (s) => {
    s.ui.sendingNoti = true;
    s.error = null;
  });
  b.addCase(sendNotiWorkSchedulesThunk.fulfilled, (s) => {
    s.ui.sendingNoti = false;
  });
  b.addCase(sendNotiWorkSchedulesThunk.rejected, (s, a) => {
    s.ui.sendingNoti = false;
    s.error = (a.payload as string) || a.error.message || "Gửi thông báo thất bại";
  });
};
