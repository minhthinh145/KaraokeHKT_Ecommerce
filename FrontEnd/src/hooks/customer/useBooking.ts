import { useDispatch, useSelector } from "react-redux";
import { useCallback, useEffect } from "react";
import {
    fetchAvailableRooms,
    fetchBookingHistory,
    fetchRoomsByType,
    bookRoom,
    createBooking,
    confirmPayment,
    cancelBooking,
    fetchUnpaidBookings,
    rePayBooking,
} from "../../redux/customer/booking/thunks";
import {
    selectRooms,
    selectHistory,
    selectBookingUI,
    selectBookingLoading,
    selectBookingCreating,
    selectFilteredRooms,
} from "../../redux/customer/booking/selectors";
import {
    setSearchQuery,
    openBooking,
    closeBooking,
    setFilterLoaiPhong,
    setDurationHours,
    setNote,
    closeInvoice,
    openInvoice,
} from "../../redux/customer/booking/slice";
import type { TaoHoaDonPhongResponseDTO } from "../../api/customer/bookingApi";

export const useBooking = (opts?: { autoLoad?: boolean; maKhachHang?: string }) => {
    const dispatch = useDispatch<any>();
    const rooms = useSelector(selectRooms);
    const filteredRooms = useSelector(selectFilteredRooms);
    const history = useSelector(selectHistory);
    const ui = useSelector(selectBookingUI);
    const loading = useSelector(selectBookingLoading);
    const bookingCreating = useSelector(selectBookingCreating);
    const invoice = useSelector((s: any) => s.customer.booking.lastInvoice);
    const showInvoiceModal = useSelector((s: any) => s.customer.booking.ui.showInvoiceModal);
    const confirmingPayment = useSelector((s: any) => s.customer.booking.confirmingPayment);
    const paymentUrl = useSelector((s: any) => s.customer.booking.ui.bookingRedirectUrl);
    const historyLoading = useSelector((s: any) => s.customer.booking.historyLoading);
    const unpaid = useSelector((s: any) => s.customer.booking.unpaid);
    const unpaidLoading = useSelector((s: any) => s.customer.booking.unpaidLoading);
    const rePayLoading = useSelector((s: any) => s.customer.booking.rePayLoading);

    const refresh = useCallback(() => {
        dispatch(fetchAvailableRooms());
        dispatch(fetchBookingHistory());
    }, [dispatch]);

    useEffect(() => {
        if (opts?.autoLoad) refresh();
    }, [opts?.autoLoad, refresh]);

    // Đảm bảo autoLoad fetch rooms
    useEffect(() => {
        if (opts?.autoLoad) {
            dispatch(fetchAvailableRooms());
        }
    }, [opts?.autoLoad, dispatch]);

    const loadRoomsByType = useCallback((maLoai: number) => {
        dispatch(fetchRoomsByType(maLoai));
    }, [dispatch]);

    const book = useCallback(
        async (p: {
            maPhong: number;
            thoiGianBatDau: string;
            soGioSuDung: number;
            ghiChu?: string;
        }) => {
            try {
                const res = await dispatch(bookRoom(p)).unwrap();
                refresh();
                return { success: true, data: res };
            } catch (e: any) {
                return { success: false, error: e.message };
            }
        },
        [dispatch, refresh]
    );

    const cancel = useCallback(
        async (maThuePhong: string) => {
            try {
                await dispatch(cancelBooking(maThuePhong)).unwrap();
                refresh();
                return { success: true };
            } catch (e: any) {
                return { success: false, error: (e as any).message };
            }
        },
        [dispatch, refresh]
    );

    const create = useCallback(
        async (p: {
            maPhong: number;
            thoiGianBatDau: string;
            soGioSuDung: number;
            ghiChu?: string;
        }) => {
            try {
                const res = await dispatch(createBooking(p)).unwrap();
                return { success: true, data: res as TaoHoaDonPhongResponseDTO };
            } catch (e: any) {
                return { success: false, error: e.message };
            }
        },
        [dispatch]
    );

    const loadHistory = useCallback(() => dispatch(fetchBookingHistory()), [dispatch]);

    const loadUnpaid = useCallback(() => dispatch(fetchUnpaidBookings()), [dispatch]);

    const rePay = useCallback(
        async (id: string) => {
            try {
                const rs = await dispatch(rePayBooking(id)).unwrap();
                return { success: true, data: rs };
            } catch (e: any) {
                return { success: false, error: e.message };
            }
        },
        [dispatch]
    );
    const confirm = useCallback(
        async (dto: { maHoaDon: string; maThuePhong: string }) => {
            try {
                const rs = await dispatch(confirmPayment(dto)).unwrap();
                return { success: true, data: rs };
            } catch (e: any) {
                return { success: false, error: e.message };
            }
        },
        [dispatch]
    );

    return {
        rooms,
        filteredRooms,
        history,
        historyLoading,
        ui,
        invoice,
        showInvoiceModal,
        paymentUrl,
        loading,
        bookingCreating,
        confirmingPayment,
        unpaid,
        unpaidLoading,
        rePayLoading,
        actions: {
            refresh,
            loadRoomsByType,
            book,
            cancel,
            create,
            confirm,
            setSearchQuery: (v: string) => dispatch(setSearchQuery(v)),
            openBooking: (room: any) => dispatch(openBooking(room)),
            closeBooking: () => dispatch(closeBooking()),
            openInvoice: () => dispatch(openInvoice()),
            closeInvoice: () => dispatch(closeInvoice()),
            setFilterLoaiPhong: (v: number | null) => dispatch(setFilterLoaiPhong(v)),
            setDurationHours: (v: number) => dispatch(setDurationHours(v)),
            setNote: (v: string) => dispatch(setNote(v)),
            loadHistory,
            loadUnpaid,
            rePay,

        },
    };
};