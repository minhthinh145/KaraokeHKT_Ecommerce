import axiosInstance from "../../../axiosConfig";
import type { ApiResponse } from "../../shared";
import type {
    AddPhongHatDTO,
    UpdatePhongHatDTO,
    PhongHatDetailDTO,
} from "../../../types";

const BASE = "QLPhongHat";

export const apiGetAllPhongHat = async (): Promise<ApiResponse<PhongHatDetailDTO[]>> => {
    try {
        const res = await axiosInstance.get(`${BASE}/GetAllPhongHat`);
        return res.data as ApiResponse<PhongHatDetailDTO[]>;
    } catch (e: any) {
        throw new Error(e?.response?.data?.message || "Có lỗi xảy ra khi lấy danh sách phòng hát");
    }
};

export const apiGetPhongHat = async (maPhong: number): Promise<ApiResponse<PhongHatDetailDTO>> => {
    try {
        const res = await axiosInstance.get(`${BASE}/GetPhongHat/${maPhong}`);
        return res.data as ApiResponse<PhongHatDetailDTO>;
    } catch (e: any) {
        throw new Error(e?.response?.data?.message || "Có lỗi xảy ra khi lấy phòng hát");
    }
};

export const apiCreatePhongHat = async (dto: AddPhongHatDTO): Promise<ApiResponse<PhongHatDetailDTO>> => {
    try {
        const res = await axiosInstance.post(`${BASE}/CreatePhongHat`, dto);
        return res.data as ApiResponse<PhongHatDetailDTO>;
    } catch (e: any) {
        throw new Error(e?.response?.data?.message || "Có lỗi xảy ra khi tạo phòng hát");
    }
};

export const apiUpdatePhongHat = async (dto: UpdatePhongHatDTO): Promise<ApiResponse<PhongHatDetailDTO>> => {
    try {
        const res = await axiosInstance.put(`${BASE}/UpdatePhongHat`, dto);
        return res.data as ApiResponse<PhongHatDetailDTO>;
    } catch (e: any) {
        throw new Error(e?.response?.data?.message || "Có lỗi xảy ra khi cập nhật phòng hát");
    }
};

export const apiToggleNgungHoatDong = async (maPhong: number, ngungHoatDong: boolean): Promise<ApiResponse<boolean>> => {
    try {
        const res = await axiosInstance.patch(
            `${BASE}/UpdateNgungHoatDong?maPhong=${maPhong}&ngungHoatDong=${ngungHoatDong}`
        );
        return res.data as ApiResponse<boolean>;
    } catch (e: any) {
        throw new Error(e?.response?.data?.message || "Có lỗi xảy ra khi cập nhật trạng thái ngừng hoạt động");
    }
};

export const apiToggleDangSuDung = async (maPhong: number, dangSuDung: boolean): Promise<ApiResponse<boolean>> => {
    try {
        const res = await axiosInstance.patch(
            `${BASE}/UpdateDangSuDung?maPhong=${maPhong}&dangSuDung=${dangSuDung}`
        );
        return res.data as ApiResponse<boolean>;
    } catch (e: any) {
        throw new Error(e?.response?.data?.message || "Có lỗi xảy ra khi cập nhật trạng thái đang sử dụng");
    }
};
