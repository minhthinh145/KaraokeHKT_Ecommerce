namespace QLQuanKaraokeHKT.Core.Enums
{
    public enum BookingStatus
    {
        Pending = 1,
        DangSuDung = 2,
        DaKetThuc = 3,
        DaHuy = 4
    }

    public enum InvoiceStatus
    {
        ChuaThanhToan = 1,
        DaThanhToan = 2,
        DaHuy = 3,
        HetHanThanhToan = 4
    }

    public enum RoomOccupancyStatus
    {
        Available = 1,
        Occupied = 2,
        Maintenance = 3,
        OutOfService = 4
    }

    public static class BookingStatusExtensions
    {
        public static string GetDisplayName(this BookingStatus status)
        {
            return status switch
            {
                BookingStatus.Pending => "Chờ thanh toán",
                BookingStatus.DangSuDung => "Đang sử dụng",
                BookingStatus.DaKetThuc => "Đã kết thúc",
                BookingStatus.DaHuy => "Đã hủy",
                _ => "Không xác định"
            };
        }

        public static string GetDisplayName(this InvoiceStatus status)
        {
            return status switch
            {
                InvoiceStatus.ChuaThanhToan => "Chưa thanh toán",
                InvoiceStatus.DaThanhToan => "Đã thanh toán",
                InvoiceStatus.DaHuy => "Đã hủy",
                InvoiceStatus.HetHanThanhToan => "Hết hạn thanh toán",
                _ => "Không xác định"
            };
        }
    }
}