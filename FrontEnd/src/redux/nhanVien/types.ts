import type {
    LichLamViecDTO,
    YeuCauChuyenCaDTO,
    NhanVienTaiKhoanDTO,
} from "../../api"; // profile DTO (tái dùng)
import type {
    AddYeuCauChuyenCaDTO,
} from "../../api"; // nếu bạn có file khác, chỉnh lại path

// Schedule (Lịch làm việc) state
export interface NVSchedulerState {
    data: LichLamViecDTO[];
    loading: boolean;
    error: string | null;
    ui: {
        searchQuery: string;
        draggingLichId: number | null;
        targetDate: string | null;
        targetCa: number | null;
    };
}

// Shift change (Yêu cầu chuyển ca) state
export interface NVShiftChangeState {
    data: YeuCauChuyenCaDTO[];
    loading: boolean;
    error: string | null;
    ui: {
        searchQuery: string;
        creating: boolean;
        deletingId: number | null;
    };
}

// Root employee self-service domain
export interface NhanVienDomainState {
    schedule: NVSchedulerState;
    shiftChanges: NVShiftChangeState;
    profile: NhanVienTaiKhoanDTO | null;
    profileLoading: boolean;
    profileError: string | null;
}

// Initials
export const nvScheduleInitial: NVSchedulerState = {
    data: [],
    loading: false,
    error: null,
    ui: {
        searchQuery: "",
        draggingLichId: null,
        targetDate: null,
        targetCa: null,
    },
};

export const nvShiftInitial: NVShiftChangeState = {
    data: [],
    loading: false,
    error: null,
    ui: {
        searchQuery: "",
        creating: false,
        deletingId: null,
    },
};

export const nvRootInitial: NhanVienDomainState = {
    schedule: nvScheduleInitial,
    shiftChanges: nvShiftInitial,
    profile: null,
    profileLoading: false,
    profileError: null,
};

// Payload helper (nếu cần tạo yêu cầu chuyển ca)
export type CreateShiftChangePayload = AddYeuCauChuyenCaDTO;

