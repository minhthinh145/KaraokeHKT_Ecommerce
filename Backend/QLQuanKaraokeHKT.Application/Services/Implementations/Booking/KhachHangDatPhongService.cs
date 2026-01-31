//using AutoMapper;
//using QLQuanKaraokeHKT.Core.Common;
//using QLQuanKaraokeHKT.Application.DTOs.BookingDTOs;
//using QLQuanKaraokeHKT.Application.DTOs.QLPhongDTOs;
//using QLQuanKaraokeHKT.Application.DTOs.VNPayDTOs;
//using QLQuanKaraokeHKT.Domain.Entities;
//using QLQuanKaraokeHKT.Application;
//using QLQuanKaraokeHKT.Application.Services.Implementations.Booking;
//using QLQuanKaraokeHKT.Application.Services.Implementations.Common;
//using QLQuanKaraokeHKT.Application.Services.Implementations.Payment;
//using QLQuanKaraokeHKT.Application.Services.Implementations.Room;

//namespace QLQuanKaraokeHKT.Application.Services.Implementations.Booking
//{
//    public class KhachHangDatPhongService : IKhachHangDatPhongService
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IVNPayService _vnPayService;
//        private readonly IRoomOrchestrator _roomOrchestrator; 
//        private readonly IPricingService _roomPricingService;
//        private readonly IMapper _mapper;
//        private readonly ILogger<KhachHangDatPhongService> _logger;


//        public KhachHangDatPhongService(
         
//            IVNPayService vnPayService,
//            IMapper mapper,
//            IUnitOfWork unitOfWork,
//            IRoomOrchestrator roomOrchestrator,
//            IPricingService roomPricingService, 
//            ILogger<KhachHangDatPhongService> logger)
//        {
//            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
//            _vnPayService = vnPayService;
//            _mapper = mapper;
//            _roomOrchestrator = roomOrchestrator ?? throw new ArgumentNullException(nameof(roomOrchestrator));
//            _roomPricingService = roomPricingService ?? throw new ArgumentNullException(nameof(roomPricingService));
//            _logger = logger;
      
//        }

//        public async Task<ServiceResult> GetPhongHatAvailableAsync()
//        {
//            try
//            {
//                var roomsResult = await _roomOrchestrator.GetRoomsByStatusWorkflowAsync(RoomStatusFilter.Available);
//                if (!roomsResult.IsSuccess)
//                    return roomsResult;

//                var phongHats = (List<PhongHatDetailDTO>)roomsResult.Data!;
                
//                var customerDTOs = new List<PhongHatForCustomerDTO>();
                
//                foreach (var room in phongHats)
//                {
//                    var customerDto = _mapper.Map<PhongHatForCustomerDTO>(room);
                    
//                    // ✅ GET CURRENT PRICING VIA PRICING SERVICE
//                    var currentPrices = await _roomPricingService.GetCurrentPricesAsync(room.MaSanPham);
//                    customerDto.GiaThueHienTai = currentPrices.FirstOrDefault()?.DonGia ?? 0;
                    
//                    customerDTOs.Add(customerDto);
//                }

//                return ServiceResult.Success("Lấy danh sách phòng hát thành công.", customerDTOs);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi lấy danh sách phòng hát");
//                return ServiceResult.Failure($"Lỗi khi lấy danh sách phòng hát: {ex.Message}");
//            }
//        }

//        public async Task<ServiceResult> GetPhongHatByLoaiAsync(int maLoaiPhong)
//        {
//            try
//            {
//                // ✅ DELEGATE TO ROOM ORCHESTRATOR
//                var roomsResult = await _roomOrchestrator.GetRoomsByTypeWorkflowAsync(maLoaiPhong);
//                if (!roomsResult.IsSuccess)
//                    return roomsResult;

//                var phongHats = (List<PhongHatDetailDTO>)roomsResult.Data!;
                
//                // Filter available rooms
//                var availableRooms = phongHats.Where(p => p.DangSuDung == false).ToList();
                
//                if (!availableRooms.Any())
//                    return ServiceResult.Failure("Không có phòng hát nào thuộc loại này đang trống.");

