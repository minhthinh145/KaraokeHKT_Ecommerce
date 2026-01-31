export interface AddVatLieuDTO {
    tenVatLieu: string;
    donViTinh: string;
    soLuongTonKho: number;
    tenSanPham: string;
    hinhAnhSanPham?: string | null;

    giaNhap: number;
    ngayApDungGiaNhap: string;      // ISO yyyy-MM-dd
    trangThaiGiaNhap: string;       // "HieuLuc"

    dongGiaAllCa: boolean;          // true => dùng giaBanChung
    giaBanChung?: number | null;    // khi dongGiaAllCa = true

    giaBanCa1?: number | null;      // khi dongGiaAllCa = false
    giaBanCa2?: number | null;
    giaBanCa3?: number | null;

    ngayApDungGiaBan: string;
    trangThaiGiaBan: string;
}

export interface UpdateSoLuongDTO {
    maVatLieu: number;
    soLuongMoi: number;
}

export interface VatLieuDetailDTO {
    maVatLieu: number;
    tenVatLieu: string;
    donViTinh: string;
    soLuongTonKho: number;
    ngungCungCap: boolean;

    maSanPham?: number | null;
    tenSanPham?: string | null;
    hinhAnhSanPham?: string | null;

    // Giá nhập hiện tại
    giaNhapHienTai?: number | null;
    ngayApDungGiaNhap?: string | null;  // DateOnly -> string (yyyy-MM-dd)
    trangThaiGiaNhap?: string | null;

    // Giá bán hiện tại (tổng quát)
    giaBanHienTai?: number | null;
    ngayApDungGiaBan?: string | null;   // DateOnly -> string
    trangThaiGiaBan?: string | null;

    // Món ăn liên kết (nếu có)
    maMonAn?: number | null;
    soLuongConLai?: number | null;

    // Bổ sung giá bán theo ca
    dongGiaAllCa?: boolean | null;
    giaBanChung?: number | null;
    giaBanCa1?: number | null;
    giaBanCa2?: number | null;
    giaBanCa3?: number | null;
}

export interface UpdateVatLieuDTO {
    maVatLieu: number;
    tenVatLieu: string;
    donViTinh: string;
    tenSanPham: string;
    hinhAnhSanPham?: string | null;

    giaNhapMoi?: number | null;
    ngayApDungGiaNhap?: string | null;
    trangThaiGiaNhap?: string | null;

    capNhatGiaBan?: boolean | null;
    dongGiaAllCa?: boolean | null;
    giaBanChung?: number | null;
    giaBanCa1?: number | null;
    giaBanCa2?: number | null;
    giaBanCa3?: number | null;
    ngayApDungGiaBan?: string | null;
    trangThaiGiaBan?: string | null;
}