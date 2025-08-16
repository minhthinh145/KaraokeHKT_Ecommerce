import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import type {
    YeuCauChuyenCaDTO,
} from "../../../../api";
import type {
    PheDuyetYeuCauChuyenCaSliceState,
    pheDuyetYeuCauInitialState,
} from "../types";
import {
    fetchAllYeuCauChuyenCa,
    fetchPendingYeuCauChuyenCa,
    fetchApprovedYeuCauChuyenCa,
    fetchYeuCauChuyenCaDetail,
    approveYeuCauChuyenCa,
} from "./thunks";

export const buildExtraReducers = (b: ActionReducerMapBuilder<PheDuyetYeuCauChuyenCaSliceState>) => {
    b.addCase(fetchAllYeuCauChuyenCa.pending, (s) => {
        s.loading = true;
        s.error = null;
    })
        .addCase(fetchAllYeuCauChuyenCa.fulfilled, (s, a) => {
            s.loading = false;
            s.all = a.payload ?? [];
        })
        .addCase(fetchAllYeuCauChuyenCa.rejected, (s, a) => {
            s.loading = false;
            s.error = a.payload ?? a.error.message ?? null;
        })
        .addCase(fetchPendingYeuCauChuyenCa.fulfilled, (s, a) => {
            s.pending = a.payload ?? [];
        })
        .addCase(fetchApprovedYeuCauChuyenCa.fulfilled, (s, a) => {
            s.approved = a.payload ?? [];
        })
        .addCase(fetchYeuCauChuyenCaDetail.fulfilled, (s, a) => {
            s.current = a.payload;
        })
        .addCase(approveYeuCauChuyenCa.pending, (s) => {
            s.approving = true;
        })
        .addCase(approveYeuCauChuyenCa.fulfilled, (s, a) => {
            s.approving = false;
            const updated = a.payload as YeuCauChuyenCaDTO;
            s.all = s.all.map((x) => (x.maYeuCau === updated.maYeuCau ? updated : x));
            s.pending = s.pending.filter((x) => x.maYeuCau !== updated.maYeuCau);
            if (updated.daPheDuyet) {
                s.approved = [updated, ...s.approved.filter((x) => x.maYeuCau !== updated.maYeuCau)];
            }
            s.current = updated;
        })
        .addCase(approveYeuCauChuyenCa.rejected, (s, a) => {
            s.approving = false;
            s.error = a.payload ?? a.error.message ?? null;
        });
};