//                // ✅ CONVERT TO CUSTOMER DTO WITH PRICING
//                var customerDTOs = new List<PhongHatForCustomerDTO>();
                
//                foreach (var room in availableRooms)
//                {
//                    var customerDto = _mapper.Map<PhongHatForCustomerDTO>(room);
//                    var currentPrices = await _roomPricingService.GetCurrentPricesAsync(room.MaSanPham);
//                    customerDto.GiaThueHienTai = currentPrices.FirstOrDefault()?.DonGia ?? 0;
//                    customerDTOs.Add(customerDto);
//                }

//                return ServiceResult.Success("Lấy danh sách phòng hát theo loại thành công.", customerDTOs);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi lấy danh sách phòng hát theo loại");
//                return ServiceResult.Failure($"Lỗi khi lấy danh sách phòng hát: {ex.Message}");
//            }
//        }

//        public async Task<ServiceResult> TaoHoaDonDatPhongAsync(DatPhongDTO datPhongDto, Guid userId)
//        {
//            try
//            {
//                var khachHang = await _unitOfWork.KhachHangRepository.GetByAccountIdAsync(userId);
//                if (khachHang == null)
//                    return ServiceResult.Failure("Không tìm thấy thông tin khách hàng.");

//                datPhongDto.MaKhachHang = khachHang.MaKhachHang;

//                // Business validation
//                var validationResult = await ValidateDatPhongAsync(datPhongDto);
//                if (!validationResult.IsSuccess)
//                    return validationResult;

//                return await _unitOfWork.ExecuteTransactionAsync(async () =>
//                {
//                    // ✅ GET ROOM DETAILS VIA ORCHESTRATOR
//                    var roomDetailResult = await _roomOrchestrator.GetRoomDetailWorkflowAsync(datPhongDto.MaPhong);
//                    if (!roomDetailResult.IsSuccess)
//                        return ServiceResult.Failure("Thông tin phòng không hợp lệ.");

//                    var roomDetail = (PhongHatDetailDTO)roomDetailResult.Data!;

//                    // ✅ DELEGATE ROOM STATUS UPDATE TO ORCHESTRATOR
//                    var statusUpdateResult = await _roomOrchestrator.ChangeRoomOccupancyStatusWorkflowAsync(datPhongDto.MaPhong, true);
//                    if (!statusUpdateResult.IsSuccess)
//                        return statusUpdateResult;

//                    // ✅ GET PRICING VIA PRICING SERVICE
//                    var currentPrices = await _roomPricingService.GetCurrentPricesAsync(roomDetail.MaSanPham);
//                    var giaPhong = currentPrices.FirstOrDefault()?.DonGia ?? 0;
//                    var tongTien = giaPhong * datPhongDto.SoGioSuDung;

//                    // Rest of booking logic...
//                    var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
//                    var thoiGianBatDauVietnam = datPhongDto.ThoiGianBatDau.Kind == DateTimeKind.Utc
//                        ? TimeZoneInfo.ConvertTimeFromUtc(datPhongDto.ThoiGianBatDau, vietnamTimeZone)
//                        : datPhongDto.ThoiGianBatDau;

//                    var thoiGianKetThucDuKien = thoiGianBatDauVietnam.AddHours(datPhongDto.SoGioSuDung);

//                    // Create invoice and booking records...
//                    // (Keep existing invoice creation logic)
                    
//                    return ServiceResult.Success("Tạo hóa đơn đặt phòng thành công.");
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi tạo hóa đơn đặt phòng");
//                return ServiceResult.Failure($"Lỗi khi tạo hóa đơn đặt phòng: {ex.Message}");
//            }
//        }

//        public async Task<ServiceResult> XacNhanThanhToanAsync(XacNhanThanhToanDTO xacNhanDto, HttpContext httpContext)
//        {
//            try
//            {
//                // ✅ LẤY HÓA ĐƠN QUA REPOSITORY
//                var hoaDon = await _unitOfWork.HoaDonRepository.GetByIdAsync(xacNhanDto.MaHoaDon);

//                if (hoaDon == null)
//                    return ServiceResult.Failure("Không tìm thấy hóa đơn.");

//                if (hoaDon.TrangThai != "ChuaThanhToan")
//                    return ServiceResult.Failure("Hóa đơn đã được xử lý hoặc đã hủy.");

