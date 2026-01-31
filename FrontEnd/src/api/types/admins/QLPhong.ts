export interface LoaiPhongDTO {
    maLoaiPhong: number;
    tenLoaiPhong: string;
    sucChua: number;
}

export interface AddLoaiPhongDTO {
    tenLoaiPhong: string;
    sucChua: number;
}

export interface UpdateLoaiPhongDTO {
    maLoaiPhong: number;
    tenLoaiPhong: string;
    sucChua: number;
}
// ...existing imports / code...
export interface AddPhongHatDTO {
    maLoaiPhong: number;
    tenPhong: string;
    hinhAnhPhong?: string | null;
    // Cấu hình giá thuê
    dongGiaAllCa: boolean;
    giaThueChung?: number | null;
    giaThueCa1?: number | null;
    giaThueCa2?: number | null;
    giaThueCa3?: number | null;

    ngayApDungGia: string;   // yyyy-MM-dd
    trangThaiGia: string;    // "HieuLuc"
}

export interface PhongHatDetailDTO {
    maPhong: number;
    dangSuDung: boolean;
    ngungHoatDong: boolean;

    maLoaiPhong: number;
    tenLoaiPhong?: string | null;
    sucChua: number;

    maSanPham: number;
    tenSanPham?: string | null;
    hinhAnhSanPham?: string | null;

    dongGiaAllCa?: boolean | null;
    giaThueChung?: number | null;
    giaThueCa1?: number | null;
    giaThueCa2?: number | null;
    giaThueCa3?: number | null;

    giaThueHienTai?: number | null;
    ngayApDungGia?: string | null;
    trangThaiGia?: string | null;
}

export interface UpdatePhongHatDTO {
    maPhong: number;
    tenPhong: string;
    hinhAnhPhong?: string | null;
    maLoaiPhong?: number | null;
    capNhatGiaThue: boolean;
    dongGiaAllCa?: boolean | null;
    giaThueChung?: number | null;
    giaThueCa1?: number | null;
    giaThueCa2?: number | null;
    giaThueCa3?: number | null;
    ngayApDungGia?: string | null;
    trangThaiGia?: string | null;
}