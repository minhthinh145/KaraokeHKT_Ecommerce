import { useDispatch, useSelector } from "react-redux";
import { useCallback, useEffect } from "react";
import type { AppDispatch } from "../../redux/store";
import {
    fetchAllYeuCauChuyenCa,
    fetchPendingYeuCauChuyenCa,
    fetchApprovedYeuCauChuyenCa,
    fetchYeuCauChuyenCaDetail,
    approveYeuCauChuyenCa,
    selectPheDuyetYeuCauLoading,
    selectPheDuyetYeuCauApproving,
    selectPheDuyetYeuCauError,
    selectPheDuyetYeuCauLists,
    selectPheDuyetYeuCauCurrent,
    selectPheDuyetYeuCauUI,
    selectFilteredPheDuyetYeuCau,
    setSearchYeuCau,
    setStatusFilterYeuCau,
    clearPheDuyetYeuCauError,
    fetchAllLichLamViec,
} from "../../redux/admin/QLNhanSu";
import { useToast } from "../useToast";

export const usePheDuyetYeuCauChuyenCa = () => {
    const dispatch = useDispatch<AppDispatch>();
    const loading = useSelector(selectPheDuyetYeuCauLoading);
    const approving = useSelector(selectPheDuyetYeuCauApproving);
    const error = useSelector(selectPheDuyetYeuCauError);
    const lists = useSelector(selectPheDuyetYeuCauLists);
    const current = useSelector(selectPheDuyetYeuCauCurrent);
    const ui = useSelector(selectPheDuyetYeuCauUI);
    const filtered = useSelector(selectFilteredPheDuyetYeuCau);
    const { showSuccess, showError } = useToast?.() || { showSuccess: () => { }, showError: () => { } };

    const loadAll = useCallback(() => {
        dispatch(fetchAllYeuCauChuyenCa());
        dispatch(fetchPendingYeuCauChuyenCa());
        dispatch(fetchApprovedYeuCauChuyenCa());
    }, [dispatch]);

    const loadDetail = (id: number) => dispatch(fetchYeuCauChuyenCaDetail(id));

    // Approve / Reject (ketQua true = duyệt, false = từ chối)
    const approve = useCallback(
        async (p: { maYeuCau: number; ghiChu?: string; ketQua?: boolean }) => {
            try {
                const payload = await dispatch(approveYeuCauChuyenCa({
                    maYeuCau: p.maYeuCau,
                    ghiChu: p.ghiChu || "",
                    ketQua: p.ketQua ?? true,
                })).unwrap();

                if (payload.isSuccess) {
                    showSuccess(p.ketQua === false ? "Từ chối thành công" : "Phê duyệt thành công");
                    dispatch(fetchAllLichLamViec());
                    await loadAll();

                }
                return { success: payload.isSuccess, data: payload.data ?? null };
            } catch (e: any) {
                showError("Xử lý yêu cầu thất bại");
                return { success: false, error: e?.message || String(e) };
            }
        },
        [dispatch, showSuccess, showError]
    );

    const setSearch = (v: string) => dispatch(setSearchYeuCau(v));
    const setStatusFilter = (v: "ALL" | "PENDING" | "APPROVED") =>
        dispatch(setStatusFilterYeuCau(v));
    const clearError = () => dispatch(clearPheDuyetYeuCauError());

    useEffect(() => {
        loadAll();
    }, [loadAll]);

    return {
        loading,
        approving,
        error,
        lists,
        current,
        ui,
        filtered,
        loadAll,
        loadDetail,
        approve,
        setSearch,
        setStatusFilter,
        clearError,
    };
};