//                var thuePhong = await _unitOfWork.ThuePhongRepository.GetByIdAsync(xacNhanDto.MaThuePhong);
//                if (thuePhong == null || thuePhong.TrangThai != "Pending")
//                    return ServiceResult.Failure("Thông tin thuê phòng không hợp lệ.");

//                // Kiểm tra thời hạn thanh toán (15 phút)
//                if (hoaDon.NgayTao.AddMinutes(15) < DateTime.Now)
//                    return ServiceResult.Failure("Đã hết thời gian thanh toán. Vui lòng đặt phòng lại.");

//                // Tạo URL thanh toán VNPay
//                var vnpayRequest = new VNPayRequestDTO
//                {
//                    OrderId = hoaDon.MaHoaDon,
//                    Amount = hoaDon.TongTienHoaDon,
//                    OrderDescription = hoaDon.TenHoaDon,
//                    CustomerName = hoaDon.MaKhachHangNavigation.TenKhachHang,
//                    CustomerEmail = hoaDon.MaKhachHangNavigation.Email,
//                    CustomerPhone = hoaDon.MaKhachHangNavigation.MaTaiKhoanNavigation?.PhoneNumber
//                };

//                var paymentResult = await _vnPayService.CreatePaymentUrlAsync(vnpayRequest, httpContext);
//                if (!paymentResult.IsSuccess)
//                    return ServiceResult.Failure("Không thể tạo URL thanh toán.");

//                return ServiceResult.Success("Tạo URL thanh toán thành công.", new
//                {
//                    hoaDon.MaHoaDon,
//                    thuePhong.MaThuePhong,
//                    UrlThanhToan = paymentResult.Data.ToString(),
//                    TongTien = hoaDon.TongTienHoaDon
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi xác nhận thanh toán");
//                return ServiceResult.Failure($"Lỗi khi xác nhận thanh toán: {ex.Message}");
//            }
//        }

//        public async Task<ServiceResult> XuLyThanhToanVNPayAsync(IQueryCollection vnpayData)
//        {
//            try
//            {
//                // 1. Xử lý response từ VNPay
//                var vnpayResult = await _vnPayService.ProcessVNPayResponseAsync(vnpayData);
//                if (!vnpayResult.IsSuccess)
//                    return vnpayResult;

//                var vnpayResponse = (VNPayResponseDTO)vnpayResult.Data;

//                // 2. ✅ LẤY HÓA ĐƠN QUA REPOSITORY
//                var hoaDon = await _unitOfWork.HoaDonRepository.GetByIdAsync(vnpayResponse.OrderId);
//                if (hoaDon == null)
//                    return ServiceResult.Failure("Không tìm thấy hóa đơn.");

//                // ✅ TÌM THUEPHONG BẰNG MaHoaDon THAY VÌ MaKhachHang
//                var thuePhongs = await _unitOfWork.ThuePhongRepository.GetThuePhongByKhachHangWithDetailsAsync(hoaDon.MaKhachHang);
//                var thuePhong = thuePhongs.FirstOrDefault(t => t.MaHoaDon == hoaDon.MaHoaDon && t.TrangThai == "Pending");

//                if (thuePhong == null)
//                    return ServiceResult.Failure("Không tìm thấy thông tin thuê phòng.");

//                if (vnpayResponse.Success)
//                {
//                    // ✅ THANH TOÁN THÀNH CÔNG
//                    // 3. Cập nhật trạng thái qua repository
//                    await _unitOfWork.HoaDonRepository.UpdateHoaDonStatusAsync(hoaDon.MaHoaDon, "DaThanhToan");
//                    await _unitOfWork.ThuePhongRepository.UpdateTrangThaiAsync(thuePhong.MaThuePhong, "DangSuDung");

//                    // 4. ✅ TẠO LỊCH SỬ SỬ DỤNG QUA REPOSITORY
//                    var lichSuSuDung = new LichSuSuDungPhong
//                    {
//                        MaPhong = thuePhong.MaPhong,
//                        MaKhachHang = thuePhong.MaKhachHang,
//                        ThoiGianBatDau = thuePhong.ThoiGianBatDau,
//                        ThoiGianKetThuc = thuePhong.ThoiGianKetThuc.Value,
//                        MaHoaDon = hoaDon.MaHoaDon,
//                        MaThuePhong = thuePhong.MaThuePhong
//                    };

