import axiosInstance from "../../axiosConfig";
import type { ApiResponse } from "../../types/apiResponse";
import type {
  LichLamViecDTO,
  YeuCauChuyenCaDTO,
  AddYeuCauChuyenCaDTO,
} from "../../types/admins/QLNhanSutypes";
import type { NhanVienTaiKhoanDTO } from "../../types/admins/QLHeThongtypes";

// BASE (không thêm dấu / đầu vì axios baseURL đã có)
const BASE_LICH = "QLLichLamViec";
const BASE_SHIFT = "NhanVienAll";
// 1. Lấy lịch làm việc theo nhân viên
export const apiGetScheduleByNhanVien = async (
  maNhanVien: string
): Promise<ApiResponse<LichLamViecDTO[]>> => {
  const res = await axiosInstance.get(
    `${BASE_LICH}/GetLichLamViecByNhanVien/${maNhanVien}`
  );
  return res.data as ApiResponse<LichLamViecDTO[]>;
};

// 2. Danh sách yêu cầu chuyển ca của nhân viên
export const apiGetMyShiftChangeRequests = async (
  maNhanVien: string
): Promise<ApiResponse<YeuCauChuyenCaDTO[]>> => {
  const res = await axiosInstance.get(
    `${BASE_SHIFT}/yeu-cau-chuyen-ca/my-requests/${maNhanVien}`
  );
  return res.data as ApiResponse<YeuCauChuyenCaDTO[]>;
};

// 3. Tạo yêu cầu chuyển ca
export const apiCreateShiftChange = async (
  payload: AddYeuCauChuyenCaDTO
): Promise<ApiResponse<YeuCauChuyenCaDTO>> => {
  const res = await axiosInstance.post(`${BASE_SHIFT}/yeu-cau-chuyen-ca/create`, payload);
  return res.data as ApiResponse<YeuCauChuyenCaDTO>;
};

// 4. Xóa yêu cầu (khi chưa duyệt)
export const apiDeleteShiftChange = async (
  maYeuCau: number
): Promise<ApiResponse<boolean>> => {
  const res = await axiosInstance.delete(`${BASE_SHIFT}/yeu-cau-chuyen-ca/delete/${maYeuCau}`);
  return res.data as ApiResponse<boolean>;
};

// 5. Chi tiết 1 yêu cầu
export const apiGetShiftChangeDetail = async (
  maYeuCau: number
): Promise<ApiResponse<YeuCauChuyenCaDTO>> => {
  const res = await axiosInstance.get(`${BASE_SHIFT}/yeu-cau-chuyen-ca/${maYeuCau}`);
  return res.data as ApiResponse<YeuCauChuyenCaDTO>;
};

// Thêm API lấy lịch theo khoảng ngày cho 1 nhân viên
export const apiGetScheduleByNhanVienAndRange = async (
  maNhanVien: string,
  start: string,
  end: string
): Promise<ApiResponse<LichLamViecDTO[]>> => {
  const url = `${BASE_LICH}/GetLichLamViecByNhanVienAndRange?maNhanVien=${maNhanVien}&start=${start}&end=${end}`;
  const res = await axiosInstance.get(url);
  return res.data as ApiResponse<LichLamViecDTO[]>;
};

export const apiGetMyProfile = async (): Promise<
  ApiResponse<NhanVienTaiKhoanDTO>
> => {
  try {
    const res = await axiosInstance.get(`${BASE_SHIFT}/profile`);
    return res.data as ApiResponse<NhanVienTaiKhoanDTO>;
  } catch (error: any) {
    throw new Error(
      error.response?.data?.message || "Có lỗi khi lấy thông tin nhân viên"
    );
  }
};