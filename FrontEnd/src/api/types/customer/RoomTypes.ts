export interface PhongHatForCustomerDTO {
    maPhong: number;
    tenPhong: string;
    tenLoaiPhong: string;
    sucChua: number;
    hinhAnhPhong?: string;
    giaThueHienTai: number;
    giaThueCa1: number;
    giaThueCa2: number;
    giaThueCa3: number;
    coGiaTheoCa: boolean;
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