//                    await _unitOfWork.LichSuSuDungPhongRepository.CreateLichSuAsync(lichSuSuDung);

//                    return ServiceResult.Success("Thanh toán thành công. Phòng đã sẵn sàng sử dụng.", new
//                    {
//                        thuePhong.MaThuePhong,
//                        hoaDon.MaHoaDon,
//                        TenPhong = thuePhong.MaPhongNavigation?.MaSanPhamNavigation?.TenSanPham,
//                        thuePhong.ThoiGianBatDau,
//                        thuePhong.ThoiGianKetThuc,
//                        TrangThai = "DangSuDung"
//                    });
//                }
//                else
//                {
//                    // Thanh toán thất bại
//                    await _unitOfWork.HoaDonRepository.UpdateHoaDonStatusAsync(hoaDon.MaHoaDon, "DaHuy");
//                    await _unitOfWork.ThuePhongRepository.UpdateTrangThaiAsync(thuePhong.MaThuePhong, "DaHuy");

//                    // Trả lại phòng
//                   // await _unitOfWork.PhongHatRepository.UpdateDangSuDungAsync(thuePhong.MaPhong, false);

//                    return ServiceResult.Failure($"Thanh toán thất bại: {vnpayResponse.ErrorMessage}");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi xử lý thanh toán VNPay");
//                return ServiceResult.Failure($"Lỗi khi xử lý thanh toán: {ex.Message}");
//            }
//        }

//        public async Task<ServiceResult> GetLichSuDatPhongByUserIdAsync(Guid userId)
//        {
//            try
//            {
//                var khachHang = await _unitOfWork.KhachHangRepository.GetByAccountIdAsync(userId);
//                if (khachHang == null)
//                    return ServiceResult.Failure("Không tìm thấy thông tin khách hàng.");

//                return await GetLichSuDatPhongAsync(khachHang.MaKhachHang);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi lấy lịch sử đặt phòng theo userId");
//                return ServiceResult.Failure($"Lỗi khi lấy lịch sử đặt phòng: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Lấy lịch sử đặt phòng đầy đủ (bao gồm cả đã thanh toán và chưa thanh toán)
//        /// </summary>
//        public async Task<ServiceResult> GetLichSuDatPhongAsync(Guid maKhachHang)
//        {
//            try
//            {
//                // ✅ LẤY TẤT CẢ THUEPHONG CỦA KHÁCH HÀNG
//                var thuePhongs = await _unitOfWork.ThuePhongRepository.GetThuePhongByKhachHangWithDetailsAsync(maKhachHang);

//                if (!thuePhongs.Any())
//                    return ServiceResult.Failure("Khách hàng chưa có lịch sử đặt phòng.");

//                var lichSuDTOs = new List<LichSuDatPhongDTO>();

//                foreach (var thuePhong in thuePhongs)
//                {
//                    // ✅ LẤY HÓA ĐƠN (NẾU CÓ)
//                    HoaDonDichVu? hoaDon = null;
//                    if (thuePhong.MaHoaDon.HasValue)
//                    {
//                        hoaDon = await _unitOfWork.HoaDonRepository.GetByIdAsync(thuePhong.MaHoaDon.Value);
//                    }

//                    // ✅ MAPPING DỮ LIỆU HOÀN CHỈNH
//                    var dto = new LichSuDatPhongDTO
//                    {
//                        // Thông tin cơ bản từ ThuePhong
//                        MaThuePhong = thuePhong.MaThuePhong,
//                        MaPhong = thuePhong.MaPhong,
//                        ThoiGianBatDau = thuePhong.ThoiGianBatDau,
//                        ThoiGianKetThuc = thuePhong.ThoiGianKetThuc,
//                        SoGioSuDung = thuePhong.ThoiGianKetThuc.HasValue ? 
//                            (int)(thuePhong.ThoiGianKetThuc.Value - thuePhong.ThoiGianBatDau).TotalHours : 0,
                        
