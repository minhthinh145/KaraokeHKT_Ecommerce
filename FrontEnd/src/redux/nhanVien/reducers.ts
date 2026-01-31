import { createReducer } from "@reduxjs/toolkit";
import {
    fetchScheduleNV,
    fetchShiftChanges,
    createShiftChange,
    deleteShiftChange,
} from "./thunks";
import type { PayloadAction } from "@reduxjs/toolkit";
import type {
    NVSchedulerState,
    NVShiftChangeState,
} from "./types";
import type { LichLamViecDTO, YeuCauChuyenCaDTO } from "../../api/types/admins/QLNhanSutypes";

// Nếu bạn cần ShiftChangeItem, dùng YeuCauChuyenCaDTO thay thế

export const initialScheduleState: NVSchedulerState = {
    data: [],
    loading: false,
    error: null,
    ui: {
        searchQuery: "",
        draggingLichId: null,
        targetDate: null,
        targetCa: null,
    },
};

export const initialShiftChangeState: NVShiftChangeState = {
    data: [],
    loading: false,
    error: null,
    ui: {
        searchQuery: "",
        creating: false,
        deletingId: null,
    },
};

export const scheduleReducer = createReducer(initialScheduleState, (b) => {
    b.addCase(fetchScheduleNV.pending, (s) => {
        s.loading = true;
        s.error = null;
    })
        .addCase(fetchScheduleNV.fulfilled, (s, a) => {
            s.loading = false;
            s.data = a.payload;
        })
        .addCase(fetchScheduleNV.rejected, (s, a) => {
            s.loading = false;
            s.error = (a.payload as string) || "Lỗi";
        });
});

export const shiftChangeReducer = createReducer(
    initialShiftChangeState,
    (b) => {
        b.addCase(fetchShiftChanges.pending, (s) => {
            s.loading = true;
            s.error = null;
        })
            .addCase(fetchShiftChanges.fulfilled, (s, a) => {
                s.loading = false;
                s.data = a.payload;
            })
            .addCase(fetchShiftChanges.rejected, (s, a) => {
                s.loading = false;
                s.error = (a.payload as string) || "Lỗi";
            })
            .addCase(createShiftChange.pending, (s) => {
                s.ui.creating = true;
            })
            .addCase(createShiftChange.fulfilled, (s, a) => {
                s.ui.creating = false;
                s.data.unshift(a.payload as YeuCauChuyenCaDTO);
            })
            .addCase(createShiftChange.rejected, (s) => {
                s.ui.creating = false;
            })
            .addCase(deleteShiftChange.pending, (s, a) => {
                s.ui.deletingId = a.meta.arg.maYeuCau;
            })
            .addCase(deleteShiftChange.fulfilled, (s, a) => {
                s.ui.deletingId = null;
                s.data = s.data.filter(
                    (x: YeuCauChuyenCaDTO) => x.maYeuCau !== a.payload
                );
            })
            .addCase(deleteShiftChange.rejected, (s) => {
                s.ui.deletingId = null;
            });
    }
);

// Reducers cho slice (dùng trực tiếp trong createSlice)
export const nvScheduleReducers = {
    setScheduleSearch(s: NVSchedulerState, a: PayloadAction<string>) {
        s.ui.searchQuery = a.payload;
    },
    setDraggingLich(s: NVSchedulerState, a: PayloadAction<number | null>) {
        s.ui.draggingLichId = a.payload;
    },
};

export const nvShiftReducers = {
    setShiftSearch(s: NVShiftChangeState, a: PayloadAction<string>) {
        s.ui.searchQuery = a.payload;
    },
    prependShiftChange(s: NVShiftChangeState, a: PayloadAction<YeuCauChuyenCaDTO>) {
        s.data.unshift(a.payload);
    },
    clearShiftError(s: NVShiftChangeState) {
        s.error = null;
    },
    replaceShiftList(
        s: NVShiftChangeState,
        a: PayloadAction<YeuCauChuyenCaDTO[]>
    ) {
        s.data = a.payload;
    },
    // Nếu muốn replaceSchedule cho root, hãy dispatch ở root slice, không ở shift
};