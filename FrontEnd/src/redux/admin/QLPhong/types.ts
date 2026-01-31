import type {
    LoaiPhongDTO,
    AddLoaiPhongDTO,
    UpdateLoaiPhongDTO,
    AddPhongHatDTO,
    UpdatePhongHatDTO,
    PhongHatDetailDTO,
} from "../../../api/types";

// -------- Loại Phòng --------
export interface LoaiPhongUISubState {
    searchQuery: string;
    showAddModal: boolean;
    showEditModal: boolean;
    selected: LoaiPhongDTO | null;
}
export interface LoaiPhongSliceState {
    data: LoaiPhongDTO[];
    loading: boolean;
    error: string | null;
    total: number;
    ui: LoaiPhongUISubState;
}
export const loaiPhongInitialState: LoaiPhongSliceState = {
    data: [],
    loading: false,
    error: null,
    total: 0,
    ui: { searchQuery: "", showAddModal: false, showEditModal: false, selected: null },
};

// -------- Phòng Hát --------
export type PhongHatStatusFilter = "ALL" | "ACTIVE" | "INACTIVE";

export interface PhongHatUISubState {
    searchQuery: string;
    statusFilter: PhongHatStatusFilter;
    showAddModal: boolean;
    showEditModal: boolean;
    selected: PhongHatDetailDTO | null;
}

export interface PhongHatSliceState {
    data: PhongHatDetailDTO[];
    loading: boolean;
    error: string | null;
    total: number;
    ui: PhongHatUISubState;
}

export const phongHatInitialState: PhongHatSliceState = {
    data: [],
    loading: false,
    error: null,
    total: 0,
    ui: {
        searchQuery: "",
        statusFilter: "ALL",
        showAddModal: false,
        showEditModal: false,
        selected: null,
    },
};

// -------- Domain Root --------
export interface QLPhongState {
    loaiPhong: LoaiPhongSliceState;
    phongHat: PhongHatSliceState;
}

export type {
    LoaiPhongDTO,
    AddLoaiPhongDTO,
    UpdateLoaiPhongDTO,
    AddPhongHatDTO,
    UpdatePhongHatDTO,
    PhongHatDetailDTO,
};