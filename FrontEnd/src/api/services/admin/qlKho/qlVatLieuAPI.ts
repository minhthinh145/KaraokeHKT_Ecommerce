import axiosInstance from "../../../axiosConfig";
import type { ApiResponse } from "../../../types";
import type { AddVatLieuDTO, UpdateSoLuongDTO, VatLieuDetailDTO, UpdateVatLieuDTO } from "../../../types";

const BASE = "QLVatLieu";

export const getAllVatLieu = async (): Promise<ApiResponse<VatLieuDetailDTO[]>> => {
    try {
        const res = await axiosInstance.get(`${BASE}/GetAllVatLieu`);
        return res.data as ApiResponse<VatLieuDetailDTO[]>;
    } catch (error: any) {
        throw new Error(
            error.response?.data?.message ||
            "Có lỗi xảy ra khi lấy danh sách vật liệu"
        );
    }
};

export const getVatLieuDetail = async (maVatLieu: number): Promise<ApiResponse<VatLieuDetailDTO>> => {
    try {
        const res = await axiosInstance.get(`${BASE}/GetVatLieuDetail/${maVatLieu}`);
        return res.data as ApiResponse<VatLieuDetailDTO>;
    } catch (error: any) {
        throw new Error(
            error.response?.data?.message ||
            "Có lỗi xảy ra khi lấy chi tiết vật liệu"
        );
    }
};

export const createVatLieu = async (payload: AddVatLieuDTO): Promise<ApiResponse<VatLieuDetailDTO>> => {
    try {
        const res = await axiosInstance.post(`${BASE}/CreateVatLieu`, payload);
        return res.data as ApiResponse<VatLieuDetailDTO>;
    } catch (error: any) {
        throw new Error(
            error.response?.data?.message ||
            "Có lỗi xảy ra khi tạo vật liệu"
        );
    }
};

export const updateSoLuongVatLieu = async (payload: UpdateSoLuongDTO): Promise<ApiResponse<null>> => {
    try {
        const res = await axiosInstance.put(`${BASE}/UpdateSoLuong`, payload);
        return res.data as ApiResponse<null>;
    } catch (error: any) {
        throw new Error(
            error.response?.data?.message ||
            "Có lỗi xảy ra khi cập nhật số lượng vật liệu"
        );
    }
};

export const updateNgungCungCap = async (
    maVatLieu: number,
    ngungCungCap: boolean
): Promise<ApiResponse<null>> => {
    try {
        const res = await axiosInstance.put(
            `${BASE}/UpdateNgungCungCap/${maVatLieu}`,
            null,
            { params: { ngungCungCap } }
        );
        return res.data as ApiResponse<null>;
    } catch (error: any) {
        throw new Error(
            error.response?.data?.message ||
            "Có lỗi xảy ra khi cập nhật trạng thái vật liệu"
        );
    }
};
export const updateVatLieu = async (payload: UpdateVatLieuDTO): Promise<ApiResponse<VatLieuDetailDTO>> => {
    try {
        const res = await axiosInstance.put(`${BASE}/UpdateVatLieu`, payload);
        return res.data as ApiResponse<VatLieuDetailDTO>;
    } catch (error: any) {
        throw new Error(
            error.response?.data?.message || "Có lỗi xảy ra khi cập nhật vật liệu"
        );
    }
};