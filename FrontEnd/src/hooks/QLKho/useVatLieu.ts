import { useCallback } from "react";
import { useDispatch, useSelector } from "react-redux";
import type { AppDispatch } from "../../redux/store";
import {
    fetchAllVatLieu,
    createVatLieu as createVatLieuThunk,
    updateSoLuongVatLieu as updateSoLuongThunk,
    updateNgungCungCap as updateNgungCungCapThunk,
    selectVatLieuItems,
    selectVatLieuLoading,
    selectVatLieuUI,
    setVatLieuShowCreateModal,
    setVatLieuShowUpdateSoLuongModal,
    setVatLieuSelected,
    updateVatLieu as updateVatLieuThunk,
    setVatLieuShowEditModal,
} from "../../redux/admin/QLKho";
import type { AddVatLieuDTO } from "../../api/index";
import type { UpdateVatLieuDTO } from "../../api";
import { useToast } from "../useToast";
export const useVatLieu = () => {
    const dispatch = useDispatch<AppDispatch>();
    const data = useSelector(selectVatLieuItems);
    const loading = useSelector(selectVatLieuLoading);
    const ui = useSelector(selectVatLieuUI);
    const { showSuccess, showError } = useToast();
    const load = useCallback(() => { dispatch(fetchAllVatLieu()); }, [dispatch]);

    const openCreate = useCallback(() => dispatch(setVatLieuShowCreateModal(true)), [dispatch]);
    const closeCreate = useCallback(() => dispatch(setVatLieuShowCreateModal(false)), [dispatch]);

    const openUpdateSL = useCallback((row: any) => {
        dispatch(setVatLieuSelected(row));
        dispatch(setVatLieuShowUpdateSoLuongModal(true));
    }, [dispatch]);
    const closeUpdateSL = useCallback(() => {
        dispatch(setVatLieuShowUpdateSoLuongModal(false));
        dispatch(setVatLieuSelected(null));
    }, [dispatch]);

    const openEdit = useCallback((row: any) => {
        dispatch(setVatLieuSelected(row));
        dispatch(setVatLieuShowEditModal(true));
    }, [dispatch]);
    const closeEdit = useCallback(() => {
        dispatch(setVatLieuShowEditModal(false));
        dispatch(setVatLieuSelected(null));
    }, [dispatch]);

    const create = useCallback(async (payload: AddVatLieuDTO) => {
        try {
            await dispatch(createVatLieuThunk(payload)).unwrap();
            await dispatch(fetchAllVatLieu());
            showSuccess("Thêm vật liệu thành công!");
            return { success: true };
        } catch (e: any) {
            showError(e?.toString?.() || "Thêm vật liệu thất bại!");
            return { success: false, message: e?.toString?.() };
        }
    }, [dispatch, showSuccess, showError]);

    const updateSoLuong = useCallback(async (maVatLieu: number, soLuongMoi: number) => {
        try {
            await dispatch(updateSoLuongThunk({ maVatLieu, soLuongMoi })).unwrap();
            await dispatch(fetchAllVatLieu());
            showSuccess("Cập nhật số lượng thành công!");
            return { success: true };
        } catch (e: any) {
            showError(e?.toString?.() || "Cập nhật số lượng thất bại!");
            return { success: false, message: e?.toString?.() };
        }
    }, [dispatch, showSuccess, showError]);

    const toggleNgungCungCap = useCallback(async (maVatLieu: number, next: boolean) => {
        try {
            await dispatch(updateNgungCungCapThunk({ maVatLieu, ngungCungCap: next })).unwrap();
            await dispatch(fetchAllVatLieu());
            showSuccess(next ? "Đã ngừng cung cấp vật liệu!" : "Đã tiếp tục cung cấp vật liệu!");
            return { success: true };
        } catch (e: any) {
            showError(e?.toString?.() || "Cập nhật trạng thái vật liệu thất bại!");
            return { success: false, message: e?.toString?.() };
        }
    }, [dispatch, showSuccess, showError]);

    const update = useCallback(async (payload: UpdateVatLieuDTO) => {
        try {
            await dispatch(updateVatLieuThunk(payload)).unwrap();
            await dispatch(fetchAllVatLieu());
            showSuccess("Cập nhật vật liệu thành công!");
            return { success: true };
        } catch (e: any) {
            showError(e?.toString?.() || "Cập nhật vật liệu thất bại!");
            return { success: false, message: e?.toString?.() };
        }
    }, [dispatch, showSuccess, showError]);

    return {
        data, loading, ui,
        load,
        openCreate, closeCreate,
        openUpdateSL, closeUpdateSL,
        openEdit, closeEdit,
        create, updateSoLuong, toggleNgungCungCap, update,
    };
};