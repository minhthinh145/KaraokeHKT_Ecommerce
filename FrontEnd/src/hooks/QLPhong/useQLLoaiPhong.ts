import { useCallback, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import type { AppDispatch } from "../../redux/store";
import {
    fetchAllLoaiPhongThunk,
    createLoaiPhongThunk,
    updateLoaiPhongThunk,
    selectLoaiPhongData,
    selectLoaiPhongLoading,
    selectLoaiPhongUI,
    openAddLoaiPhongModal,
    closeAddLoaiPhongModal,
    openEditLoaiPhongModal,
    closeEditLoaiPhongModal,
    setLoaiPhongSearchQuery,
    type LoaiPhongDTO,
    type AddLoaiPhongDTO,
    type UpdateLoaiPhongDTO,
} from "../../redux/admin/QLPhong";
import { useToast } from "../useToast";

export const useQLLoaiPhong = ({ autoLoad = true } = {}) => {
    const dispatch = useDispatch<AppDispatch>();
    const data = useSelector(selectLoaiPhongData);
    const loading = useSelector(selectLoaiPhongLoading);
    const ui = useSelector(selectLoaiPhongUI);
    const { showSuccess, showError } = useToast();

    // Định nghĩa refresh giống useNhanVien
    const refresh = useCallback(() => {
        dispatch(fetchAllLoaiPhongThunk());
    }, [dispatch]);

    useEffect(() => {
        if (autoLoad) {
            refresh();
        }
    }, [autoLoad, refresh]);

    const add = useCallback(
        async (p: AddLoaiPhongDTO) => {
            try {
                await dispatch(createLoaiPhongThunk(p)).unwrap();
                showSuccess("Thêm loại phòng thành công");
                // refresh(); // KHÔNG gọi lại nếu extraReducers đã cập nhật state
                return { success: true };
            } catch (e: any) {
                showError(e?.message || "Thêm loại phòng thất bại");
                return { success: false };
            }
        },
        [dispatch, showSuccess, showError]
    );

    const update = useCallback(
        async (p: UpdateLoaiPhongDTO) => {
            try {
                await dispatch(updateLoaiPhongThunk(p)).unwrap();
                showSuccess("Cập nhật loại phòng thành công");
                // refresh();
                return { success: true };
            } catch (e: any) {
                showError(e?.message || "Cập nhật loại phòng thất bại");
                return { success: false };
            }
        },
        [dispatch, showSuccess, showError]
    );

    return {
        data,
        loading,
        ui,
        refresh,
        add,
        update,
        setSearchQuery: (q: string) => dispatch(setLoaiPhongSearchQuery(q)),
        openAdd: () => dispatch(openAddLoaiPhongModal()),
        closeAdd: () => dispatch(closeAddLoaiPhongModal()),
        openEdit: (row: LoaiPhongDTO) => dispatch(openEditLoaiPhongModal(row)),
        closeEdit: () => dispatch(closeEditLoaiPhongModal()),
    };
};