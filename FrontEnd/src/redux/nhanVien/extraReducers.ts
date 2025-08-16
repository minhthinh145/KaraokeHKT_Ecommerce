import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type { NhanVienDomainState } from "./types";
import {
    fetchNhanVienProfile,
    fetchScheduleNV,
    fetchShiftChanges,
    createShiftChange,
    deleteShiftChange,
} from "./thunks";

export const nvRegisterExtra = (b: ActionReducerMapBuilder<NhanVienDomainState>) => {
    b
        // profile
        .addCase(fetchNhanVienProfile.pending, (s) => {
            s.profileLoading = true;
            s.profileError = null;
        })
        .addCase(fetchNhanVienProfile.fulfilled, (s, a) => {
            s.profileLoading = false;
            s.profile = a.payload;
        })
        .addCase(fetchNhanVienProfile.rejected, (s, a) => {
            s.profileLoading = false;
            s.profileError = (a.payload as string) || a.error.message || null;
        })
        // schedule
        .addCase(fetchScheduleNV.pending, (s) => {
            s.schedule.loading = true;
            s.schedule.error = null;
        })
        .addCase(fetchScheduleNV.fulfilled, (s, a) => {
            s.schedule.loading = false;
            s.schedule.data = a.payload;
        })
        .addCase(fetchScheduleNV.rejected, (s, a) => {
            s.schedule.loading = false;
            s.schedule.error = (a.payload as string) || "Lỗi";
        })
        // shift changes
        .addCase(fetchShiftChanges.pending, (s) => {
            s.shiftChanges.loading = true;
            s.shiftChanges.error = null;
        })
        .addCase(fetchShiftChanges.fulfilled, (s, a) => {
            s.shiftChanges.loading = false;
            s.shiftChanges.data = a.payload;
        })
        .addCase(fetchShiftChanges.rejected, (s, a) => {
            s.shiftChanges.loading = false;
            s.shiftChanges.error = (a.payload as string) || "Lỗi";
        })
        // create
        .addCase(createShiftChange.pending, (s) => {
            s.shiftChanges.ui.creating = true;
        })
        .addCase(createShiftChange.fulfilled, (s, a) => {
            s.shiftChanges.ui.creating = false;
            s.shiftChanges.data.unshift(a.payload);
        })
        .addCase(createShiftChange.rejected, (s) => {
            s.shiftChanges.ui.creating = false;
        })
        // delete
        .addCase(deleteShiftChange.pending, (s, a) => {
            s.shiftChanges.ui.deletingId = a.meta.arg.maYeuCau;
        })
        .addCase(deleteShiftChange.fulfilled, (s, a) => {
            s.shiftChanges.ui.deletingId = null;
            s.shiftChanges.data = s.shiftChanges.data.filter(
                (x) => x.maYeuCau !== a.payload
            );
        })
        .addCase(deleteShiftChange.rejected, (s) => {
            s.shiftChanges.ui.deletingId = null;
        });
};