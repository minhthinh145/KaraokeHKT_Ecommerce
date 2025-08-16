import type { PhongHatForCustomerDTO, LichSuDatPhongDTO, DatPhongResponseDTO } from "../../../api/customer/bookingApi";
import type {
    TaoHoaDonPhongResponseDTO,
    ConfirmPaymentResponse,
} from "../../../api/customer/bookingApi";
import type { RePayResponse } from "../../../api/customer/bookingApi";

export interface BookingUISlice {
    searchQuery: string;
    selectedRoom: PhongHatForCustomerDTO | null;
    showBookingModal: boolean;
    filterLoaiPhong: number | null;
    durationHours: number;
    note: string;
    bookingRedirectUrl: string | null; // đổi
}

export interface BookingState {
    data: PhongHatForCustomerDTO[];
    history: LichSuDatPhongDTO[];
    loading: boolean;
    historyLoading: boolean;
    bookingCreating: boolean;
    confirmingPayment: boolean;
    cancelLoading: boolean;
    error: string | null;
    total: number;
    ui: BookingUISlice & {
        showInvoiceModal: boolean;
    };
    lastInvoice?: TaoHoaDonPhongResponseDTO | null;
    paymentResult?: ConfirmPaymentResponse | null;
    unpaid: LichSuDatPhongDTO[];
    unpaidLoading: boolean;
    rePayLoading: boolean;
}

export const initialBookingState: BookingState = {
    data: [],
    history: [],
    loading: false,
    historyLoading: false,
    bookingCreating: false,
    confirmingPayment: false,
    cancelLoading: false,
    error: null,
    total: 0,
    ui: {
        searchQuery: "",
        selectedRoom: null,
        showBookingModal: false,
        showInvoiceModal: false,
        filterLoaiPhong: null,
        durationHours: 2,
        note: "",
        bookingRedirectUrl: null, // thêm
    },
    lastInvoice: null,
    paymentResult: null,
    unpaid: [],
    unpaidLoading: false,
    rePayLoading: false,
};