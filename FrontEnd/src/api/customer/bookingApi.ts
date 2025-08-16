import axiosInstance from "../axiosConfig";
import type { ApiResponse } from "../types";
const BASE = "KhachHangBooking";

// STEP 1 DTO (create booking - invoice draft)
export interface TaoHoaDonPhongResponseDTO {
    maThuePhong: string;
    maHoaDon: string;
    tenPhong: string;
    tenKhachHang: string;
    thoiGianBatDau: string;
    thoiGianKetThucDuKien: string;
    soGioSuDung: number;
    giaPhong: number;
    tongTien: number;
    ngayTao: string;
    hanThanhToan: string;
    trangThai: string;
    ghiChu?: string;
    hoaDonDetail: HoaDonDetailDTO;
}

export interface HoaDonDetailDTO {
    maHoaDon: string;
    tenHoaDon: string;
    moTaHoaDon: string;
    ngayTao: string;
    tongTienHoaDon: number;
    chiTietItems: ChiTietHoaDonDTO[];
}

export interface ChiTietHoaDonDTO {
    tenSanPham: string;
    soLuong: number;
    giaPhong: number;
    thanhTien: number;
}

// Step 1 payload
export interface DatPhongDTO {
    maPhong: number;
    thoiGianBatDau: string; // ISO
    soGioSuDung: number;
    ghiChu?: string;
}

// Step 2 payload
export interface XacNhanThanhToanDTO {
    maHoaDon: string;
    maThuePhong: string;
}

export interface ConfirmPaymentResponse {
    maHoaDon: string;
    maThuePhong: string;
    urlThanhToan: string;
    tongTien: number;
}

export interface RePayResponse {
    maThuePhong: string;
    maHoaDon: string;
    urlThanhToan: string;
    tongTien: number;
}

export interface PhongHatForCustomerDTO {
    maPhong: number;
    tenPhong: string;
    tenLoaiPhong: string;
    sucChua: number;
    hinhAnhPhong?: string;
    giaThueHienTai: number;
    available: boolean;
    moTa?: string;
}

export interface DatPhongResponseDTO {
    maThuePhong: string;
    maHoaDon: string;
    tenPhong: string;
    thoiGianBatDau: string;
    thoiGianKetThucDuKien: string;
    tongTien: number;
    trangThai: string;
    ngayTao: string;
    hanThanhToan?: string;
    urlThanhToan?: string;
}

export interface LichSuDatPhongDTO {
    maThuePhong: string;
    maPhong: number;
    tenPhong: string;
    hinhAnhPhong?: string | null;
    tenLoaiPhong?: string | null;
    thoiGianBatDau: string;
    thoiGianKetThuc?: string | null;
    soGioSuDung: number;
    trangThai: "ChuaThanhToan" | "DaThanhToan" | "DangSuDung" | "DaHoanThanh" | "DaHuy" | "HetHanThanhToan";
    tongTien: number;
    ngayTao: string;
    ngayThanhToan?: string | null;
    maHoaDon?: string | null;
    tenHoaDon?: string | null;
    moTaHoaDon?: string | null;
    hanThanhToan?: string | null;
    daHetHanThanhToan: boolean;
    tenKhachHang?: string | null;
    emailKhachHang?: string | null;
    coTheThanhToanLai: boolean;
    coTheHuy: boolean;
    coTheXacNhanThanhToan: boolean;
    phutConLaiDeThanhToan?: number | null;
}

// Helper build message
const errMsg = (e: any, fallback: string) =>
    e?.response?.data?.message || fallback;

export const getAvailableRooms = async (): Promise<
    ApiResponse<PhongHatForCustomerDTO[] | null>
> => {
    try {
        const res =
            await axiosInstance.get<ApiResponse<PhongHatForCustomerDTO[] | null>>(
                `${BASE}/available-rooms`
            );
        return res.data;
    } catch (e: any) {
        throw new Error(errMsg(e, "Có lỗi xảy ra khi lấy phòng khả dụng"));
    }
};

export const getRoomsByType = async (
    maLoaiPhong: number
): Promise<ApiResponse<PhongHatForCustomerDTO[] | null>> => {
    try {
        const res =
            await axiosInstance.get<ApiResponse<PhongHatForCustomerDTO[] | null>>(
                `${BASE}/rooms-by-type/${maLoaiPhong}`
            );
        return res.data;
    } catch (e: any) {
        throw new Error(errMsg(e, "Có lỗi xảy ra khi lọc theo loại phòng"));
    }
};

