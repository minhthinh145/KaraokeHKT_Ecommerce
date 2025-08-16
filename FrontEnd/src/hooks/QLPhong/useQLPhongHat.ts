import { useCallback, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import type { AppDispatch } from "../../redux/store";
import {
    fetchAllPhongHatThunk,
    createPhongHatThunk,
    updatePhongHatThunk,
    toggleNgungHoatDongThunk,
    toggleDangSuDungThunk,
    selectPhongHatData,
    selectPhongHatLoading,
    selectPhongHatUI,
    selectPhongHatFiltered,
    setPhongHatSearchQuery,
    setPhongHatStatusFilter,
    openAddPhongHatModal,
    closeAddPhongHatModal,
    openEditPhongHatModal,
    closeEditPhongHatModal,
    type AddPhongHatDTO,
    type UpdatePhongHatDTO,
    type PhongHatDetailDTO,
} from "../../redux/admin/QLPhong";
import { useToast } from "../useToast";

export const useQLPhongHat = ({ autoLoad = true } = {}) => {
    const dispatch = useDispatch<AppDispatch>();
    const data = useSelector(selectPhongHatData);
    const filtered = useSelector(selectPhongHatFiltered);
    const ui = useSelector(selectPhongHatUI);
    const loading = useSelector(selectPhongHatLoading);
    const { showSuccess, showError } = useToast();

    const refresh = useCallback(() => {
        dispatch(fetchAllPhongHatThunk());
    }, [dispatch]);

    useEffect(() => {
        if (autoLoad) refresh();
    }, [autoLoad, refresh]);

    const add = useCallback(async (p: AddPhongHatDTO) => {
        try {
            await dispatch(createPhongHatThunk(p)).unwrap();
            refresh();
            showSuccess("Thêm phòng hát thành công");
            return { success: true };
        } catch (e: any) {
            showError(e.message || "Thêm phòng thất bại");
            return { success: false };
        }
    }, [dispatch, showSuccess, showError]);

    const update = useCallback(async (p: UpdatePhongHatDTO) => {
        try {
            await dispatch(updatePhongHatThunk(p)).unwrap();
            refresh();

            showSuccess("Cập nhật phòng hát thành công");
            return { success: true };
        } catch (e: any) {
            showError(e.message || "Cập nhật phòng thất bại");
            return { success: false };
        }
    }, [dispatch, showSuccess, showError]);

    const toggleNgung = useCallback(async (row: PhongHatDetailDTO, next: boolean) => {
        try {
            await dispatch(toggleNgungHoatDongThunk({ maPhong: row.maPhong, ngungHoatDong: next })).unwrap();
            refresh();

            showSuccess(next ? "Đã ngừng hoạt động" : "Đã kích hoạt lại");
            return { success: true };
        } catch (e: any) {
            showError(e.message || "Thao tác thất bại");
            return { success: false };
        }
    }, [dispatch, showSuccess, showError]);

    const toggleDangSuDung = useCallback(async (row: PhongHatDetailDTO, next: boolean) => {
        try {
            await dispatch(toggleDangSuDungThunk({ maPhong: row.maPhong, dangSuDung: next })).unwrap();
            refresh();

            return { success: true };
        } catch (e: any) {
            return { success: false };
        }
    }, [dispatch]);

    return {
        data,
        filtered,
        ui,
        loading,
        refresh,
        add,
        update,
        toggleNgung,
        toggleDangSuDung,
        setSearch: (q: string) => dispatch(setPhongHatSearchQuery(q)),
        setStatusFilter: (f: any) => dispatch(setPhongHatStatusFilter(f)),
        openAdd: () => dispatch(openAddPhongHatModal()),
        closeAdd: () => dispatch(closeAddPhongHatModal()),
        openEdit: (r: PhongHatDetailDTO) => dispatch(openEditPhongHatModal(r)),
        closeEdit: () => dispatch(closeEditPhongHatModal()),
    };
};