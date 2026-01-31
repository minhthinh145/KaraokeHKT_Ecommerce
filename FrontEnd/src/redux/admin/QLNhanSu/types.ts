import type {
  NhanVienDTO,
  LuongCaLamViecDTO,
  CaLamViecDTO,
  LichLamViecDTO,
  YeuCauChuyenCaDTO
} from "../../../api/index";

export interface NhanVienUISubState {
  searchQuery: string;
  filters: {
    loaiNhanVien: string;
    trangThai?: string;
  };
  selectedNhanVien: NhanVienDTO | null;
  showAddModal: boolean;
  showEditModal: boolean;
}

export interface NhanVienSliceState {
  data: NhanVienDTO[];
  loading: boolean;
  error: string | null;
  total: number;
  ui: NhanVienUISubState;
}

// ✨ Thêm UI state cho TienLuong
export interface TienLuongUISubState {
  searchQuery: string;
  filters: {
    selectedCa: number | "ALL";
    dateRange: [string | null, string | null];
  };
  selectedTienLuong: LuongCaLamViecDTO | null;
  showAddModal: boolean;
  showEditModal: boolean;
}

export interface TienLuongSliceState {
  data: LuongCaLamViecDTO[];
  loading: boolean;
  error: string | null;
  total: number;
  current: LuongCaLamViecDTO | null;
  ui: TienLuongUISubState; // ✨ Thêm UI state
}

export interface CaLamViecSliceState {
  data: CaLamViecDTO[];
  current: CaLamViecDTO | null;
  loading: boolean;
  error: string | null;
  lastFetch: number | null;
}

export interface LichLamViecUISubState {
  searchQuery: string;
  filters: {
    selectedNhanVien: string | "ALL";
    dateRange: [string | null, string | null];
  };
  showAddModal: boolean;
  showEditModal: boolean;
  sendingNoti: boolean;
}

export interface LichLamViecSliceState {
  data: LichLamViecDTO[];
  loading: boolean;
  error: string | null;
  total: number;
  current: LichLamViecDTO | null;
  ui: LichLamViecUISubState;
}

export interface PheDuyetYeuCauChuyenCaSliceState {
  all: YeuCauChuyenCaDTO[];
  pending: YeuCauChuyenCaDTO[];
  approved: YeuCauChuyenCaDTO[];
  current?: YeuCauChuyenCaDTO;
  loading: boolean;
  approving: boolean;
  error: string | null;
  ui: {
    search: string;
    statusFilter: "ALL" | "PENDING" | "APPROVED";
  };
}

export const nhanVienInitialState: NhanVienSliceState = {
  data: [],
  loading: false,
  error: null,
  total: 0,
  ui: {
    searchQuery: "",
    filters: { loaiNhanVien: "All" },
    selectedNhanVien: null,
    showAddModal: false,
    showEditModal: false,
  },
};

export const tienLuongInitialState: TienLuongSliceState = {
  data: [],
  loading: false,
  error: null,
  total: 0,
  current: null,
  ui: {
    searchQuery: "",
    filters: {
      selectedCa: "ALL",
      dateRange: [null, null],
    },
    selectedTienLuong: null,
    showAddModal: false,
    showEditModal: false,
  },
};

export const caLamViecInitialState: CaLamViecSliceState = {
  data: [],
  current: null,
  loading: false,
  error: null,
  lastFetch: null,
};

export const lichLamViecInitialState: LichLamViecSliceState = {
  data: [],
  loading: false,
  error: null,
  total: 0,
  current: null,
  ui: {
    searchQuery: "",
    filters: {
      selectedNhanVien: "ALL",
      dateRange: [null, null],
    },
    showAddModal: false,
    showEditModal: false,
    sendingNoti: false,
  },
};

export const pheDuyetYeuCauInitialState: PheDuyetYeuCauChuyenCaSliceState = {
  all: [],
  pending: [],
  approved: [],
  current: undefined,
  loading: false,
  approving: false,
  error: null,
  ui: {
    search: "",
    statusFilter: "ALL",
  },
};

// Domain state = kết quả combineReducers nên không cần qlNhanSuInitialState tổng.
export interface QLNhanSuState {
  nhanVien: NhanVienSliceState;
  tienLuong: TienLuongSliceState;
  caLamViec: CaLamViecSliceState;
  lichLamViec: LichLamViecSliceState;
}