//                        // Thông tin phòng
//                        TenPhong = thuePhong.MaPhongNavigation?.MaSanPhamNavigation?.TenSanPham ?? $"Phòng {thuePhong.MaPhong}",
//                        HinhAnhPhong = thuePhong.MaPhongNavigation?.MaSanPhamNavigation?.HinhAnhSanPham,
//                        TenLoaiPhong = thuePhong.MaPhongNavigation?.MaLoaiPhongNavigation?.TenLoaiPhong,
                        
//                        // Thông tin hóa đơn
//                        MaHoaDon = hoaDon?.MaHoaDon,
//                        TenHoaDon = hoaDon?.TenHoaDon,
//                        MoTaHoaDon = hoaDon?.MoTaHoaDon,
//                        TongTien = hoaDon?.TongTienHoaDon ?? 0,
//                        NgayTao = hoaDon?.NgayTao ?? thuePhong.ThoiGianBatDau,
//                        NgayThanhToan = hoaDon?.TrangThai == "DaThanhToan" ? hoaDon.NgayTao : null,
                        
//                        // Thông tin thanh toán
//                        HanThanhToan = hoaDon?.NgayTao.AddMinutes(15),
//                        DaHetHanThanhToan = hoaDon != null && hoaDon.NgayTao.AddMinutes(15) < DateTime.Now,
//                        TrangThai = GetTrangThaiDatPhong(thuePhong, hoaDon),
                        
//                        // Thông tin khách hàng
//                        TenKhachHang = thuePhong.MaKhachHangNavigation?.TenKhachHang,
//                        EmailKhachHang = thuePhong.MaKhachHangNavigation?.Email
//                    };

//                    lichSuDTOs.Add(dto);
//                }

//                // ✅ SẮP XẾP THEO THỜI GIAN MỚI NHẤT
//                var result = lichSuDTOs.OrderByDescending(x => x.NgayTao).ToList();

//                return ServiceResult.Success("Lấy lịch sử đặt phòng thành công.", result);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi lấy lịch sử đặt phòng");
//                return ServiceResult.Failure($"Lỗi khi lấy lịch sử đặt phòng: {ex.Message}");
//            }
//        }

//        public async Task<ServiceResult> HuyDatPhongAsync(Guid maThuePhong, Guid maKhachHang)
//        {
//            try
//            {
//                var thuePhong = await _unitOfWork.ThuePhongRepository.GetByIdAsync(maThuePhong);

//                if (thuePhong == null)
//                    return ServiceResult.Failure("Không tìm thấy thông tin đặt phòng.");

//                if (thuePhong.MaKhachHang != maKhachHang)
//                    return ServiceResult.Failure("Bạn không có quyền hủy đặt phòng này.");

//                if (thuePhong.TrangThai != "Pending")
//                    return ServiceResult.Failure("Chỉ có thể hủy đặt phòng chưa thanh toán.");

//                // ✅ KIỂM TRA HÓA ĐƠN TRƯỚC KHI HỦY
//                HoaDonDichVu? hoaDon = null;
//                if (thuePhong.MaHoaDon.HasValue)
//                {
//                    hoaDon = await _unitOfWork.HoaDonRepository.GetByIdAsync(thuePhong.MaHoaDon.Value);
                    
//                    // Kiểm tra trạng thái hóa đơn
//                    if (hoaDon != null && hoaDon.TrangThai == "DaThanhToan")
//                    {
//                        return ServiceResult.Failure("Không thể hủy đặt phòng đã thanh toán.");
//                    }
//                }

//                // ✅ HỦY HÓA ĐƠN THÔNG QUA MaHoaDon TRONG THUEPHONG
//                if (thuePhong.MaHoaDon.HasValue)
//                {
//                    await _unitOfWork.HoaDonRepository.UpdateHoaDonStatusAsync(thuePhong.MaHoaDon.Value, "DaHuy");
//                }

//                // ✅ CẬP NHẬT TRẠNG THÁI THUEPHONG
//                await _unitOfWork.ThuePhongRepository.UpdateTrangThaiAsync(maThuePhong, "DaHuy");

//                // ✅ TRẢ LẠI PHÒNG
//             //   await _unitOfWork.PhongHatRepository.UpdateAsync(thuePhong.MaPhong, false);

