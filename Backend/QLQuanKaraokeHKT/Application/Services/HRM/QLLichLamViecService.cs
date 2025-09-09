using AutoMapper;
using QLQuanKaraokeHKT.Application.Helpers;
using QLQuanKaraokeHKT.Core.Common;
using QLQuanKaraokeHKT.Core.DTOs.QLNhanSuDTOs;
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;
using QLQuanKaraokeHKT.Core.Interfaces.Services.HRM;

namespace QLQuanKaraokeHKT.Application.Services.HRM
{

    public class QLLichLamViecService : IQLLichLamViecService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISendEmailService _sendEmailService;
        private readonly ILogger<QLLichLamViecService> _logger;

        public QLLichLamViecService(IUnitOfWork unitOfWork, IMapper mapper, ISendEmailService sendEmailService, ILogger<QLLichLamViecService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _sendEmailService = sendEmailService;
            _logger = logger;
        }


        public async Task<ServiceResult> CreateLichLamViecAsync(AddLichLamViecDTO addLichLamViecDto)
        {
            try
            {
                if (addLichLamViecDto == null)
                {   
                    return ServiceResult.Failure("Thông tin lịch làm việc không hợp lệ.");
                }

                var lichLamViec = _mapper.Map<LichLamViec>(addLichLamViecDto);

                var createdLichLamViec = await _unitOfWork.ExecuteTransactionAsync(async () => {
                    await _unitOfWork.LichLamViecRepository.CreateAsync(lichLamViec);
                    return lichLamViec;
                });

                if (createdLichLamViec == null)
                {
                    return ServiceResult.Failure("Không thể tạo lịch làm việc do lỗi hệ thống.");
                }

                var result = _mapper.Map<LichLamViecDTO>(createdLichLamViec);
                return ServiceResult.Success("Tạo lịch làm việc thành công.", result);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi tạo lịch làm việc: {ex.Message}");
            }
        }


        public async Task<ServiceResult> GetAllLichLamViecAsync()
        {
            try
            {
                var lichLamViecs = await _unitOfWork.LichLamViecRepository.GetAllAsync();

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


        public async Task<ServiceResult> GetLichLamViecByNhanVienAsync(Guid maNhanVien)
        {
            try
            {
                if (maNhanVien == Guid.Empty)
                {
                    return ServiceResult.Failure("Mã nhân viên không hợp lệ.");
                }

                var lichLamViecs = await _unitOfWork.LichLamViecRepository.GetLichLamViecByNhanVienAsync(maNhanVien);

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



        public async Task<ServiceResult> GetLichLamViecByRangeAsync(DateOnly start, DateOnly end)
        {
            try
            {
                var lichLamViecs = await _unitOfWork.LichLamViecRepository.GetLichLamViecByRangeAsync(start, end);
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
                var result = await _unitOfWork.ExecuteTransactionAsync(async () => {
                    return await _unitOfWork.LichLamViecRepository.UpdateAsync(entity);
                });

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
                var result = await _unitOfWork.ExecuteTransactionAsync(async () => {
                    return await _unitOfWork.LichLamViecRepository.DeleteAsync(maLichLamViec);
                });

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
            var all = await _unitOfWork.LichLamViecRepository.GetLichLamViecByNhanVienAsync(maNhanVien);

            var filtered = all.Where(lv => lv.NgayLamViec >= start && lv.NgayLamViec <= end).ToList();
            if (!filtered.Any())
                return ServiceResult.Success("Không có lịch làm việc trong khoảng này.");
            var dtos = _mapper.Map<List<LichLamViecDTO>>(filtered);
            return ServiceResult.Success("Lấy lịch làm việc thành công.", dtos);
        }

        public async Task<ServiceResult> SendNotiWorkSchedulesAsync(DateOnly start, DateOnly end)
        {
            try
            {
                var lichLamViecList = await _unitOfWork.LichLamViecRepository.GetLichLamViecByRangeAsync(start, end);

                if (!lichLamViecList?.Any() == true)
                    return ServiceResult.Failure("Không có lịch làm việc nào trong khoảng thời gian này.");

                var nhanVienGroups = lichLamViecList!
                    .GroupBy(lv => lv.NhanVien)
                    .Where(g => !string.IsNullOrWhiteSpace(g.Key?.Email))
                    .ToList();

                if (!nhanVienGroups.Any())
                    return ServiceResult.Failure("Không có nhân viên nào có email để gửi thông báo.");

                int success = 0, fail = 0;

                foreach (var group in nhanVienGroups)
                {
                    var nhanVien = group.Key;
                    var lichTrongKhoang = group.OrderBy(lv => lv.NgayLamViec).ToList();

                    try
                    {
                        var emailContent = EmailContentHelper.CreateWorkScheduleNotificationEmail(
                            nhanVien.HoTen,
                            start,
                            end,
                            lichTrongKhoang);

                        var subject = EmailContentHelper.Subjects.WorkScheduleNotification;

                        await _sendEmailService.SendEmailByContentAsync(nhanVien.Email, subject, emailContent);
                        success++;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Lỗi gửi email cho nhân viên {MaNhanVien}", nhanVien.MaNv);
                        fail++;
                    }
                }

                return ServiceResult.Success($"Đã gửi email thành công cho {success} nhân viên, thất bại {fail}.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Lỗi hệ thống khi gửi thông báo lịch làm việc: {ex.Message}");
            }
        }
    }
}