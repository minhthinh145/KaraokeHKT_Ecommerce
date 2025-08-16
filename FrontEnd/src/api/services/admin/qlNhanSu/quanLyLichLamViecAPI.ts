import axiosInstance from "../../../axiosConfig";
import type {
  ApiResponse,
  LichLamViecDTO,
  AddLichLamViecDTO,
  YeuCauChuyenCaDTO,
  PheDuyetYeuCauChuyenCaDTO,
} from "../../../../api";

const BASE = "QLLichLamViec";
const BASESHIFT = "NhanVienAll"
export const getAllLichLamViec = async (): Promise<
  ApiResponse<LichLamViecDTO[]>
> => {
  try {
    const res = await axiosInstance.get(`${BASE}/GetAllLichLamViec`);
    return res.data as ApiResponse<LichLamViecDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
      "Có lỗi xảy ra khi lấy toàn bộ lịch làm việc"
    );
  }
};

export const getLichLamViecByNhanVien = async (
  maNhanVien: string
): Promise<ApiResponse<LichLamViecDTO[]>> => {
  try {
    const res = await axiosInstance.get(
      `${BASE}/GetLichLamViecByNhanVien/${maNhanVien}`
    );
    return res.data as ApiResponse<LichLamViecDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
      "Có lỗi xảy ra khi lấy lịch làm việc theo nhân viên"
    );
  }
};

export const getLichLamViecByRange = async (
  start: string,
  end: string
): Promise<ApiResponse<LichLamViecDTO[]>> => {
  try {
    const res = await axiosInstance.get(`${BASE}/GetLichLamViecByRange`, {
      params: { start, end },
    });
    return res.data as ApiResponse<LichLamViecDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
      "Có lỗi xảy ra khi lấy lịch làm việc theo khoảng thời gian"
    );
  }
};


export const sendNotiWorkSchedules = async (
  start: string,
  end: string
): Promise<ApiResponse<null>> => {
  try {
    const res = await axiosInstance.post(
      `${BASE}/SendNotiWorkSchedules`,
      null, // body là null
      { params: { start, end } } // params sẽ lên query string
    );
    return res.data as ApiResponse<null>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
      "Có lỗi xảy ra khi gửi thông báo lịch làm việc"
    );
  }
};
export const createLichLamViec = async (
  data: AddLichLamViecDTO
): Promise<ApiResponse<LichLamViecDTO>> => {
  try {
    const res = await axiosInstance.post(`${BASE}/CreateLichLamViec`, data);
    return res.data as ApiResponse<LichLamViecDTO>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Có lỗi xảy ra khi tạo lịch làm việc"
    );
  }
};

export const updateLichLamViec = async (
  data: LichLamViecDTO
): Promise<ApiResponse<LichLamViecDTO>> => {
  try {
    const res = await axiosInstance.put(`${BASE}/UpdateLichLamViec`, data);
    return res.data as ApiResponse<LichLamViecDTO>;
  } catch (error: any) {
    throw new Error(error.response?.data?.message || "Có lỗi xảy ra khi cập nhật lịch làm việc");
  }
};

export const deleteLichLamViec = async (
  maLichLamViec: number
): Promise<ApiResponse<null>> => {
  try {
    const res = await axiosInstance.delete(`${BASE}/DeleteLichLamViec/${maLichLamViec}`);
    return res.data as ApiResponse<null>;
  } catch (error: any) {
    throw new Error(error.response?.data?.message || "Có lỗi xảy ra khi xóa lịch làm việc");
  }
};

// ================== YÊU CẦU CHUYỂN CA (QUẢN LÝ) ==================

export const getAllYeuCauChuyenCaAdmin = async (): Promise<
  ApiResponse<YeuCauChuyenCaDTO[]>
> => {
  try {
    const res = await axiosInstance.get(`${BASESHIFT}/yeu-cau-chuyen-ca/all`);
    return res.data as ApiResponse<YeuCauChuyenCaDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
      "Lỗi khi lấy tất cả yêu cầu chuyển ca"
    );
  }
};

export const getPendingYeuCauChuyenCaAdmin = async (): Promise<
  ApiResponse<YeuCauChuyenCaDTO[]>
> => {
  try {
    const res = await axiosInstance.get(`${BASESHIFT}/yeu-cau-chuyen-ca/pending`);
    return res.data as ApiResponse<YeuCauChuyenCaDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
      "Lỗi khi lấy yêu cầu chuyển ca đang chờ duyệt"
    );
  }
};

export const getApprovedYeuCauChuyenCaAdmin = async (): Promise<
  ApiResponse<YeuCauChuyenCaDTO[]>
> => {
  try {
    const res = await axiosInstance.get(`${BASESHIFT}/yeu-cau-chuyen-ca/approved`);
    return res.data as ApiResponse<YeuCauChuyenCaDTO[]>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
      "Lỗi khi lấy yêu cầu chuyển ca đã duyệt"
    );
  }
};

export const getYeuCauChuyenCaByIdAdmin = async (
  maYeuCau: number
): Promise<ApiResponse<YeuCauChuyenCaDTO>> => {
  try {
    const res = await axiosInstance.get(
      `${BASESHIFT}/yeu-cau-chuyen-ca/${maYeuCau}`
    );
    return res.data as ApiResponse<YeuCauChuyenCaDTO>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
      "Lỗi khi lấy chi tiết yêu cầu chuyển ca"
    );
  }
};

export const approveYeuCauChuyenCaAdmin = async (
  dto: PheDuyetYeuCauChuyenCaDTO
): Promise<ApiResponse<YeuCauChuyenCaDTO>> => {
  try {
    const res = await axiosInstance.put(
      `${BASESHIFT}/yeu-cau-chuyen-ca/approve`,
      dto
    );
    return res.data as ApiResponse<YeuCauChuyenCaDTO>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message ||
      "Lỗi khi phê duyệt yêu cầu chuyển ca"
    );
  }
};