//                var responseMessage = hoaDon != null ? 
//                    $"Hủy đặt phòng thành công. Hóa đơn {hoaDon.TenHoaDon} đã được hủy." :
//                    "Hủy đặt phòng thành công.";

//                return ServiceResult.Success(responseMessage, new
//                {
//                    MaThuePhong = maThuePhong,
//                    thuePhong.MaHoaDon,
//                    TrangThai = "DaHuy",
//                    NgayHuy = DateTime.Now
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi hủy đặt phòng");
//                return ServiceResult.Failure($"Lỗi khi hủy đặt phòng: {ex.Message}");
//            }
//        }

//        public async Task<ServiceResult> HuyDatPhongByUserIdAsync(Guid maThuePhong, Guid userId)
//        {
//            try
//            {
//                var khachHang = await _unitOfWork.KhachHangRepository.GetByAccountIdAsync(userId);
//                if (khachHang == null)
//                    return ServiceResult.Failure("Không tìm thấy thông tin khách hàng.");

//                return await HuyDatPhongAsync(maThuePhong, khachHang.MaKhachHang);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi hủy đặt phòng theo userId");
//                return ServiceResult.Failure($"Lỗi khi hủy đặt phòng: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Lấy danh sách đặt phòng chưa thanh toán của khách hàng
//        /// </summary>
//        public async Task<ServiceResult> GetDatPhongChuaThanhToanByUserIdAsync(Guid userId)
//        {
//            try
//            {
//                var khachHang = await _unitOfWork.KhachHangRepository.GetByAccountIdAsync(userId);
//                if (khachHang == null)
//                    return ServiceResult.Failure("Không tìm thấy thông tin khách hàng.");

//                return await GetDatPhongChuaThanhToanAsync(khachHang.MaKhachHang);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi lấy danh sách đặt phòng chưa thanh toán theo userId");
//                return ServiceResult.Failure($"Lỗi khi lấy danh sách đặt phòng chưa thanh toán: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Lấy danh sách đặt phòng chưa thanh toán của khách hàng
//        /// </summary>
//        public async Task<ServiceResult> GetDatPhongChuaThanhToanAsync(Guid maKhachHang)
//        {
//            try
//            {
//                // ✅ LẤY TẤT CẢ THUEPHONG CỦA KHÁCH HÀNG CÓ TRẠNG THÁI PENDING
//                var thuePhongs = await _unitOfWork.ThuePhongRepository.GetThuePhongByKhachHangWithDetailsAsync(maKhachHang);
//                var thuePhongsPending = thuePhongs.Where(t => t.TrangThai == "Pending" && t.MaHoaDon.HasValue).ToList();

//                if (!thuePhongsPending.Any())
//                    return ServiceResult.Failure("Không có đơn đặt phòng nào chưa thanh toán.");

//                var datPhongChuaThanhToanDTOs = new List<LichSuDatPhongDTO>();

//                foreach (var thuePhong in thuePhongsPending)
//                {
//                    // ✅ LẤY HÓA ĐƠN THÔNG QUA MaHoaDon TRONG THUEPHONG
//                    var hoaDon = await _unitOfWork.HoaDonRepository.GetByIdAsync(thuePhong.MaHoaDon.Value);
                    
//                    if (hoaDon == null || hoaDon.TrangThai != "ChuaThanhToan")
//                        continue; // Bỏ qua nếu không có hóa đơn hoặc hóa đơn không phải chưa thanh toán

//                    // ✅ MAPPING DỮ LIỆU HOÀN CHỈNH
//                    var dto = new LichSuDatPhongDTO
//                    {
//                        // Thông tin cơ bản từ ThuePhong
//                        MaThuePhong = thuePhong.MaThuePhong,
//                        MaPhong = thuePhong.MaPhong,
//                        ThoiGianBatDau = thuePhong.ThoiGianBatDau,
//                        ThoiGianKetThuc = thuePhong.ThoiGianKetThuc,
//                        SoGioSuDung = thuePhong.ThoiGianKetThuc.HasValue ? 
//                            (int)(thuePhong.ThoiGianKetThuc.Value - thuePhong.ThoiGianBatDau).TotalHours : 0,
                        