export const postBookRoom = async (
    payload: DatPhongDTO
): Promise<ApiResponse<DatPhongResponseDTO | null>> => {
    try {
        const res =
            await axiosInstance.post<ApiResponse<DatPhongResponseDTO | null>>(
                `${BASE}/book-room`,
                payload
            );
        return res.data;
    } catch (e: any) {
        throw new Error(errMsg(e, "Có lỗi xảy ra khi đặt phòng"));
    }
};

export const getBookingHistory = async (
): Promise<ApiResponse<LichSuDatPhongDTO[] | null>> => {
    try {
        const res =
            await axiosInstance.get<ApiResponse<LichSuDatPhongDTO[] | null>>(
                `${BASE}/history`
            );
        return res.data;
    } catch (e: any) {
        throw new Error(errMsg(e, "Có lỗi xảy ra khi lấy lịch sử đặt phòng"));
    }
};

export const cancelBookingApi = async (
    maThuePhong: string
): Promise<ApiResponse<null>> => {
    try {
        const res = await axiosInstance.put<ApiResponse<null>>(
            `${BASE}/cancel/${maThuePhong}`
        );
        return res.data;
    } catch (e: any) {
        throw new Error(errMsg(e, "Có lỗi xảy ra khi hủy đặt phòng"));
    }
};

export const createBookingInvoice = async (
    payload: DatPhongDTO
): Promise<ApiResponse<TaoHoaDonPhongResponseDTO | null>> => {
    try {
        const res =
            await axiosInstance.post<ApiResponse<TaoHoaDonPhongResponseDTO | null>>(
                `${BASE}/create-booking`,
                payload
            );
        return res.data;
    } catch (e: any) {
        throw new Error(errMsg(e, "Có lỗi khi tạo hóa đơn đặt phòng"));
    }
};

export const confirmPaymentApi = async (
    payload: XacNhanThanhToanDTO
): Promise<ApiResponse<ConfirmPaymentResponse | null>> => {
    try {
        const res =
            await axiosInstance.post<ApiResponse<ConfirmPaymentResponse | null>>(
                `${BASE}/confirm-payment`,
                payload
            );
        return res.data;
    } catch (e: any) {
        throw new Error(errMsg(e, "Có lỗi khi tạo URL thanh toán"));
    }
};

export const getUnpaidBookings = async (): Promise<
    ApiResponse<LichSuDatPhongDTO[] | null>
> => {
    try {
        const res = await axiosInstance.get<ApiResponse<LichSuDatPhongDTO[] | null>>(
            `${BASE}/unpaid-bookings`
        );
        return res.data;
    } catch (e: any) {
        throw new Error(errMsg(e, "Có lỗi khi lấy danh sách chưa thanh toán"));
    }
};

export const rePayBookingApi = async (
    maThuePhong: string
): Promise<ApiResponse<RePayResponse | null>> => {
    try {
        const res = await axiosInstance.put<ApiResponse<RePayResponse | null>>(
            `${BASE}/re-pay/${maThuePhong}`
        );
        return res.data;
    } catch (e: any) {
        throw new Error(errMsg(e, "Có lỗi khi thanh toán lại"));
    }
};

// Mapper (server -> camelCase)
export const mapHistoryDto = (s: any): LichSuDatPhongDTO => ({
    maThuePhong: s.maThuePhong,
    maPhong: s.maPhong,
    tenPhong: s.tenPhong,
    hinhAnhPhong: s.hinhAnhPhong,
    tenLoaiPhong: s.tenLoaiPhong,
    thoiGianBatDau: s.thoiGianBatDau,
    thoiGianKetThuc: s.thoiGianKetThuc,
    soGioSuDung: s.soGioSuDung,
    trangThai: s.trangThai,
    tongTien: Number(s.tongTien ?? 0),
    ngayTao: s.ngayTao,
    ngayThanhToan: s.ngayThanhToan,
    maHoaDon: s.maHoaDon,
    tenHoaDon: s.tenHoaDon,
    moTaHoaDon: s.moTaHoaDon,
    hanThanhToan: s.hanThanhToan,
    daHetHanThanhToan: !!s.daHetHanThanhToan,
    tenKhachHang: s.tenKhachHang,
    emailKhachHang: s.emailKhachHang,
    coTheThanhToanLai: !!s.coTheThanhToanLai,
    coTheHuy: !!s.coTheHuy,
    coTheXacNhanThanhToan: !!s.coTheXacNhanThanhToan,
    phutConLaiDeThanhToan: s.phutConLaiDeThanhToan,
});