namespace QLQuanKaraokeHKT.Shared.Enums
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
}