//                        // Thông tin phòng
//                        TenPhong = thuePhong.MaPhongNavigation?.MaSanPhamNavigation?.TenSanPham ?? $"Phòng {thuePhong.MaPhong}",
//                        HinhAnhPhong = thuePhong.MaPhongNavigation?.MaSanPhamNavigation?.HinhAnhSanPham,
//                        TenLoaiPhong = thuePhong.MaPhongNavigation?.MaLoaiPhongNavigation?.TenLoaiPhong,
                        
//                        // Thông tin hóa đơn
//                        MaHoaDon = hoaDon.MaHoaDon,
//                        TenHoaDon = hoaDon.TenHoaDon,
//                        MoTaHoaDon = hoaDon.MoTaHoaDon,
//                        TongTien = hoaDon.TongTienHoaDon,
//                        NgayTao = hoaDon.NgayTao,
                        
//                        // Thông tin thanh toán
//                        HanThanhToan = hoaDon.NgayTao.AddMinutes(15),
//                        DaHetHanThanhToan = hoaDon.NgayTao.AddMinutes(15) < DateTime.Now,
//                        TrangThai = GetTrangThaiDatPhong(thuePhong, hoaDon),
                        
//                        // Thông tin khách hàng
//                        TenKhachHang = thuePhong.MaKhachHangNavigation?.TenKhachHang,
//                        EmailKhachHang = thuePhong.MaKhachHangNavigation?.Email
//                    };

//                    datPhongChuaThanhToanDTOs.Add(dto);
//                }

//                if (!datPhongChuaThanhToanDTOs.Any())
//                    return ServiceResult.Failure("Không có đơn đặt phòng nào chưa thanh toán hợp lệ.");

//                // ✅ SẮP XẾP: ƯU TIÊN CHƯA HẾT HẠN, SAU ĐÓ THEO THỜI GIAN TẠO
//                var result = datPhongChuaThanhToanDTOs
//                    .OrderBy(x => x.DaHetHanThanhToan)  // Chưa hết hạn lên đầu
//                    .ThenByDescending(x => x.NgayTao)   // Mới nhất lên đầu
//                    .ToList();

//                return ServiceResult.Success("Lấy danh sách đặt phòng chưa thanh toán thành công.", result);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi lấy danh sách đặt phòng chưa thanh toán");
//                return ServiceResult.Failure($"Lỗi khi lấy danh sách đặt phòng chưa thanh toán: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Thanh toán lại cho đơn hết hạn
//        /// </summary>
//        public async Task<ServiceResult> ThanhToanLaiAsync(Guid maThuePhong, Guid userId)
//        {
//            try
//            {
//                var khachHang = await _unitOfWork.KhachHangRepository.GetByAccountIdAsync(userId);
//                if (khachHang == null)
//                    return ServiceResult.Failure("Không tìm thấy thông tin khách hàng.");

//                var thuePhong = await _unitOfWork.ThuePhongRepository.GetByIdAsync(maThuePhong);
//                if (thuePhong == null || thuePhong.MaKhachHang != khachHang.MaKhachHang)
//                    return ServiceResult.Failure("Không tìm thấy thông tin đặt phòng.");

//                // ✅ LẤY HÓA ĐƠN THÔNG QUA MaHoaDon TRONG THUEPHONG
//                if (!thuePhong.MaHoaDon.HasValue)
//                    return ServiceResult.Failure("Không tìm thấy hóa đơn liên quan.");

//                var hoaDon = await _unitOfWork.HoaDonRepository.GetHoaDonWithDetailsAsync(thuePhong.MaHoaDon.Value);
//                if (hoaDon == null)
//                    return ServiceResult.Failure("Không tìm thấy hóa đơn.");

//                // Chỉ cho phép thanh toán lại nếu chưa thanh toán và hết hạn
//                if (hoaDon.TrangThai != "ChuaThanhToan")
//                    return ServiceResult.Failure("Hóa đơn đã được xử lý.");

//                if (hoaDon.NgayTao.AddMinutes(15) > DateTime.Now)
//                    return ServiceResult.Failure("Hóa đơn chưa hết hạn, vui lòng thanh toán bình thường.");

