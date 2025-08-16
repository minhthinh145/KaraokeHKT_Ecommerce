import axiosInstance from "../../../axiosConfig";
import type { AddLoaiPhongDTO, LoaiPhongDTO, UpdateLoaiPhongDTO } from "../../../types/admins/QLPhong";
import type { ApiResponse } from "../../shared";

const BASE = "QLLoaiPhong";

export const apiGetAllLoaiPhong = async (): Promise<ApiResponse<LoaiPhongDTO[]>> => {
    try {
        const res = await axiosInstance.get(`${BASE}/GetAllLoaiPhong`);
        return res.data as ApiResponse<LoaiPhongDTO[]>;
    } catch (error: any) {
        throw new Error(error.response?.data?.message || "Có lỗi xảy ra khi lấy danh sách loại phòng");
    }
};

export const apiGetLoaiPhong = async (maLoaiPhong: number): Promise<ApiResponse<LoaiPhongDTO>> => {
    try {
        const res = await axiosInstance.get(`${BASE}/GetLoaiPhong/${maLoaiPhong}`);
        return res.data as ApiResponse<LoaiPhongDTO>;
    } catch (error: any) {
        throw new Error(error.response?.data?.message || "Có lỗi xảy ra khi lấy loại phòng");
    }
};

export const apiCreateLoaiPhong = async (data: AddLoaiPhongDTO): Promise<ApiResponse<LoaiPhongDTO>> => {
    try {
        const res = await axiosInstance.post(`${BASE}/CreateLoaiPhong`, data);
        return res.data as ApiResponse<LoaiPhongDTO>;
    } catch (error: any) {
        throw new Error(error.response?.data?.message || "Có lỗi xảy ra khi tạo loại phòng");
    }
};

export const apiUpdateLoaiPhong = async (data: UpdateLoaiPhongDTO): Promise<ApiResponse<LoaiPhongDTO>> => {
    try {
        const res = await axiosInstance.put(`${BASE}/UpdateLoaiPhong`, data);
        return res.data as ApiResponse<LoaiPhongDTO>;
    } catch (error: any) {
        throw new Error(error.response?.data?.message || "Có lỗi xảy ra khi cập nhật loại phòng");
    }
};