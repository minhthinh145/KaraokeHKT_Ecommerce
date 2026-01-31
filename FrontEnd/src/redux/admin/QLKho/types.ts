import type { VatLieuDetailDTO } from "../../../api/index";

export interface VatLieuUISubState {
  searchQuery: string;
  filters: { ngungCungCap: "ALL" | "TRUE" | "FALSE" };
  showCreateModal: boolean;
  showUpdateSoLuongModal: boolean;
  showEditModal: boolean; // thêm
  selected?: VatLieuDetailDTO | null;
}

export interface VatLieuSliceState {
  items: VatLieuDetailDTO[];
  loading: boolean;
  error: string | null;
  total: number;
  ui: VatLieuUISubState;
}

export const vatLieuInitialState: VatLieuSliceState = {
  items: [],
  loading: false,
  error: null,
  total: 0,
  ui: {
    searchQuery: "",
    filters: { ngungCungCap: "ALL" },
    showCreateModal: false,
    showUpdateSoLuongModal: false,
    showEditModal: false, // thêm
    selected: null,
  },
};

export interface QLKhoState {
  vatLieu: VatLieuSliceState;
}