//                // ✅ TẠO HÓA ĐƠN MỚI VỚI THỜI GIAN MỚI  
//                var hoaDonMoi = new HoaDonDichVu
//                {
//                    MaHoaDon = Guid.NewGuid(),
//                    TenHoaDon = hoaDon.TenHoaDon + " (Thanh toán lại)",
//                    MoTaHoaDon = hoaDon.MoTaHoaDon,
//                    MaKhachHang = hoaDon.MaKhachHang,
//                    TongTienHoaDon = hoaDon.TongTienHoaDon,
//                    NgayTao = DateTime.Now,
//                    TrangThai = "ChuaThanhToan"
//                };

//                // Hủy hóa đơn cũ và tạo mới
//                await _unitOfWork.HoaDonRepository.UpdateHoaDonStatusAsync(hoaDon.MaHoaDon, "DaHuy");
//                var createdHoaDon = await _unitOfWork.HoaDonRepository.CreateAsync(hoaDonMoi);

//                // ✅ CẬP NHẬT MaHoaDon MỚI CHO THUEPHONG
//                await _unitOfWork.ThuePhongRepository.UpdateMaHoaDonAsync(thuePhong.MaThuePhong, createdHoaDon.MaHoaDon);

//                // Copy chi tiết hóa đơn
//                var chiTietCu = await _unitOfWork.ChiTietHoaDonDichVuRepository.GetChiTietByHoaDonAsync(hoaDon.MaHoaDon);
//                foreach (var chiTiet in chiTietCu)
//                {
//                    var chiTietMoi = new ChiTietHoaDonDichVu
//                    {
//                        MaHoaDon = createdHoaDon.MaHoaDon,
//                        MaSanPham = chiTiet.MaSanPham,
//                        SoLuong = chiTiet.SoLuong
//                    };
//                    await _unitOfWork.ChiTietHoaDonDichVuRepository.CreateAsync(chiTietMoi);
//                }

//                return ServiceResult.Success("Tạo thanh toán lại thành công.", new
//                {
//                    MaHoaDonMoi = createdHoaDon.MaHoaDon,
//                    thuePhong.MaThuePhong,
//                    HanThanhToan = DateTime.Now.AddMinutes(15)
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi thanh toán lại");
//                return ServiceResult.Failure($"Lỗi khi thanh toán lại: {ex.Message}");
//            }
//        }

//        #region Private Helper Methods

//        private async Task<ServiceResult> ValidateDatPhongAsync(DatPhongDTO datPhongDto)
//        {
//            // ✅ DELEGATE ROOM VALIDATION TO ORCHESTRATOR
//            var roomDetailResult = await _roomOrchestrator.GetRoomDetailWorkflowAsync(datPhongDto.MaPhong);
//            if (!roomDetailResult.IsSuccess)
//                return ServiceResult.Failure("Phòng hát không tồn tại.");

//            var roomDetail = (PhongHatDetailDTO)roomDetailResult.Data!;
            
//            // ✅ USE COMPUTED PROPERTIES FROM ENTITY
//         //   if (!roomDetail.IsAvailableForBooking)
//                return ServiceResult.Failure("Phòng hát không khả dụng để đặt.");

//            // Rest of validation logic...
//            return ServiceResult.Success();
//        }
//        private string GetTrangThaiDatPhong(ThuePhong thuePhong, HoaDonDichVu hoaDon)
//        {
//            // Nếu không có hóa đơn
//            if (hoaDon == null)
//                return "KhongCoHoaDon";

//            // ✅ DỰA VÀO TRẠNG THÁI HÓA ĐƠN THANH TOÁN
//            return hoaDon.TrangThai switch
//            {
//                "DaTha`nhToan" => thuePhong.TrangThai switch
//                {
//                    "DangSuDung" => "DangSuDung",
//                    "DaKetThuc" => "DaHoanThanh",
//                    _ => "DaThanhToan"
//                },
//                "ChuaThanhToan" => hoaDon.NgayTao.AddMinutes(15) < DateTime.Now
//                    ? "HetHanThanhToan"
//                    : "ChuaThanhToan",
//                "DaHuy" => "DaHuy",
//                _ => "KhongXacDinh"
//            };
//        }
//        #endregion
//    }
//}