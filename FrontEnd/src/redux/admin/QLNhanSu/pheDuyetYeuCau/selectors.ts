import { createSelector } from "@reduxjs/toolkit";
import type { RootState } from "../../../store";

export const selectPheDuyetYeuCauSlice = (s: RootState) => s.qlNhanSu.pheDuyetYeuCau;
export const selectPheDuyetYeuCauLoading = (s: RootState) =>
    selectPheDuyetYeuCauSlice(s).loading;
export const selectPheDuyetYeuCauApproving = (s: RootState) =>
    selectPheDuyetYeuCauSlice(s).approving;
export const selectPheDuyetYeuCauError = (s: RootState) =>
    selectPheDuyetYeuCauSlice(s).error;
export const selectPheDuyetYeuCauLists = createSelector(
    [selectPheDuyetYeuCauSlice],
    (st) => ({
        all: st.all, // giữ nguyên reference từ state
        pending: st.pending,
        approved: st.approved,
    })
);
export const selectPheDuyetYeuCauCurrent = (s: RootState) =>
    selectPheDuyetYeuCauSlice(s).current;
export const selectPheDuyetYeuCauUI = (s: RootState) => selectPheDuyetYeuCauSlice(s).ui;
export const selectFilteredPheDuyetYeuCau = createSelector(
    [selectPheDuyetYeuCauSlice],
    (st) => {
        const key = st.ui.search.toLowerCase();
        return st.all.filter((x) => {
            const matchText =
                !key ||
                x.tenCaGoc?.toLowerCase().includes(key) ||
                x.tenCaMoi?.toLowerCase().includes(key) ||
                x.tenNhanVien?.toLowerCase().includes(key);
            if (!matchText) return false;
            if (st.ui.statusFilter === "PENDING") return !x.daPheDuyet;
            if (st.ui.statusFilter === "APPROVED") return x.daPheDuyet;
            return true;
        });
    }
);