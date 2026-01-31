import { useDispatch, useSelector } from "react-redux";
import { useEffect, useCallback } from "react";
import {
    fetchScheduleNV,
    fetchShiftChanges,
    createShiftChange,
    deleteShiftChange,
    selectNVScheduleState,
    selectNVShiftChangeState,
} from "../../redux/nhanVien";
import {
    setDraggingLich,
    setShiftSearch,
} from "../../redux/nhanVien/slice";

export const useNhanVienAll = (
    maNhanVien: string,
    options?: { autoLoad?: boolean }
) => {
    const dispatch = useDispatch<any>();
    const scheduleState = useSelector(selectNVScheduleState);
    const shiftState = useSelector(selectNVShiftChangeState);

    useEffect(() => {
        if (maNhanVien && options?.autoLoad) {
            dispatch(fetchScheduleNV({ maNhanVien }));
            dispatch(fetchShiftChanges({ maNhanVien }));
        }
    }, [maNhanVien, options?.autoLoad, dispatch]);

    const refreshSchedule = useCallback(
        () => dispatch(fetchScheduleNV({ maNhanVien })),
        [dispatch, maNhanVien]
    );
    const beginDrag = (id: number) => dispatch(setDraggingLich(id));
    const endDrag = () => dispatch(setDraggingLich(null));

    const refreshShiftChanges = useCallback(
        () => dispatch(fetchShiftChanges({ maNhanVien })),
        [dispatch, maNhanVien]
    );
    const setSearchQuery = (q: string) => dispatch(setShiftSearch(q));

    const create = async (payload: any) => {
        const r = await dispatch(createShiftChange(payload));
        return { success: !r.error, data: (r as any).payload };
    };
    const remove = async (maYeuCau: number) => {
        const r = await dispatch(deleteShiftChange({ maYeuCau }));
        return { success: !r.error };
    };

    return {
        schedule: {
            data: scheduleState.data,
            loading: scheduleState.loading,
            error: scheduleState.error,
            draggingLichId: scheduleState.ui.draggingLichId,
            refresh: refreshSchedule,
            beginDrag,
            endDrag,
        },
        shiftChanges: {
            data: shiftState.data,
            loading: shiftState.loading,
            error: shiftState.error,
            deletingId: shiftState.ui.deletingId,
            searchQuery: shiftState.ui.searchQuery,
            setSearchQuery,
            create,
            remove,
            refresh: refreshShiftChanges,
        },
        refreshAll: () => {
            dispatch(fetchScheduleNV({ maNhanVien }));
            dispatch(fetchShiftChanges({ maNhanVien }));
        },
    };
};