import { useCallback } from "react";
import { useDispatch, useSelector } from "react-redux";
import {
    fetchAllVatLieu,
    createVatLieu as createVatLieuThunk,
    updateSoLuongVatLieu as updateSoLuongThunk,
    updateNgungCungCap as updateNgungCungCapThunk,
} from "../../redux/admin/QLKho";
import {
    selectVatLieuItems,
    selectVatLieuLoading,
    selectVatLieuError,
} from "../../redux/admin/QLKho";
import type { AddVatLieuDTO } from "../../api";
import type { AppDispatch } from "../../redux";

export const useQLVatLieu = () => {
    const dispatch = useDispatch<AppDispatch>();
    const data = useSelector(selectVatLieuItems);
    const loading = useSelector(selectVatLieuLoading);
    const error = useSelector(selectVatLieuError);

    const load = useCallback(() => {
        dispatch(fetchAllVatLieu());
    }, [dispatch]);

    const create = useCallback(async (payload: AddVatLieuDTO) => {
        try {
            await dispatch(createVatLieuThunk(payload)).unwrap();
            return { success: true };
        } catch {
            return { success: false };
        }
    }, [dispatch]);

    const updateSoLuong = useCallback(async (maVatLieu: number, soLuongMoi: number) => {
        try {
            await dispatch(updateSoLuongThunk({ maVatLieu, soLuongMoi })).unwrap();
            return { success: true };
        } catch {
            return { success: false };
        }
    }, [dispatch]);

    const toggleNgungCungCap = useCallback(async (maVatLieu: number, next: boolean) => {
        try {
            await dispatch(updateNgungCungCapThunk({ maVatLieu, ngungCungCap: next })).unwrap();
            return { success: true };
        } catch {
            return { success: false };
        }
    }, [dispatch]);

    return { data, loading, error, load, create, updateSoLuong, toggleNgungCungCap };
};