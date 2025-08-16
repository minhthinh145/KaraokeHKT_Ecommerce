using AutoMapper;
using QLQuanKaraokeHKT.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.ExternalService.Interfaces;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.QLNhanSu.Interfaces;

namespace QLQuanKaraokeHKT.Services.QLNhanSuServices.QLLichLamViecServices
{
    /// <summary>
    /// Service implementation for managing LichLamViec (work schedule) operations.
    /// </summary>
    public class QLLichLamViecService : IQLLichLamViecService
    {
        private readonly ILichLamViecRepository _lichLamViecRepository;
        private readonly IMapper _mapper;
        private readonly ISendEmailService _sendEmailService;

        public QLLichLamViecService(ILichLamViecRepository lichLamViecRepository, IMapper mapper, ISendEmailService sendEmailService)
        {
            _lichLamViecRepository = lichLamViecRepository ?? throw new ArgumentNullException(nameof(lichLamViecRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _sendEmailService = sendEmailService;
        }

        /// <summary>
        /// Creates a new work schedule for an employee.
        /// </summary>
        public async Task<ServiceResult> CreateLichLamViecAsync(AddLichLamViecDTO addLichLamViecDto)
        {
            try
            {
                if (addLichLamViecDto == null)
                {
                    return ServiceResult.Failure("Thông tin lịch làm việc không hợp lệ.");
                }

                // Map DTO to entity
                var lichLamViec = _mapper.Map<LichLamViec>(addLichLamViecDto);

                // Create work schedule
                var createdLichLamViec = await _lichLamViecRepository.CreateLichLamViecWithNhanVienAsync(lichLamViec);

                if (createdLichLamViec == null)
                {
                    return ServiceResult.Failure("Không thể tạo lịch làm việc do lỗi hệ thống.");
                }

                // Map result to DTO
                var result = _mapper.Map<LichLamViecDTO>(createdLichLamViec);
                return ServiceResult.Success("Tạo lịch làm việc thành công.", result);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi tạo lịch làm việc: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all work schedules in the system.
        /// </summary>
        public async Task<ServiceResult> GetAllLichLamViecAsync()
        {
            try
            {
                var lichLamViecs = await _lichLamViecRepository.GetAllLichLamViecAsync();

                if (lichLamViecs == null || !lichLamViecs.Any())
                {
                    return ServiceResult.Failure("Không có lịch làm việc nào trong hệ thống.");
                }

                var lichLamViecDTOs = _mapper.Map<List<LichLamViecDTO>>(lichLamViecs);
                return ServiceResult.Success("Lấy danh sách lịch làm việc thành công.", lichLamViecDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi lấy danh sách lịch làm việc: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves work schedules for a specific employee.
        /// </summary>
        public async Task<ServiceResult> GetLichLamViecByNhanVienAsync(Guid maNhanVien)
        {
            try
            {
                if (maNhanVien == Guid.Empty)
                {
                    return ServiceResult.Failure("Mã nhân viên không hợp lệ.");
                }

                var lichLamViecs = await _lichLamViecRepository.GetLichLamViecByNhanVienAsync(maNhanVien);

                if (lichLamViecs == null || !lichLamViecs.Any())
                {
                    return ServiceResult.Failure("Không tìm thấy lịch làm việc cho nhân viên này.");
                }

                var lichLamViecDTOs = _mapper.Map<List<LichLamViecDTO>>(lichLamViecs);
                return ServiceResult.Success("Lấy lịch làm việc của nhân viên thành công.", lichLamViecDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi lấy lịch làm việc của nhân viên: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves all work in a specified date range.
        /// </summary>
        public async Task<ServiceResult> GetLichLamViecByRangeAsync(DateOnly start, DateOnly end)
        {
            try
            {
                var lichLamViecs = await _lichLamViecRepository.GetLichLamViecByRangeAsync(start, end);
                if (lichLamViecs == null || !lichLamViecs.Any())
                    return ServiceResult.Failure("Không có lịch làm việc trong khoảng thời gian này.");

                var lichLamViecDTOs = _mapper.Map<List<LichLamViecDTO>>(lichLamViecs);
                return ServiceResult.Success("Lấy lịch làm việc theo khoảng ngày thành công.", lichLamViecDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi lấy lịch làm việc theo khoảng ngày: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateLichLamViecAsync(LichLamViecDTO lichLamViecDto)
        {
            try
            {
                if (lichLamViecDto == null)
                    return ServiceResult.Failure("Dữ liệu lịch làm việc không hợp lệ.");

                var entity = _mapper.Map<LichLamViec>(lichLamViecDto);
                var result = await _lichLamViecRepository.UpdateLichLamViecAsync(entity);

                if (!result)
                    return ServiceResult.Failure("Cập nhật lịch làm việc thất bại.");

                return ServiceResult.Success("Cập nhật lịch làm việc thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi cập nhật lịch làm việc: {ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteLichLamViecAsync(int maLichLamViec)
        {
            try
            {
                var result = await _lichLamViecRepository.DeleteLichLamViecByIdAsync(maLichLamViec);
                if (!result)
                    return ServiceResult.Failure("Xóa lịch làm việc thất bại hoặc không tìm thấy.");

                return ServiceResult.Success("Xóa lịch làm việc thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi xóa lịch làm việc: {ex.Message}");
            }
        }
        public async Task<ServiceResult> GetLichLamViecByNhanVienAndRangeAsync(Guid maNhanVien, DateOnly start, DateOnly end)
        {
            var all = await _lichLamViecRepository.GetLichLamViecByNhanVienAsync(maNhanVien);
            var filtered = all.Where(lv => lv.NgayLamViec >= start && lv.NgayLamViec <= end).ToList();
            if (!filtered.Any())
                return ServiceResult.Failure("Không có lịch làm việc trong khoảng này.");
            var dtos = _mapper.Map<List<LichLamViecDTO>>(filtered);
            return ServiceResult.Success("Lấy lịch làm việc thành công.", dtos);
        }
        public async Task<ServiceResult> SendNotiWorkSchedulesAsync(DateOnly start, DateOnly end)
        {
            var nhanVienList = await _lichLamViecRepository.GetNhanVienByLichLamViecRangeAsync(start, end);
            if (nhanVienList == null || !nhanVienList.Any())
                return ServiceResult.Failure("Không có nhân viên nào có lịch làm việc trong khoảng thời gian này.");

            int success = 0, fail = 0;

            foreach (var nv in nhanVienList)
            {
                if (string.IsNullOrWhiteSpace(nv.Email)) { fail++; continue; }

                // Lấy lịch làm việc của nhân viên trong khoảng ngày
                var lichTrongKhoang = nv.LichLamViecs
                    .Where(lv => lv.NgayLamViec >= start && lv.NgayLamViec <= end)
                    .OrderBy(lv => lv.NgayLamViec)
                    .ToList();

                if (!lichTrongKhoang.Any()) { fail++; continue; }

                // Tạo nội dung email
                var lichText = string.Join("\n", lichTrongKhoang.Select(lv =>
                    $"- {lv.NgayLamViec:yyyy-MM-dd}: Ca {lv.CaLamViec?.TenCa } ({lv.CaLamViec?.GioBatDauCa:hh\\:mm} - {lv.CaLamViec?.GioKetThucCa:hh\\:mm})"
                ));

                var subject = $"Lịch làm việc từ {start:yyyy-MM-dd} đến {end:yyyy-MM-dd}";
                var body = $"Xin chào {nv.HoTen},\n\nBạn có lịch làm việc trong khoảng thời gian sau:\n{lichText}\n\nVui lòng đăng nhập để xem chi tiết hơn. Trân trọng - Quản lý nhân sự.";

                try
                {
                    await _sendEmailService.SendEmailByContentAsync(nv.Email, subject, body);
                    success++;
                }
                catch
                {
                    fail++;
                }
            }

            return ServiceResult.Success($"Đã gửi email thành công cho {success} nhân viên, thất bại {fail}.");
        }
    }
}