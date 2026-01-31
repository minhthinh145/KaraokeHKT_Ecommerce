import type { PhongHatForCustomerDTO } from "../../../api";
import type { PagedResult } from "../../../api/customer/bookingApi";

export interface BookingUIState {
    searchQuery: string;
    filterLoaiPhong: number | null;
    selectedRoom: PhongHatForCustomerDTO | null;
    showBookingModal: boolean;
    showInvoiceModal: boolean;
    durationHours: number;
    note: string;
    bookingRedirectUrl: string | null;
}

export interface BookingState {
    // Original data array (for compatibility)
    data: PhongHatForCustomerDTO[];

    // Paged data from new API
    rooms: PagedResult<PhongHatForCustomerDTO> | null;

    // History
    history: any[];

    // Unpaid bookings
    unpaid: any[];

    // Loading states
    loading: boolean;
    bookingCreating: boolean;
    historyLoading: boolean;
    confirmingPayment: boolean;
    unpaidLoading: boolean;

    // Error states
    error: string | null;

    // UI state
    ui: BookingUIState;

    // Pagination for new API
    availableRoomsPaged: any;
    availableRoomsLoading: boolean;
    availableRoomsError: string | null;
    availableRoomsPage: number;
    availableRoomsPageSize: number;

    // Other states
    invoice: any;
    lastInvoice: any;
    paymentUrl: string | null;
    rePayLoading: boolean;
}

export const initialBookingState: BookingState = {
    data: [],
    rooms: null,
    history: [],
    unpaid: [],
    loading: false,
    bookingCreating: false,
    historyLoading: false,
    confirmingPayment: false,
    unpaidLoading: false,
    error: null,
    ui: {
        searchQuery: "",
        filterLoaiPhong: null,
        selectedRoom: null,
        showBookingModal: false,
        showInvoiceModal: false,
        durationHours: 2,
        note: "",
        bookingRedirectUrl: null,
    },
    availableRoomsPaged: null,
    availableRoomsLoading: false,
    availableRoomsError: null,
    availableRoomsPage: 1,
    availableRoomsPageSize: 10,
    invoice: null,
    lastInvoice: null,
    paymentUrl: null,
    rePayLoading: false,
};