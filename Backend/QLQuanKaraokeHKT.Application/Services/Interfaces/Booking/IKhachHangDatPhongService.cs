﻿using QLQuanKaraokeHKT.Application.DTOs.BookingDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Booking
{
    public interface IKhachHangDatPhongService
    {
        /// <summary>
        /// Lấy danh sách phòng hát có thể đặt
        /// </summary>
        Task<ServiceResult> GetPhongHatAvailableAsync();

        /// <summary>
        /// Lấy danh sách phòng theo loại phòng
        /// </summary>
        Task<ServiceResult> GetPhongHatByLoaiAsync(int maLoaiPhong);

        /// <summary>
        /// BƯỚC 1: Đặt phòng và tạo hóa đơn (chưa thanh toán)
        /// </summary>

        Task<ServiceResult> TaoHoaDonDatPhongAsync(DatPhongDTO datPhongDto, Guid userId);
        Task<ServiceResult> GetLichSuDatPhongByUserIdAsync(Guid userId);
        Task<ServiceResult> HuyDatPhongByUserIdAsync(Guid maThuePhong, Guid userId);


        /// <summary>
        /// BƯỚC 2: Xác nhận thanh toán và tạo URL VNPay
        /// </summary>
        Task<ServiceResult> XacNhanThanhToanAsync(XacNhanThanhToanDTO xacNhanDto, HttpContext httpContext);

        /// <summary>
        /// Xử lý thanh toán VNPay callback
        /// </summary>
        Task<ServiceResult> XuLyThanhToanVNPayAsync(IQueryCollection vnpayData);

        /// <summary>
        /// Lịch sử đặt phòng của khách hàng
        /// </summary>
        Task<ServiceResult> GetLichSuDatPhongAsync(Guid maKhachHang);

        /// <summary>
        /// Hủy đặt phòng
        /// </summary>
        Task<ServiceResult> HuyDatPhongAsync(Guid maThuePhong, Guid maKhachHang);

        /// <summary>
        /// Thanh toán lại cho đơn hết hạn
        /// </summary>
        Task<ServiceResult> ThanhToanLaiAsync(Guid maThuePhong, Guid userId);

        /// <summary>
        /// Lấy danh sách đặt phòng chưa thanh toán của khách hàng
        /// </summary>
        Task<ServiceResult> GetDatPhongChuaThanhToanByUserIdAsync(Guid userId);

        /// <summary>
        /// Lấy danh sách đặt phòng chưa thanh toán của khách hàng
        /// </summary>
        Task<ServiceResult> GetDatPhongChuaThanhToanAsync(Guid maKhachHang);
    }
}